using System.Drawing;
using MathSupport;
using Raster;
using System;
using System.Collections.Generic;

namespace Utilities
{
  public class ImageRoundedHistogram
  {
    /// <summary>
    /// Cached histogram data.
    /// </summary>
    protected static bool[,] histArray = new bool[256, 64];

    // Graph appearance (just an example of second visualization option
    // read from param string).
    protected static bool alt = false;

    /// <summary>
    /// Draws the current histogram to the given raster image.
    /// </summary>
    /// <param name="graph">Result image (already scaled to the desired size).</param>
    public static void DrawHistogram (
      Bitmap graph)
    {
      if (histArray == null)
        return;

      using (Graphics gfx = Graphics.FromImage(graph))
      {
        gfx.Clear(Color.White);

        // Graph scaling:
        int centerX = graph.Width/2;
        int centerY = graph.Height/2;
        int diameter = Math.Min(graph.Height/2, graph.Width/2);
        
        // Histogram:
        for (int hue = 0; hue <= (histArray.GetLength(0) - 1); hue++)
        {
          for (int saturation = 0; saturation <= (histArray.GetLength(1)-1); saturation++)
          {

            if (!histArray[hue, saturation])
              continue;

            HsvToRgb(hue * 360 / (histArray.GetLength(0) - 1), (double)saturation / (histArray.GetLength(1)-1), 1.0, out int r, out int g, out int b);
            Color c = Color.FromArgb(255, r, g, b);
            Pen graphPen = new Pen(c);
            SolidBrush graphBrush = new SolidBrush(c);

            double angle = Math.PI * 2 * hue / (histArray.GetLength(0)-1);

            float x1 = (float)Math.Sin(angle)*saturation/(histArray.GetLength(1)-1)*diameter + centerX;
            float y1 = (float)Math.Cos(angle)*saturation/(histArray.GetLength(1)-1)*diameter + centerY;

            float x2 = (float)Math.Sin(angle+Math.PI * 2 / (histArray.GetLength(0)-1))*saturation/(histArray.GetLength(1)-1)*diameter + centerX;
            float y2 = (float)Math.Cos(angle+Math.PI * 2 / (histArray.GetLength(0)-1))*saturation/(histArray.GetLength(1)-1)*diameter + centerY;

            float x3 = (float)Math.Sin(angle+Math.PI * 2 / (histArray.GetLength(0)-1))*(saturation+1)/(histArray.GetLength(1)-1)*diameter + centerX;
            float y3 = (float)Math.Cos(angle+Math.PI * 2 / (histArray.GetLength(0)-1))*(saturation+1)/(histArray.GetLength(1)-1)*diameter + centerY;

            float x4 = (float)Math.Sin(angle)*(saturation+1)/(histArray.GetLength(1)-1)*diameter + centerX;
            float y4 = (float)Math.Cos(angle)*(saturation+1)/(histArray.GetLength(1)-1)*diameter + centerY;

            PointF point1 = new PointF(x1, y1);
            PointF point2 = new PointF(x2, y2);
            PointF point3 = new PointF(x3, y3);
            PointF point4 = new PointF(x4, y4);
            PointF[] curvePoints = {point1, point2, point3, point4};

            // Draw polygon to screen.
            gfx.FillPolygon(graphBrush, curvePoints);
          }
        }
      }
    }

    /// <summary>
    /// Recomputes image histogram and draws the result in the given raster image.
    /// </summary>
    /// <param name="input">Input image.</param>
    /// <param name="param">Textual parameter.</param>
    public static void ComputeHistogram (
      Bitmap input,
      string param)
    {
      // Text parameters:
      param = param.ToLower().Trim();

      // Graph appearance:
      alt = param.IndexOf("alt") >= 0;

      int x, y;

      // 1. Histogram recomputation.
      int size = int.TryParse(param, out size) ? size : 64;
      size = Math.Min(Math.Max(size,2),255);
      histArray = new bool[histArray.GetLength(0), size];

      int width = input.Width;
      int height = input.Height;
      
      for (y = 0; y < height; y++)
        for (x = 0; x < width; x++)
        {
          Color color = input.GetPixel( x, y );

          byte hue = (byte)(color.GetHue()*(histArray.GetLength(0)-1)/360.0);
          byte saturation = (byte)(color.GetSaturation()*(histArray.GetLength(1)-1));

          histArray[hue, saturation]=true;
        }
    }

    /// <summary>
    /// Convert HSV to RGB
    /// h is from 0-360
    /// s,v values are 0-1
    /// r,g,b values are 0-255
    /// </summary>
    static void HsvToRgb (double h, double S, double V, out int r, out int g, out int b)
    {
      // ######################################################################
      // T. Nathan Mundhenk
      // mundhenk@usc.edu
      // C/C++ Macro HSV to RGB

      double H = h;
      while (H < 0)
      { H += 360; };
      while (H >= 360)
      { H -= 360; };
      double R, G, B;
      if (V <= 0)
      { R = G = B = 0; }
      else if (S <= 0)
      {
        R = G = B = V;
      }
      else
      {
        double hf = H / 60.0;
        int i = (int)Math.Floor(hf);
        double f = hf - i;
        double pv = V * (1 - S);
        double qv = V * (1 - S * f);
        double tv = V * (1 - S * (1 - f));
        switch (i)
        {

          // Red is the dominant color

          case 0:
            R = V;
            G = tv;
            B = pv;
            break;

          // Green is the dominant color

          case 1:
            R = qv;
            G = V;
            B = pv;
            break;
          case 2:
            R = pv;
            G = V;
            B = tv;
            break;

          // Blue is the dominant color

          case 3:
            R = pv;
            G = qv;
            B = V;
            break;
          case 4:
            R = tv;
            G = pv;
            B = V;
            break;

          // Red is the dominant color

          case 5:
            R = V;
            G = pv;
            B = qv;
            break;

          // Just in case we overshoot on our math by a little, we put these here. Since its a switch it won't slow us down at all to put these here.

          case 6:
            R = V;
            G = tv;
            B = pv;
            break;
          case -1:
            R = V;
            G = pv;
            B = qv;
            break;

          // The color is not defined, we should throw an error.

          default:
            //LFATAL("i Value error in Pixel conversion, Value is %d", i);
            R = G = B = V; // Just pretend its black/white
            break;
        }
      }
      r = Clamp((int)(R * 255.0));
      g = Clamp((int)(G * 255.0));
      b = Clamp((int)(B * 255.0));
    }

    /// <summary>
    /// Clamp a value to 0-255
    /// </summary>
    static int Clamp (int i)
    {
      if (i < 0)
        return 0;
      if (i > 255)
        return 255;
      return i;
    }
  }
}
