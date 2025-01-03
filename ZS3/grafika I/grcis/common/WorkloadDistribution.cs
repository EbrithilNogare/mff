﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Threading;
using MathSupport;

namespace Rendering
{
  /// <summary>
  /// Takes care of distribution of rendering work between local threads and remote/network render clients
  /// </summary>
  public class Master
  {
    public static Master singleton;

    public ConcurrentQueue<Assignment> availableAssignments;

    public List<NetworkWorker> networkWorkers;

    private Thread[] pool;

    public Thread mainRenderThread;

    public int totalNumberOfAssignments;

    public int finishedAssignments;

    // width and height of one block of pixels (rendered at one thread at the time); 64 seems to be optimal; should be power of 2 and larger than 8 (=stride)
    public const int assignmentSize = 64;

    public Progress progressData;

    public int assignmentRoundsFinished;
    public int assignmentRoundsTotal;

    public Bitmap    bitmap;
    public IRayScene scene;
    public IRenderer renderer;

    private readonly IEnumerable<Client> clientsCollection;

    public PointCloud pointCloud;

    private readonly int threads;

    /// <summary>
    /// Constructor which takes also care of initializing assignments
    /// </summary>
    /// <param name="bitmap">Main bitmap - used also in PictureBox in Form1</param>
    /// <param name="scene">Scene to render</param>
    /// <param name="renderer">Rendered to use for RenderPixel method</param>
    /// <param name="clientsCollection">Collection of Clients to get their IP addresses - names are only for user</param>
    /// <param name="threads">Number of threads to be used for rendering</param>
    /// <param name="newPointCloud">Bool whether new point cloud should be created</param>
    /// <param name="pointCloudStorage">External storage of point cloud data</param>
    public Master ( Bitmap bitmap, IRayScene scene, IRenderer renderer, IEnumerable<Client> clientsCollection, 
                    int threads, bool newPointCloud, ref PointCloud pointCloudStorage)
    {
      finishedAssignments = 0;
   
      this.scene = scene;
      this.renderer = renderer;
      this.bitmap = bitmap;
      this.clientsCollection = clientsCollection;
      this.threads = threads;

      Assignment.assignmentSize = assignmentSize;

      if ( newPointCloud && !MT.pointCloudSavingInProgress )
      {
        pointCloud = new PointCloud ( threads );
        pointCloudStorage = pointCloud;
      }
      else
      {
        pointCloud = pointCloudStorage;
      }

      singleton = this;
    }

    /// <summary>
    /// Creates threadpool and starts all threads on Consume method
    /// Thread which calls this method will take care of preparing assignments and receiving rendered images from RenderClients meanwhile
    /// </summary>   
    public void StartThreads ()
    {
      pool = new Thread[threads];

      AssignNetworkWorkerToStream ();

      WaitHandle[] waitHandles = new WaitHandle[threads];

      for ( int i = 0; i < threads; i++ )
      {
        EventWaitHandle handle = new EventWaitHandle ( false, EventResetMode.ManualReset );

        int i1 = i;
        Thread newThread = new Thread ( () =>
        {
          MT.threadID = i1;
          Consume();
          handle.Set();         
        } );

        newThread.Name = "RenderThread #" + i;
        pool [ i ] = newThread;
        newThread.Start ();

        waitHandles [ i ] = handle;
      }

      mainRenderThread = pool [ 0 ];

      Thread imageReceiver = new Thread ( RenderedImageReceiver );
      imageReceiver.Name = "ImageReceiver";
      imageReceiver.Start ();

      WaitHandle.WaitAll ( waitHandles );
      
      if ( networkWorkers?.Count > 0 )
      {
        foreach ( NetworkWorker worker in networkWorkers ) // sends ending assignment to all clients
        {
          worker.SendSpecialAssignment ( Assignment.AssignmentType.Ending );
        }
      }
    }

