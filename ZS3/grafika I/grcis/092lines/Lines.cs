using System;
using System.Collections.Generic;
using System.Drawing;
using LineCanvas;
using Utilities;

namespace _092lines
{
    public class Lines
    {
        /// <summary>
        /// Form data initialization.
        /// </summary>
        /// <param name="name">Your first-name and last-name.</param>
        /// <param name="wid">Initial image width in pixels.</param>
        /// <param name="hei">Initial image height in pixels.</param>
        /// <param name="param">Optional text to initialize the form's text-field.</param>
        /// <param name="tooltip">Optional tooltip = param help.</param>
        public static void InitParams(out string name, out int wid, out int hei, out string param, out string tooltip)
        {
            // Put your name here.
            name = "David Napravnik";

            // Image size in pixels.
            wid = 500;
            hei = 500;

            // Specific animation params.
            param = "width=1.0,anti=true";

            // Tooltip = help.
            tooltip = "width=<int>, anti[=<bool>]";
        }

        /// <summary>
        /// Draw the image into the initialized Canvas object.
        /// </summary>
        /// <param name="c">Canvas ready for your drawing.</param>
        /// <param name="param">Optional string parameter from the form.</param>
        public static void Draw(Canvas c, string param)
        {
            Dictionary<string, string> p = Util.ParseKeyValueList(param);
            if (p.Count > 0)
            {
                float penWidth = 1;
                if (Util.TryParse(p, "width", ref penWidth))
                {
                    c.SetPenWidth(penWidth);
                }

                bool antialias = true;
                // anti[=<bool>]
                if (Util.TryParse(p, "anti", ref antialias))
                {
                    c.SetAntiAlias(antialias);
                }
            }

            int wh, hh;
            wh = hh = Math.Min(c.Width, c.Height) / 2;
            Random rand = new Random();

            c.Clear(Color.Black);
            c.SetColor(Color.White);

            // 1st quadrant - spiral.
            DrawSpiral(c, rand.NextDouble() / 50, 0, 0, wh, hh);

            // 2nd quadrant - snowflake.
            DrawSnowflake(c, rand.Next(0, 5), wh, 0, wh * 2, hh);

            // 3rd quadrant - pentagram.
            DrawPentagram(c, rand.Next(2, 10) * 2 + 1, 0, hh, wh, hh * 2);

            // 4th quadrant - .
            DrawMaze(c, rand.Next(5, 20), wh, hh, wh * 2, hh * 2);
        }

