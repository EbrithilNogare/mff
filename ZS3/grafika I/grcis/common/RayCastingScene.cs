using MathSupport;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Utilities;

// Interfaces and objects for ray-based rendering.
namespace Rendering
{
  /// <summary>
  /// Set operations for CSG inner nodes.
  /// </summary>
  public enum SetOperation
  {
    Union,
    Intersection,
    Difference,
    Xor,
  }

  /// <summary>
  /// Builtin
  /// </summary>
  public class PropertyName
  {
    /// <summary>
    /// Surface property = base color.
    /// </summary>
    public static string COLOR = "color";

    /// <summary>
    /// Surface property = texture (multi-attribute).
    /// </summary>
    public static string TEXTURE = "texture";

    /// <summary>
    /// (Perhaps) globally used reflectance model.
    /// </summary>
    public static string REFLECTANCE_MODEL = "reflectance";

    /// <summary>
    /// Surface property: material description. Must match used reflectance model.
    /// </summary>
    public static string MATERIAL = "material";
  }

  /// <summary>
  /// General scene node (hierarchical 3D scene used in ray-based rendering).
  /// </summary>
  public interface ISceneNode: IIntersectable
  {
    /// <summary>
    /// Reference to a parent node (null for root node).
    /// </summary>
    ISceneNode Parent { get; set; }

    /// <summary>
    /// Transform from this space to parent's one.
    /// </summary>
    Matrix4d ToParent { get; set; }

    /// <summary>
    /// Transform from parent space to this node's space.
    /// </summary>
    Matrix4d FromParent { get; set; }

    /// <summary>
    /// Collection of node's children, can be null.
    /// </summary>
    ICollection<ISceneNode> Children { get; set; }

    /// <summary>
    /// True for object root (subject to animation). 3D texture coordinates should use Object space.
    /// </summary>
    bool ObjectRoot { get; set; }

    /// <summary>
    /// Retrieves value of the given Attribute. Looks in parent nodes if not found locally.
    /// </summary>
    /// <param name="name">Attribute name.</param>
    /// <returns>Attribute value or null if not found.</returns>
    object GetAttribute (string name);

    /// <summary>
    /// Retrieves value of the given Attribute. Looks only in this node.
    /// </summary>
    /// <param name="name">Attribute name.</param>
    /// <returns>Attribute value or null if not found.</returns>
    object GetLocalAttribute (string name);

    /// <summary>
    /// Sets the new value of the given attribute.
    /// </summary>
    /// <param name="name">Attribute name.</param>
    /// <param name="value">Attribute value.</param>
    void SetAttribute (string name, object value);

    /// <summary>
    /// Returns transform from the Local space (Solid) to the World space.
    /// </summary>
    /// <returns>Transform matrix.</returns>
    Matrix4d ToWorld ();

    /// <summary>
    /// Returns transform from the Local space (Solid) to the Object space (subject to animation).
    /// </summary>
    /// <returns>Transform matrix.</returns>
    Matrix4d ToObject ();

    /// <summary>
    /// Collects texture sequence in the right (application) order.
    /// </summary>
    /// <returns>Sequence of textures or null.</returns>
    LinkedList<ITexture> GetTextures ();
  }

  /// <summary>
  /// Elementary solid - atomic building block of a scene.
  /// </summary>
  public interface ISolid : ISceneNode
  {
    void GetBoundingBox (out Vector3d corner1, out Vector3d corner2);
  }

  /// <summary>
  /// Common code for ISceneNode.
  /// </summary>
  [Serializable]
  public abstract class DefaultSceneNode : ISceneNode
  {
    public static float infinityPlaceholder = 100000;

    protected LinkedList<ISceneNode> children;

    public ICollection<ISceneNode> Children
    {
      get => children;
      set
      {
        children.Clear();
        foreach (ISceneNode sn in value)
          children.AddLast(sn);
      }
    }

    public ISceneNode Parent { get; set; }

    public Matrix4d ToParent { get; set; }

    public Matrix4d FromParent { get; set; }

    public bool ObjectRoot { get; set; }

    protected Dictionary<string, object> attributes;

    public object GetAttribute (string name)
    {
      if (attributes != null)
      {
        object result;
        if (attributes.TryGetValue(name, out result))
          return result;
      }

      return Parent?.GetAttribute(name);
    }

    public object GetLocalAttribute (string name)
    {
      object result;
      return attributes == null ||
           !attributes.TryGetValue(name, out result)
        ? null
        : result;
    }

    public void SetAttribute (string name, object value)
    {
      if (attributes == null)
        attributes = new Dictionary<string, object>();
      attributes[name] = value;
    }

