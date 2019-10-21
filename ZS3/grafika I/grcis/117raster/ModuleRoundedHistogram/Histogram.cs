using System.Drawing;
using MathSupport;
using Raster;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Drawing.Imaging;

namespace Utilities
{
    public class ImageRoundedHistogram
    {
        /// <summary>
        /// Cached histogram data.
        /// </summary>
        protected static byte[,] histArray = new byte[256, 64];

        private static byte treshold = 2;

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
                    for (int saturation = 0; saturation <= (histArray.GetLength(1) - 1); saturation++)
                    {
                        if (histArray[hue, saturation]<treshold)
                            continue;

                        Color color = Arith.HSVToColor(hue * 360.0 / (histArray.GetLength(0) - 1), (double)saturation / (histArray.GetLength(1) - 1), 1.0);
                        color = Color.FromArgb(126, color);
                        //color = Color.FromArgb(255, 0, 0);
                        Pen graphPen = new Pen(color);
                        SolidBrush graphBrush = new SolidBrush(color);

                        double angle = Math.PI * 2 * hue / (histArray.GetLength(0) - 1);
                        double anglePart = Math.PI * 2 / (histArray.GetLength(0) - 1);
                        float distance = (float)saturation * (float)diameter / (histArray.GetLength(1) - 1);
                        float distancePart = (float)diameter / (histArray.GetLength(1) - 1);


                        float x1 = (float)Math.Sin(angle - anglePart) * (distance - distancePart) + centerX;
                        float y1 = (float)Math.Cos(angle - anglePart) * (distance - distancePart) + centerY;

                        float x2 = (float)Math.Sin(angle + anglePart) * (distance - distancePart) + centerX;
                        float y2 = (float)Math.Cos(angle + anglePart) * (distance - distancePart) + centerY;

                        float x3 = (float)Math.Sin(angle + anglePart) * (distance + distancePart) + centerX;
                        float y3 = (float)Math.Cos(angle + anglePart) * (distance + distancePart) + centerY;

                        float x4 = (float)Math.Sin(angle - anglePart) * (distance + distancePart) + centerX;
                        float y4 = (float)Math.Cos(angle - anglePart) * (distance + distancePart) + centerY;

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

            // faster bitmap access
            LockBitmap lockBitmap = new LockBitmap(input);
            lockBitmap.LockBits();

            // 1. Histogram recomputation.
            int size = int.TryParse(param, out size) ? size : 64;
            size = Math.Min(Math.Max(size, 2), 255);
            histArray = new byte[histArray.GetLength(0), size];

            int width = input.Width;
            int height = input.Height;

            for (y = 0; y < height; y++)
                for (x = 0; x < width; x++)
                {
                    Color color = lockBitmap.GetPixel(x, y);

                    byte hue = (byte)(color.GetHue() * (histArray.GetLength(0) - 1) / 360.0);
                    byte saturation = (byte)(color.GetSaturation() * (histArray.GetLength(1) - 1));

                    if(histArray[hue, saturation] < 255)
                        histArray[hue, saturation]++;
                }
            lockBitmap.UnlockBits();
        }
    }
    public class LockBitmap
    {
        Bitmap source = null;
        IntPtr Iptr = IntPtr.Zero;
        BitmapData bitmapData = null;

        public byte[] Pixels { get; set; }
        public int Depth { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }

        public LockBitmap(Bitmap source)
        {
            this.source = source;
        }

        /// <summary>
        /// Lock bitmap data
        /// </summary>
        public void LockBits()
        {
            try
            {
                // Get width and height of bitmap
                Width = source.Width;
                Height = source.Height;

                // get total locked pixels count
                int PixelCount = Width * Height;

                // Create rectangle to lock
                Rectangle rect = new Rectangle(0, 0, Width, Height);

                // get source bitmap pixel format size
                Depth = System.Drawing.Bitmap.GetPixelFormatSize(source.PixelFormat);

                // Check if bpp (Bits Per Pixel) is 8, 24, or 32
                if (Depth != 8 && Depth != 24 && Depth != 32)
                {
                    throw new ArgumentException("Only 8, 24 and 32 bpp images are supported.");
                }

                // Lock bitmap and return bitmap data
                bitmapData = source.LockBits(rect, ImageLockMode.ReadWrite,
                                             source.PixelFormat);

                // create byte array to copy pixel values
                int step = Depth / 8;
                Pixels = new byte[PixelCount * step];
                Iptr = bitmapData.Scan0;

                // Copy data from pointer to array
                Marshal.Copy(Iptr, Pixels, 0, Pixels.Length);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Unlock bitmap data
        /// </summary>
        public void UnlockBits()
        {
            try
            {
                // Copy data from byte array to pointer
                Marshal.Copy(Pixels, 0, Iptr, Pixels.Length);

                // Unlock bitmap data
                source.UnlockBits(bitmapData);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Get the color of the specified pixel
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public Color GetPixel(int x, int y)
        {
            Color clr = Color.Empty;

            // Get color components count
            int cCount = Depth / 8;

            // Get start index of the specified pixel
            int i = ((y * Width) + x) * cCount;

            if (i > Pixels.Length - cCount)
                throw new IndexOutOfRangeException();

            if (Depth == 32) // For 32 bpp get Red, Green, Blue and Alpha
            {
                byte b = Pixels[i];
                byte g = Pixels[i + 1];
                byte r = Pixels[i + 2];
                byte a = Pixels[i + 3]; // a
                clr = Color.FromArgb(a, r, g, b);
            }
            if (Depth == 24) // For 24 bpp get Red, Green and Blue
            {
                byte b = Pixels[i];
                byte g = Pixels[i + 1];
                byte r = Pixels[i + 2];
                clr = Color.FromArgb(r, g, b);
            }
            if (Depth == 8)
            // For 8 bpp get color value (Red, Green and Blue values are the same)
            {
                byte c = Pixels[i];
                clr = Color.FromArgb(c, c, c);
            }
            return clr;
        }

        /// <summary>
        /// Set the color of the specified pixel
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="color"></param>
        public void SetPixel(int x, int y, Color color)
        {
            // Get color components count
            int cCount = Depth / 8;

            // Get start index of the specified pixel
            int i = ((y * Width) + x) * cCount;

            if (Depth == 32) // For 32 bpp set Red, Green, Blue and Alpha
            {
                Pixels[i] = color.B;
                Pixels[i + 1] = color.G;
                Pixels[i + 2] = color.R;
                Pixels[i + 3] = color.A;
            }
            if (Depth == 24) // For 24 bpp set Red, Green and Blue
            {
                Pixels[i] = color.B;
                Pixels[i + 1] = color.G;
                Pixels[i + 2] = color.R;
            }
            if (Depth == 8)
            // For 8 bpp set color value (Red, Green and Blue values are the same)
            {
                Pixels[i] = color.B;
            }
        }
    }

}