    /// <summary>
    /// Consumer-producer based multithreading work distribution
    /// Each thread waits for a new Assignment to be added to availableAssignments queue
    /// Most of the time is number of items in availableAssignments expected to be several times larger than number of threads
    /// </summary>
    protected void Consume ()
    {
      MT.InitThreadData ();

      while ( !availableAssignments.IsEmpty || finishedAssignments < totalNumberOfAssignments - threads || NetworkWorker.assignmentsAtClients > 0 )
      {
        availableAssignments.TryDequeue ( out Assignment newAssignment );

        if ( !progressData.Continue ) // test whether rendering should end (Stop button pressed) 
          return;

        if ( newAssignment == null ) // TryDequeue was not successful
          continue;

        float[] colorArray = newAssignment.Render ( false, renderer, progressData );
        BitmapMerger ( colorArray, newAssignment.x1, newAssignment.y1, newAssignment.x2 + 1, newAssignment.y2 + 1 );

        if ( newAssignment.stride == 1 )
        {
          finishedAssignments++;
          assignmentRoundsFinished++;
        }
        else
        {
          newAssignment.stride = newAssignment.stride >> 1; // stride values: 8 > 4 > 2 > 1
          assignmentRoundsFinished++;
          availableAssignments.Enqueue ( newAssignment );
        }
      }
    }

    /// <summary>
    /// Creates new assignments based on width and heigh of bitmap and assignmentSize
    /// </summary>
    /// <param name="bitmap">Main bitmap - used also in PictureBox in Form1</param>
    /// <param name="scene">Scene to render</param>
    /// <param name="renderer">Rendered to use for RenderPixel method</param>
    public void InitializeAssignments ( Bitmap bitmap, IRayScene scene, IRenderer renderer )
    {
      availableAssignments = new ConcurrentQueue<Assignment> ();

      int numberOfAssignmentsOnWidth = bitmap.Width % assignmentSize == 0
        ? bitmap.Width / assignmentSize
        : bitmap.Width / assignmentSize + 1;

      int numberOfAssignmentsOnHeight = bitmap.Height % assignmentSize == 0
        ? bitmap.Height / assignmentSize
        : bitmap.Height / assignmentSize + 1;


      for ( int y = 0; y < numberOfAssignmentsOnHeight; y++ )
      {
        for ( int x = 0; x < numberOfAssignmentsOnWidth; x++ )
        {
          int localX = x * assignmentSize;
          int localY = y * assignmentSize;

          Assignment newAssignment = new Assignment ( localX,
                                                      localY,
                                                      localX + assignmentSize - 1,
                                                      localY + assignmentSize - 1,
                                                      bitmap.Width, 
                                                      bitmap.Height );
          availableAssignments.Enqueue ( newAssignment );
        }
      }


      totalNumberOfAssignments = availableAssignments.Count;

      // number of assignments * number of renderings of each assignments (depends on stride)
      assignmentRoundsTotal = totalNumberOfAssignments * (int)( Math.Log ( Assignment.startingStride, 2 ) + 1 );  
    }

    /// <summary>
    /// Goes through all clients from RenderClientsForm and assigns a NetworkWorker to each of them
    /// </summary>
    public void AssignNetworkWorkerToStream ()
    {
      if ( clientsCollection == null )
      {
        return;
      }

      networkWorkers = new List<NetworkWorker> ();

      foreach ( Client client in clientsCollection )
      {
        NetworkWorker newWorker = new NetworkWorker ( client.address );        

        if ( newWorker.ConnectToClient () )
        {
          try
          {
            newWorker.ExchangeNecessaryInfo ();
          }
          catch ( IOException ) //thrown usually in case when stream is closed while exchanging necessary data
          {
            continue;
          }

          networkWorkers.Add ( newWorker );

          for ( int i = 0; i < newWorker.threadCountAtClient + 2; i++ ) //to each client is sent enough assignments to fill all threads + 2 for reserve
          {
            newWorker.TryToGetNewAssignment ();
          }
        }
      }
    }