    /// <summary>
    /// Inserts one new child node to this parent node.
    /// </summary>
    /// <param name="ch">Child node to add.</param>
    /// <param name="toParent">Transform from local space of the child to the parent's space.</param>
    public virtual void InsertChild (ISceneNode ch, Matrix4d toParent)
    {
      children.AddLast(ch);
      ch.ToParent = toParent;
      toParent.Invert();
      ch.FromParent = toParent;
      ch.Parent = this;
    }

    /// <summary>
    /// Returns transform from the Local space (Solid) to the World space.
    /// </summary>
    /// <returns>Transform matrix.</returns>
    public Matrix4d ToWorld ()
    {
      return Parent == null ? Matrix4d.Identity : ToParent * Parent.ToWorld();
    }

    /// <summary>
    /// Returns transform from the Local space (Solid) to the Object space (subject to animation).
    /// </summary>
    /// <returns>Transform matrix.</returns>
    public Matrix4d ToObject ()
    {
      return ObjectRoot || Parent == null ? Matrix4d.Identity : ToParent * Parent.ToObject();
    }

    /// <summary>
    /// Collects texture sequence in the right (application) order.
    /// </summary>
    /// <returns>Sequence of textures or null.</returns>
    public LinkedList<ITexture> GetTextures ()
    {
      LinkedList<ITexture> result = null;
      if (Parent != null)
        result = Parent.GetTextures();

      object local = GetLocalAttribute ( PropertyName.TEXTURE );
      if (local == null)
        return result;

      if (local is ITexture)
      {
        if (result == null)
          result = new LinkedList<ITexture>();
        result.AddLast((ITexture)local);
      }
      else if (local is IEnumerable<ITexture>)
        if (result == null)
          result = new LinkedList<ITexture>((IEnumerable<ITexture>)local);
        else
          foreach (ITexture tex in (IEnumerable<ITexture>)local)
            result.AddLast(tex);

      return result;
    }

    /// <summary>
    /// Computes the complete intersection of the given ray with the object.
    /// </summary>
    /// <param name="p0">Ray origin.</param>
    /// <param name="p1">Ray direction vector.</param>
    /// <returns>Sorted list of intersection records.</returns>
    public virtual LinkedList<Intersection> Intersect (Vector3d p0, Vector3d p1)
    {
      if (children == null || children.Count == 0)
        return null;

      ISceneNode child  = children.First.Value;
      Vector3d   origin = Vector3d.TransformPosition( p0, child.FromParent );
      Vector3d   dir    = Vector3d.TransformVector( p1, child.FromParent );
      // ray in local child's coords: [ origin, dir ]

      return child.Intersect(origin, dir);
    }

    /// <summary>
    /// Complete all relevant items in the given Intersection object.
    /// </summary>
    /// <param name="inter">Intersection instance to complete.</param>
    public virtual void CompleteIntersection (Intersection inter)
    {}

    public DefaultSceneNode ()
    {
      children   = new LinkedList<ISceneNode>();
      attributes = null;
      ObjectRoot = false;
    }

    public void ShareCloneChildren (DefaultSceneNode n)
    {
      foreach (var child in children)
      {
        ITimeDependent cha = child as ITimeDependent;
        n.InsertChild((cha == null) ? child : (ISceneNode)cha.Clone(), child.ToParent);
      }
    }

    public void ShareCloneAttributes (DefaultSceneNode n)
    {
      if (attributes == null ||
          attributes.Count == 0)
        n.attributes = null;
      else
      {
        n.attributes = new Dictionary<string, object>();
        foreach (var kvp in attributes)
        {
          ICloneable vala = kvp.Value as ICloneable;
          n.attributes.Add(kvp.Key, (vala == null) ? kvp.Value : vala.Clone());
#if DEBUG
          if (vala != null)
            Util.Log("Clone Attribute: " + kvp.Key);
#endif
        }
      }
    }
  }

  /// <summary>
  /// CSG set operations in a inner scene node..
  /// </summary>
  [Serializable]
  public class CSGInnerNode : DefaultSceneNode
  {
    /// <summary>
    /// Delegate function for boolean operations
    /// </summary>
    //[Serializable]
    //public delegate bool BooleanOperation ( bool x, bool y );

    /// <summary>
    /// Current boolean operation.
    /// </summary>
    protected Operation bop;

    /// <summary>
    /// Does empty left operand kill the result?
    /// </summary>
    protected bool shortCurcuit;

    /// <summary>
    /// Empty right operand doesn't change anything..
    /// </summary>
    protected bool trivial;

