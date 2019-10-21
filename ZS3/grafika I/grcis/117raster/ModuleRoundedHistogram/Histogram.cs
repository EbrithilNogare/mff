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
        public static void DrawHistogram(
          Bitmap graph)
        {
            if (histArray == null)
                return;

            using (Graphics gfx = Graphics.FromImage(graph))
            {
                gfx.Clear(Color.White);

                // Graph scaling:
                int centerX = graph.Width / 2;
                int centerY = graph.Height / 2;
                int diameter = Math.Min(graph.Height / 2, graph.Width / 2);

                // Histogram:
                for (int hue = 0; hue <= (histArray.GetLength(0) - 1); hue++)
                {
                    for (int saturation = 0; saturation <= (histArray.GetLength(1) - 1); saturation++)
                    {

                        if (!histArray[hue, saturation])
                            continue;

                        Color color = Arith.HSVToColor(hue * 360.0 / (histArray.GetLength(0) - 1), (double)saturation / (histArray.GetLength(1) - 1), 1.0);
                        Pen graphPen = new Pen(color);
                        SolidBrush graphBrush = new SolidBrush(color);

                        double angle = Math.PI * 2 * hue / (histArray.GetLength(0) - 1);

                        float x1 = (float)Math.Sin(angle) * saturation / (histArray.GetLength(1) - 1) * diameter + centerX;
                        float y1 = (float)Math.Cos(angle) * saturation / (histArray.GetLength(1) - 1) * diameter + centerY;

                        float x2 = (float)Math.Sin(angle + Math.PI * 2 / (histArray.GetLength(0) - 1)) * saturation / (histArray.GetLength(1) - 1) * diameter + centerX;
                        float y2 = (float)Math.Cos(angle + Math.PI * 2 / (histArray.GetLength(0) - 1)) * saturation / (histArray.GetLength(1) - 1) * diameter + centerY;

                        float x3 = (float)Math.Sin(angle + Math.PI * 2 / (histArray.GetLength(0) - 1)) * (saturation + 1) / (histArray.GetLength(1) - 1) * diameter + centerX;
                        float y3 = (float)Math.Cos(angle + Math.PI * 2 / (histArray.GetLength(0) - 1)) * (saturation + 1) / (histArray.GetLength(1) - 1) * diameter + centerY;

                        float x4 = (float)Math.Sin(angle) * (saturation + 1) / (histArray.GetLength(1) - 1) * diameter + centerX;
                        float y4 = (float)Math.Cos(angle) * (saturation + 1) / (histArray.GetLength(1) - 1) * diameter + centerY;

                        PointF[] curvePoints = {
                            new PointF(x1, y1),
                            new PointF(x2, y2),
                            new PointF(x3, y3),
                            new PointF(x4, y4),
                        };

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
        public static void ComputeHistogram(
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
            size = Math.Min(Math.Max(size, 2), 255);
            histArray = new bool[histArray.GetLength(0), size];

            int width = input.Width;
            int height = input.Height;

            for (y = 0; y < height; y++)
                for (x = 0; x < width; x++)
                {
                    Color color = input.GetPixel(x, y);

                    byte hue = (byte)(color.GetHue() * (histArray.GetLength(0) - 1) / 360.0);
                    byte saturation = (byte)(color.GetSaturation() * (histArray.GetLength(1) - 1));

                    histArray[hue, saturation] = true;
                }
        }
    }
}