    /// <summary>
    /// Adds colors represented in colorBuffer array to main bitmap
    /// </summary>
    /// <param name="colorBuffer">Float values (possible to be used for HDR) representing pixel color values</param>
    /// <param name="x1">X coordinate of left upper corner in main picture</param>
    /// <param name="y1">Y coordinate of left upper corner</param>
    /// <param name="x2">X coordinate of right lower corner</param>
    /// <param name="y2">Y coordinate of right lower corner</param>
    public void BitmapMerger ( float[] colorBuffer, int x1, int y1, int x2, int y2 )
    {
      lock ( bitmap )
      {
        int arrayPosition = 0;

        for ( int y = y1; y < Math.Min ( y2, bitmap.Height ); y++ )
        {
          for ( int x = x1; x < x2; x++ )
          {
            if ( !float.IsInfinity ( colorBuffer [ arrayPosition ] ) && x < bitmap.Width )  // positive infinity is indicator that color for this pixel is already present in bitmap and is final
            {
              Color color = Color.FromArgb ( Math.Min ( (int) colorBuffer [ arrayPosition ], 255 ),
                                             Math.Min ( (int) colorBuffer [ arrayPosition + 1 ], 255 ),
                                             Math.Min ( (int) colorBuffer [ arrayPosition + 2 ], 255 ) );
              bitmap.SetPixel ( x, y, color );
            }

            arrayPosition += 3;
          }
        }
      }
    }

    /// <summary>
    /// Goes through NetworkStreams of all clients and checks if there is rendered image pending in them
    /// Plus it takes care of that data via ReceiveRenderedImage method
    /// </summary>
    public void RenderedImageReceiver ()
    {
      if ( networkWorkers == null || networkWorkers.Count == 0 )
      {
        return;
      }

      foreach ( NetworkWorker t in networkWorkers )
      {
        t?.ReceiveRenderedImageAsync ();
      }
    }
  }

  /// <summary>
  /// Takes care of network communication with with 1 render client
  /// </summary>
  public class NetworkWorker
  {
    private readonly IPAddress  ipAdr;
    private          IPEndPoint endPoint;
    private const    int        port = 5411;

    public TcpClient     client;
    public NetworkStream stream;

    public int threadCountAtClient;
    public static int assignmentsAtClients;

    private readonly List<Assignment> unfinishedAssignments = new List<Assignment>();

    public CancellationTokenSource imageReceivalCancelSource;
    private readonly CancellationToken imageReceivalCancelToken;

    public NetworkWorker ( IPAddress ipAdr )
    {
      this.ipAdr = ipAdr;

      imageReceivalCancelSource = new CancellationTokenSource ();
      imageReceivalCancelToken = imageReceivalCancelSource.Token;
    }

    /// <summary>
    /// Establishes NetworkStream with desired client
    /// </summary>
    /// <returns>True if connection was succesfull, False otherwise</returns>
    public bool ConnectToClient ()
    {
      if ( ipAdr == null || ipAdr.ToString() == "0.0.0.0")
        return false;

      if ( endPoint == null )
        endPoint = new IPEndPoint ( ipAdr, port );

      client = new TcpClient ();

      try
      {
        client.Connect ( endPoint );
      }
      catch ( SocketException )
      {
        return false;
      }

      stream = client.GetStream ();

      // needed just in case - large portions of data are expected to be transfered at the same time (one rendered assignment is 50kB)
      client.ReceiveBufferSize = 1024 * 1024;
      client.SendBufferSize    = 1024 * 1024;

      return true;
    }