    /// <summary>
    /// Optional bounding volume.
    /// </summary>
    public IBoundingVolume BoundingVolume;

    /// <summary>
    /// Number of ray x bounding-box intersections.
    /// </summary>
    public static long countBoundingBoxes = 0L;

    /// <summary>
    /// Number of ray x triangle intersections.
    /// </summary>
    public static long countTriangles = 0L;

    /// <summary>
    /// Number of faces in the scene.
    /// </summary>
    public static long countFaces = 0L;

    public CSGInnerNode (SetOperation op)
    {
      switch (op)
      {
        case SetOperation.Intersection:
          bop = new OperationIntersection();
          break;
        case SetOperation.Difference:
          bop = new OperationDifference();
          break;
        case SetOperation.Xor:
          bop = new OperationXor();
          break;
        case SetOperation.Union:
        default:
          bop = new OperationUnion();
          break;
      }

      // Set accelerator flags.
      shortCurcuit = !(bop.Result(false, false) || bop.Result(false, true));  // does empty left operand kill the result?
      trivial      = bop.Result(true, false) && !bop.Result(false, false);    // empty right operand doesn't change anything..
    }

    protected CSGInnerNode (Operation _bop)
    {
      bop = _bop;

      // set accelerator flags:
      shortCurcuit = !(bop.Result(false, false) || bop.Result(false, true));  // does empty left operand kill the result?
      trivial      = bop.Result(true, false) && !bop.Result(false, false);    // empty right operand doesn't change anything..
    }

    /// <summary>
    /// Reset all the counters.
    /// </summary>
    public static void ResetStatistics ()
    {
      Intersection.countRays =
      Intersection.countIntersections =
      countBoundingBoxes =
      countTriangles = 0L;
    }

    /// <summary>
    /// Not to be modified!
    /// </summary>
    protected static LinkedList<Intersection> emptyResult = new LinkedList<Intersection>();

    /// <summary>
    /// Computes the complete intersection of the given ray with the object.
    /// </summary>
    /// <param name="p0">Ray origin.</param>
    /// <param name="p1">Ray direction vector.</param>
    /// <returns>Sorted list of intersection records.</returns>
    public override LinkedList<Intersection> Intersect (Vector3d p0, Vector3d p1)
    {
      if (children == null || children.Count == 0)
        return null;

      if (BoundingVolume != null)
      {
        countBoundingBoxes++;
        if (BoundingVolume.Intersect(p0, p1) < -0.5)
          return null;
      }

      LinkedList<Intersection> result = null;
      LinkedList<Intersection> left   = null; // I'm going to reuse these two..

      bool leftOp = true; // the 1st pass => left operand

      foreach (ISceneNode child in children)
      {
        Vector3d origin = Vector3d.TransformPosition( p0, child.FromParent );
        Vector3d dir    = Vector3d.TransformVector( p1, child.FromParent );
        // ray in local child's coords: [ origin, dir ]

        LinkedList<Intersection> partial = child.Intersect( origin, dir );
        if (partial == null)
          partial = leftOp ? new LinkedList<Intersection>() : emptyResult;
        else if (child is ISolid)
          Intersection.countIntersections += partial.Count;

        if (leftOp)
        {
          leftOp = false;
          result = partial;
          left = new LinkedList<Intersection>();
        }
        else
        {
          if (trivial && partial.Count == 0)
            continue;

          // resolve one binary operation (result := left # partial):
          {
            LinkedList<Intersection> tmp = left;
            left = result;
            result = tmp;
          }
          // result .. empty so far
          result.Clear();

          Intersection leftFirst  = left.First?.Value;
          Intersection rightFirst = partial.First?.Value;
          // initial inside status values:
          bool insideLeft   = leftFirst != null && !leftFirst.Enter;
          bool insideRight  = rightFirst != null && !rightFirst.Enter;
          bool insideResult = bop.Result(insideLeft, insideRight);
          // merge behavior:

          while (leftFirst != null || rightFirst != null)
          {
            double leftVal  = leftFirst?.T  ?? double.PositiveInfinity;
            double rightVal = rightFirst?.T ?? double.PositiveInfinity;
            double lowestT  = Math.Min(leftVal, rightVal);
            Debug.Assert(!double.IsInfinity(lowestT));

            bool minLeft  = leftVal == lowestT;
            bool minRight = rightVal == lowestT;

            Intersection first = null;
            if (minRight)
            {
              first = rightFirst;
              partial.RemoveFirst();
              rightFirst = partial.First?.Value;
              insideRight = first.Enter;
            }

            if (minLeft)
            {
              first = leftFirst;
              left.RemoveFirst();
              leftFirst = left.First?.Value;
              insideLeft = first.Enter;
            }

            bool newResult = bop.Result(insideLeft, insideRight);

            if (newResult != insideResult)
            {
              first.Enter = insideResult = newResult;
              result.AddLast(first);
            }
          }
        }

        if (shortCurcuit && result.Count == 0)
          break;
      }

      return result;
    }
  }