        private static void DrawSpiral(Canvas c, double randomConstant, int beginX, int beginY, int endX, int endY)
        {
            c.SetColor(Color.White);
            double x = (beginX + endX) / 2;
            double y = (beginY + endY) / 2;
            double angle = 0;
            int iterations = 100;

            for (int i = 0; i < iterations; i++)
            {
                angle += Math.PI / 2 + randomConstant;
                double newX = x + Math.Sin(angle) * i / iterations * (beginX + endX) / 2;
                double newY = y + Math.Cos(angle) * i / iterations * (beginY + endY) / 2;
                c.SetColor(Color.FromArgb(i*255/iterations, 255-i * 255 / iterations, 0));
                c.Line(x, y, newX, newY);
                x = newX;
                y = newY;
            }
            c.SetColor(Color.White);
        }
        private static void DrawSnowflake(Canvas c, int iterations, int beginX, int beginY, int endX, int endY)
        {
            int width = endX - beginX;
            int height = endY - beginY;
            double topY = beginY + height / 3;
            double botY = beginY + height / 3 + (Math.Sqrt(3.0 / 4) / 2 * height);
            double leftX = beginX + width / 4;
            double midX = beginX + width / 2;
            double rightX = beginX + width / 4 * 3;

            KochEdge(c, leftX, topY, rightX, topY, iterations);
            KochEdge(c, rightX, topY, midX, botY, iterations);
            KochEdge(c, midX, botY, leftX, topY, iterations);
        }
        private static void KochEdge(Canvas c, double beginX, double beginY, double endX, double endY, int iteration)
        {
            double x1 = (endX - beginX) / 3 + beginX;
            double y1 = (endY - beginY) / 3 + beginY;
            double x2 = (endX - beginX) * 2 / 3 + beginX;
            double y2 = (endY - beginY) * 2 / 3 + beginY;
            double x3 = ((x1 + x2) + Math.Sqrt(3) * (-y1 + y2)) / 2;
            double y3 = ((y1 + y2) + Math.Sqrt(3) * (x1 - x2)) / 2;

            if (iteration > 0)
            {
                KochEdge(c, beginX, beginY, x1, y1, iteration - 1);
                KochEdge(c, x1, y1, x3, y3, iteration - 1);
                KochEdge(c, x3, y3, x2, y2, iteration - 1);
                KochEdge(c, x2, y2, endX, endY, iteration - 1);
            }
            else
            {
                c.Line(beginX, beginY, x1, y1);
                c.Line(x2, y2, endX, endY);
                c.Line(x1, y1, x3, y3);
                c.Line(x2, y2, x3, y3);
            }
        }
        private static void DrawPentagram(Canvas c, int randomConstant, int beginX, int beginY, int endX, int endY)
        {
            c.SetColor(Color.White);
            double x = (beginX + endX) / 2;
            double y = (beginY + endY) / 2 + (beginX + endX) / 4;
            double angle = 0;
            int iterations = randomConstant;
            double angleDiff = Math.PI - Math.PI / iterations;

            for (int i = 0; i < iterations; i++)
            {
                angle += angleDiff;
                double newX = x + Math.Sin(angle) * (beginX + endX) / 2;
                double newY = y + Math.Cos(angle) * (beginX + endX) / 2;
                c.SetColor(Color.FromArgb(0,i * 255 / iterations, 255 - i * 255 / iterations));
                c.Line(x, y, newX, newY);
                x = newX;
                y = newY;
            }
            c.SetColor(Color.White);
        }
        private static void DrawMaze(Canvas c, int size, int beginX, int beginY, int endX, int endY)
        {
            int width = size;
            int height = size;
            bool[,] map = new bool[width, height];
            Random rand = new Random();

            // borders outside
            c.SetColor(Color.DarkGreen);
            for (int i = 0; i < width; i++)
            {
                c.Line(
                    beginX + (endX - beginX) / (width + 1f),
                    beginY + (endY - beginY) / (height + 1f),
                    endX - (endX - beginX) / (width + 1f),
                    beginY + (endY - beginY) / (height + 1f));
                map[i, 0] = true;
                c.Line(
                    beginX + (endX - beginX) / (width + 1f),
                    endY - (endY - beginY) / (height + 1f),
                    endX - (endX - beginX) / (width + 1f),
                    endY - (endY - beginY) / (height + 1f));
                map[i, height - 1] = true;
            }
            for (int i = 0; i < height; i++)
            {
                c.Line(
                    beginX + (endX - beginX) / (width + 1f),
                    beginY + (endY - beginY) / (height + 1f),
                    beginX + (endX - beginX) / (width + 1f),
                    endY - (endY - beginY) / (height + 1f));
                map[0, i] = true;
                c.Line(
                    endX - (endX - beginX) / (width + 1f),
                    beginY + (endY - beginY) / (height + 1f),
                    endX - (endX - beginX) / (width + 1f),
                    endY - (endY - beginY) / (height + 1f));
                map[width - 1, i] = true;
            }
            // borders inside
            c.SetColor(Color.White);
            for (int i = 0; i < (width - 2) * (height - 2);)
            {
                int randomX, randomY;
                do
                {
                    randomX = rand.Next(1, width - 1);
                    randomY = rand.Next(1, height - 1);
                } while (map[randomX, randomY]);
                MakeMazePath(c, ref i, beginX, beginY, endX, endY, width, height, map, randomX, randomY, rand.Next(0, 4));
            }
        }
        private static void MakeMazePath(Canvas c, ref int i, int beginX, int beginY, int endX, int endY, int width, int height, bool[,] map, int x, int y, int direction)
        {
            if (map[x, y]) return;

            map[x, y] = true;
            i++;

            float segmentWidth = (endX - beginX) / (width + 1f);
            float segmentHeight = (endY - beginY) / (height + 1f);
            int newX = x;
            int newY = y;

            switch (direction)
            {
                case 0: newX++; break;
                case 1: newY++; break;
                case 2: newX--; break;
                case 3: newY--; break;
            }
            c.Line(beginX + (x + 1) * segmentWidth, beginY + (y + 1) * segmentHeight, beginX + (newX + 1) * segmentWidth, beginY + (newY + 1) * segmentHeight);
            MakeMazePath(c, ref i, beginX, beginY, endX, endY, width, height, map, newX, newY, direction);

        }
    }
}