    /// <summary>
    /// Sends all objects which are necessary to render scene to client
    /// Assignment - special type of assignment (reset) to indicate (new) start of work
    /// IRayScene - scene representation like solids, textures, lights, camera, ...
    /// IRenderer - renderer itself including IImageFunction; needed for RenderPixel method
    /// Receives number of threads available to render at RenderClient
    /// </summary>
    public void ExchangeNecessaryInfo ()
    {
      // set assemblies - needed for correct serialization/deserialization
      NetworkSupport.SendString ( Assembly.GetExecutingAssembly ().GetName ().Name, stream );
      string targetAssembly = NetworkSupport.ReceiveString ( stream );
      NetworkSupport.SetAssemblyNames ( Assembly.GetExecutingAssembly ().GetName ().Name, targetAssembly );

      NetworkSupport.SendObject<Assignment> ( new Assignment ( Assignment.AssignmentType.Reset ), stream );     

      NetworkSupport.SendObject<IRayScene> ( Master.singleton.scene, stream );

      NetworkSupport.SendObject<IRenderer> ( Master.singleton.renderer, stream );

      threadCountAtClient = NetworkSupport.ReceiveInt ( stream );
    }

    
    private const int bufferSize = ( Master.assignmentSize * Master.assignmentSize * 3 + 2) * sizeof ( float );
    /// <summary>
    /// Asynchronous image receiver uses NetworkStream.ReadAsync
    /// Recursively called at the end
    /// If so, it reads it as - expected format is array of floats
    ///   - first 2 floats represents x1 and y1 coordinates - position in main bitmap;
    ///   - rest of array are colors of rendered bitmap - 3 floats (RGB values) per pixel;
    ///   - stored per lines from left upper corner (coordinates position)
    /// </summary>
    public async void ReceiveRenderedImageAsync ()
    {
      while ( Master.singleton.finishedAssignments < Master.singleton.totalNumberOfAssignments && Master.singleton.progressData.Continue )
      {
        if ( !NetworkSupport.IsConnected ( client ) )
        {
          LostConnection ();
          return;
        }

        byte[] receiveBuffer = new byte[bufferSize];

        try
        {
          await stream.ReadAsync ( receiveBuffer, 0, bufferSize, imageReceivalCancelToken );
        }
        catch ( IOException )
        {
          LostConnection ();
          return;
        }

        //uses parts of receiveBuffer - separates and converts data to coordinates and floats representing colors of pixels
        float[] coordinates = new float[2];
        float[] floatBuffer = new float[Master.assignmentSize * Master.assignmentSize * 3];
        Buffer.BlockCopy ( receiveBuffer, 0, coordinates, 0, 2 * sizeof ( float ) );
        Buffer.BlockCopy ( receiveBuffer, 2 * sizeof ( float ), floatBuffer, 0, floatBuffer.Length * sizeof ( float ) );

        Master.singleton.BitmapMerger ( floatBuffer,
                                       (int) coordinates[0],
                                       (int) coordinates[1],
                                       (int) coordinates[0] + Master.assignmentSize,
                                       (int) coordinates[1] + Master.assignmentSize );

        Master.singleton.finishedAssignments++;

        //takes care of increasing assignmentRoundsFinished by the ammount of finished rendering rounds on RenderClient
        lock (unfinishedAssignments)
        {
          Assignment currentAssignment = unfinishedAssignments.Find(a => a.x1 == (int)coordinates[0] && a.y1 == (int)coordinates[1]);
          int roundsFinished = (int)Math.Log(currentAssignment.stride, 2) + 1; // stride goes from 8 > 4 > 2 > 1 (1 step = 1 rendering round)
          Master.singleton.assignmentRoundsFinished += roundsFinished;

          assignmentsAtClients--;

          RemoveAssignmentFromUnfinishedAssignments((int)coordinates[0], (int)coordinates[1]);
        }        

        TryToGetNewAssignment ();
      }
    }

    /// <summary>
    /// Takes care of flushing unfinished assignments back to availableAssignments queue in Master,
    /// removes this NetworkWorker from networkWorkers list and properly closes TCP connection
    /// Useful in case of lost connection to the client
    /// </summary>
    private void LostConnection ()
    {
      ResetUnfinishedAssignments ();
      Master.singleton.networkWorkers.Remove ( this );
      client.Close ();
    }

    /// <summary>
    /// Removes assignments identified by its coordinates of left upper corner (x1 and y1) from unfinishedAssignments list
    /// </summary>
    /// <param name="x">Compared to value of x1 in assignment</param>
    /// <param name="y">Compared to value of y1 in assignment</param>
    private void RemoveAssignmentFromUnfinishedAssignments ( int x, int y )
    {
      for ( int i = 0; i < unfinishedAssignments.Count; i++ )
      {
        if ( unfinishedAssignments[i].x1 == x && unfinishedAssignments[i].y1 == y ) // assignments are uniquely indentified by coordinates of left upper corner
        {
          unfinishedAssignments.RemoveAt ( i );
          return;
        }
      }

      unfinishedAssignments.Clear ();
    }

    /// <summary>
    /// Loops until it gets a new assignment and sends it to the RenderClient (or all assignments have been rendered)
    /// </summary>
    public void TryToGetNewAssignment ()
    {
      while ( Master.singleton.finishedAssignments < Master.singleton.totalNumberOfAssignments - assignmentsAtClients )
      {
        if ( !NetworkSupport.IsConnected ( client ) )
        {
          LostConnection ();
          return;
        }

        Master.singleton.availableAssignments.TryDequeue ( out Assignment newAssignment );

        if ( !Master.singleton.progressData.Continue ) // test whether rendering should end (Stop button pressed) 
          return;

        if ( newAssignment == null ) // TryDequeue was not succesfull
          continue;

        lock ( stream )
        {
          NetworkSupport.SendObject<Assignment> ( newAssignment, stream );
          assignmentsAtClients++;
          unfinishedAssignments.Add ( newAssignment );
        }

        break;
      }
    }