  /// <summary>
  /// Default scene class for ray-based rendering.
  /// </summary>
  [System.Serializable]
  public class DefaultRayScene : IRayScene
  {
    /// <summary>
    /// Scene model (whatever is able to compute ray intersections).
    /// </summary>
    public IIntersectable Intersectable { get; set; }

    /// <summary>
    /// Background color.
    /// </summary>
    public double[] BackgroundColor { get; set; }

    /// <summary>
    /// Camera = primary ray generator.
    /// </summary>
    public ICamera Camera { get; set; }

    /// <summary>
    /// Set of light sources.
    /// </summary>
    public ICollection<ILightSource> Sources { get; set; }
  }

  /// <summary>
  /// Base implementation of time-dependent Ray-scene.
  /// </summary>
  public class AnimatedRayScene : DefaultRayScene, ITimeDependent
  {
    /// <summary>
    /// Starting (minimal) time in seconds.
    /// </summary>
    public double Start { get; set; }

    /// <summary>
    /// Ending (maximal) time in seconds.
    /// </summary>
    public double End { get; set; }

    /// <summary>
    /// Internal variable for the current time.
    /// </summary>
    protected double time;

    /// <summary>
    /// Changes the current time - internal routine.
    /// Override it if you need time-dependent background color..
    /// </summary>
    protected virtual void setTime (double newTime)
    {
      time = Arith.Clamp(newTime, Start, End);

      // Time-dependent scene?
      ITimeDependent intersectable = Intersectable as ITimeDependent;
      if (intersectable != null)
        intersectable.Time = time;

      // Time-dependent camera?
      ITimeDependent camera = Camera as ITimeDependent;
      if (camera != null)
        camera.Time = time;

      // Time-dependent light sources?
      foreach (ILightSource light in Sources)
      {
        ITimeDependent li = light as ITimeDependent;
        if (li != null)
          li.Time = time;
      }
    }

    /// <summary>
    /// Current time in seconds.
    /// </summary>
    public double Time
    {
      get => time;
      set => setTime(value);
    }

    /// <summary>
    /// Clone all the time-dependent components, share the others.
    /// </summary>
    /// <returns></returns>
    public virtual object Clone ()
    {
#if DEBUG
      Util.Log("Clone: AnimatedRayScene");
#endif
      AnimatedRayScene sc = new AnimatedRayScene ();

      sc.Intersectable = Intersectable;
      if (sc.Intersectable is ITimeDependent intersectable)
        sc.Intersectable = (IIntersectable)intersectable.Clone();

      sc.BackgroundColor = (double[])BackgroundColor.Clone();

      sc.Camera = Camera;
      if (sc.Camera is ITimeDependent camera)
        sc.Camera = (ICamera)camera.Clone();

      ILightSource[] tmp = new ILightSource[Sources.Count];
      Sources.CopyTo(tmp, 0);
      for (int i = 0; i < tmp.Length; i++)
      {
        if (tmp[i] is ITimeDependent source)
          tmp[i] = (ILightSource)source.Clone();
      }
      sc.Sources = new LinkedList<ILightSource>(tmp);

      sc.Start = Start;
      sc.End = End;
      sc.setTime(Time); // propagates the current time to all time-dependent components..

      return sc;
    }

    /// <summary>
    /// Creates default ray-rendering scene.
    /// </summary>
    public AnimatedRayScene ()
    {
      Start = 0.0;
      End   = 10.0;
      time  = 0.0;
    }
  }

  [Serializable]
  public abstract class Operation
  {
    public abstract bool Result (bool x, bool y);
  }

  [Serializable]
  public class OperationUnion : Operation
  {
    public override bool Result (bool x, bool y) => x || y;
  }

  [Serializable]
  public class OperationIntersection : Operation
  {
    public override bool Result (bool x, bool y) => x && y;
  }

  [Serializable]
  public class OperationDifference : Operation
  {
    public override bool Result (bool x, bool y) => x && !y;
  }

  [Serializable]
  public class OperationXor : Operation
  {
    public override bool Result (bool x, bool y) => x ^ y;
  }
}