    /// <summary>
    /// Moves all unfinished assignments cached in local unfinishedAssignments list to the main availableAssignments queue
    /// Useful in case of lost connection to the client
    /// </summary>
    private void ResetUnfinishedAssignments ()
    {
      foreach ( Assignment unfinishedAssignment in unfinishedAssignments )
      {
        Master.singleton.availableAssignments.Enqueue ( unfinishedAssignment );
        assignmentsAtClients--;
      }
    }

    /// <summary>
    /// Sends special Assignment which can signal to client that rendering stopped or that client should be reset and wait for more work 
    /// </summary>
    public void SendSpecialAssignment ( Assignment.AssignmentType assignmentType )
    {
      Assignment newAssignment = new Assignment ( assignmentType );

      lock ( stream )
      {
        NetworkSupport.SendObject<Assignment> ( newAssignment, stream );
      }
    }
  }

  /// <summary>
  /// Represents 1 render work (= square of pixels to render at specific stride)
  /// </summary>
  [Serializable]
  public class Assignment
  {
    public int x1, y1, x2, y2;

    public static int assignmentSize;

    public int stride; // stride of 'n' means that only each 'n'-th pixel is rendered (for sake of dynamic rendering)

    private readonly int bitmapWidth, bitmapHeight;

    public AssignmentType type;

    public const int startingStride = 8;

    /// <summary>
    /// Main constructor for standard assignments
    /// </summary>
    public Assignment ( int x1, int y1, int x2, int y2, int bitmapWidth, int bitmapHeight)
    {
      this.x1 = x1;
      this.y1 = y1;
      this.x2 = x2;
      this.y2 = y2;
      this.bitmapWidth = bitmapWidth;
      this.bitmapHeight = bitmapHeight;
      this.type = AssignmentType.Standard;

      // stride values: 8 > 4 > 2 > 1; initially always 8
      // decreases at the end of rendering of current assignment and therefore makes another render of this assignment more detailed
      stride = startingStride;
    }

    /// <summary>
    /// Constructor used for special assignments with AssignmentType other than Standard
    /// Special assignments are used to indicate end of rendering, request to reset render client, ...
    /// Coordinates, dimensions and stride are irrelevant in this case - they are set to -1 for error checking
    /// </summary>
    /// <param name="type"></param>
    public Assignment ( AssignmentType type )
    {
      x1 = y1 = x2 = y2 = bitmapWidth = bitmapHeight = stride = -1;

      this.type = type;
    }

    /// <summary>
    /// Main render method
    /// </summary>
    /// <param name="renderEverything">True if you want to ignore stride and just render everything at once (removes dynamic rendering effect; distributed network rendering)</param>
    /// <param name="renderer">IRenderer which will be used for RenderPixel method</param>
    /// <param name="progressData">Used for sync of bitmap with main PictureBox</param>
    /// <returns>Float array which represents colors of pixels (3 floats per pixel - RGB channel)</returns>
    public float[] Render ( bool renderEverything, IRenderer renderer, Progress progressData = null )
    {
      float[] returnArray = new float[assignmentSize * assignmentSize * 3];

      int previousStride = stride;

      if ( renderEverything )
      {
        stride = 1;

        if ( previousStride == stride )
        {
          renderEverything = false;
        }
      }

      for ( int y = y1; y <= y2; y += stride )
      {
        for ( int x = x1; x <= x2; x += stride )
        {
          double[] color      = new double[3];
          float[]  floatColor = new float[3];

          // removes the need to make assignments of different sizes to accommodate bitmaps with sides indivisible by assignment size
          if ( y >= bitmapHeight || x >= bitmapWidth )
            break;

          // prevents rendering of already rendered pixels
          if ( ( stride == 8 ||
                 ( y % ( stride << 1 ) != 0 ) ||
                 ( x % ( stride << 1 ) != 0 ) )
             ||
               renderEverything )
          {
            renderer.RenderPixel ( x, y, color ); // called at desired IRenderer; gets pixel color

            floatColor [ 0 ] = (float) color [ 0 ];
            floatColor [ 1 ] = (float) color [ 1 ];
            floatColor [ 2 ] = (float) color [ 2 ];
          }
          else
          {
            // positive infinity is used to signal BitmapMerger that color for this pixel is already present in main bitmap and is final (therefore no need for change)
            floatColor [ 0 ] = float.PositiveInfinity;  
            floatColor [ 1 ] = float.PositiveInfinity;
            floatColor [ 2 ] = float.PositiveInfinity;
          }

          if ( stride == 1 )  // apply color only to one pixel
          {
            returnArray [ PositionInArray ( x, y ) ]     = floatColor [ 0 ] * 255;
            returnArray [ PositionInArray ( x, y ) + 1 ] = floatColor [ 1 ] * 255;
            returnArray [ PositionInArray ( x, y ) + 2 ] = floatColor [ 2 ] * 255;
          }
          else // apply same color to multiple neighbour pixels (rectangle with current pixel in top left and length of side equal to stride value)
          {
            for ( int j = y; j < y + stride; j++ )
            {
              if ( j < bitmapHeight )
              {
                for ( int i = x; i < x + stride; i++ )
                {
                  if ( i < bitmapWidth )
                  {
                    returnArray [ PositionInArray ( i, j ) ]     = floatColor[0] * 255;
                    returnArray [ PositionInArray ( i, j ) + 1 ] = floatColor[1] * 255;
                    returnArray [ PositionInArray ( i, j ) + 2 ] = floatColor[2] * 255;
                  }
                }
              }
            }
          }

          if ( progressData != null ) // progressData is not used for distributed network rendering - null value used in rendering in RenderClients
          {
            lock ( Master.singleton.progressData )
            {
              // test whether rendering should end (Stop button pressed) 
              if ( !Master.singleton.progressData.Continue )
                return returnArray;

              // synchronization of bitmap with PictureBox in Form and update of progress (percentage of done work)
              if ( Master.singleton.mainRenderThread == Thread.CurrentThread )
              {
                Master.singleton.progressData.Finished = Master.singleton.assignmentRoundsFinished / (float) Master.singleton.assignmentRoundsTotal;
                Master.singleton.progressData.Sync ( Master.singleton.bitmap );
              }
            }
          }         
        }
      }

      return returnArray;
    }

    /// <summary>
    /// Computes position of coordinates (bitmap) into one-dimensional array of floats (with 3 floats (RGB channels) per pixel)
    /// </summary>
    /// <param name="x">X coordinate in bitmap</param>
    /// <param name="y">Y coordinate in bitmap</param>
    /// <returns>Position in array of floats</returns>
    private int PositionInArray ( int x, int y )
    {
      return ( ( y - y1 ) * assignmentSize + ( x - x1 ) ) * 3;
    }

    /// <summary>
    /// Standard - normal assignment with valid coordinates, dimensions and stride
    /// Ending - special assignment which tells to RenderClient that rendering is dome/no more work should be expected
    /// Reset - special asssignment which tells to RenderClient that it should reset and expect new render work
    /// </summary>
    public enum AssignmentType
    {
      Standard, Ending, Reset
    }
  }

  /// <summary>
  /// Represents one render client - name and IPaddress got from clientsDataGrid
  /// </summary>
  public class Client
  {
    public  IPAddress address;

    public string Name { get; set; }

    public string AddressString // string representation for the need of text in clientsDataGrid
    {
      get => GetIPAddress ();
      set => CheckAndSetIPAddress ( value );
    }

    private void CheckAndSetIPAddress ( string value )
    {
      value = value.Trim ();

      if ( value.ToLower() == "localhost" || value.ToLower() == "local" || value.ToLower () == "l" )
      {
        address = IPAddress.Parse ( "127.0.0.1" );
        return;
      }

      bool isValidIP = IPAddress.TryParse ( value, out address );

      if ( !isValidIP )
      {
        address = IPAddress.Parse ( "0.0.0.0" );
      }
    }

    private string GetIPAddress ()
    {
      if ( address == null )
      {
        return "";
      }

      if ( address.ToString () == "0.0.0.0" )
      {
        return "Invalid IP Address!";
      }
      else
      {
        return address.ToString ();
      }
    }
  }
}
