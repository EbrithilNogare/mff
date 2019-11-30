using System;
using System.Collections.Generic;
using System.Drawing;
using LineCanvas;
using MathSupport;
using Utilities;

namespace _093animation
{
    public class Animation
    {
        /// <summary>
        /// Form data initialization.
        /// </summary>
        /// <param name="name">Your first-name and last-name.</param>
        /// <param name="wid">Image width in pixels.</param>
        /// <param name="hei">Image height in pixels.</param>
        /// <param name="from">Animation start in seconds.</param>
        /// <param name="to">Animation end in seconds.</param>
        /// <param name="fps">Frames-per-seconds.</param>
        /// <param name="param">Optional text to initialize the form's text-field.</param>
        /// <param name="tooltip">Optional tooltip = param help.</param>
        public static void InitParams(out string name, out int wid, out int hei, out double from, out double to, out double fps, out string param, out string tooltip)
        {
            // Put your name here.
            name = "David Napravnik";

            // Image size in pixels.
            wid = 1920;
            hei = 1080;

            // Animation.
            from = 0.0;
            to = 10.0;
            fps = 30.0;

            // Specific animation params.
            param = "width=5.0,anti=true,rays=100";

            // Tooltip = help.
            tooltip = "width=<double>, anti[=<bool>], rays=<int>";
        }

        /// <summary>
        /// Global initialization. Called before each animation batch
        /// or single-frame computation.
        /// </summary>
        /// <param name="width">Width of the future canvas in pixels.</param>
        /// <param name="height">Height of the future canvas in pixels.</param>
        /// <param name="start">Start time (t0)</param>
        /// <param name="end">End time (for animation length normalization).</param>
        /// <param name="fps">Required fps.</param>
        /// <param name="param">Optional string parameter from the form.</param>
        public static void InitAnimation(int width, int height, double start, double end, double fps, string param)
        {

        }

        /// <summary>
        /// Draw single animation frame.
        /// Has to be re-entrant!
        /// </summary>
        /// <param name="c">Canvas to draw to.</param>
        /// <param name="time">Current time in seconds.</param>
        /// <param name="start">Start time (t0)</param>
        /// <param name="end">End time (for animation length normalization).</param>
        /// <param name="param">Optional string parameter from the form.</param>
        public static void DrawFrame(Canvas c, double time, double start, double end, string param)
        {
            double timeNorm = Arith.Clamp((time - start) / (end - start), 0.0, 1.0);

            // input params:
            float penWidth = 1.0f;   // pen width
            bool antialias = false;  // use anti-aliasing?
            int rays = 100;    // number of rays from light

            Dictionary<string, string> p = Util.ParseKeyValueList(param);
            if (p.Count > 0)
            {
                // with=<line-width>
                if (Util.TryParse(p, "width", ref penWidth) && penWidth < 0.0f)
                   penWidth = 0.0f;

                // anti[=<bool>]
                Util.TryParse(p, "anti", ref antialias);

                // rays=<number>
                if (Util.TryParse(p, "rays", ref rays) && rays < 0)
                    rays = 0;
            }

            int wh = c.Width / 2;
            int hh = c.Height / 2;
            int minh = Math.Min(wh, hh);

            c.Clear(Color.Black);
            c.SetColor(Color.White);
            c.SetPenWidth(penWidth);
            c.SetAntiAlias(antialias);



            List<PointF[]> map = new List<PointF[]>();

            // create map

            // borders
            map.Add(new PointF[] { new Point(10, 10), new Point(wh*2 - 10, 10) });
            map.Add(new PointF[] { new Point(wh * 2 - 10, 10), new Point(wh * 2 - 10, hh * 2 - 10) });
            map.Add(new PointF[] { new Point(wh * 2 - 10, hh * 2 - 10), new Point(10, hh * 2 - 10) });
            map.Add(new PointF[] { new Point(10, hh * 2 - 10), new Point(10, 10) });

            // some objects
            map.Add(new PointF[] { new Point(500, 100), new Point(500, 900) });
            map.Add(new PointF[] { new Point(1400, 500), new Point(1100, 900) });
            map.Add(new PointF[] { new Point(600, 570), new Point(600, 630) });
            map.Add(new PointF[] { new Point(820, 384), new Point(1032, 378) });
            map.Add(new PointF[] { new Point(1058, 292), new Point(1095, 435) });
            map.Add(new PointF[] { new Point(1212, 524), new Point(1290, 800) });



            // draw it all
            foreach (PointF[] wall in map)
            {
                c.PolyLine(wall);
            }




            // computer light
            c.SetPenWidth(1);
            c.SetColor(Color.Yellow);
            float lightX = wh + (float)Math.Sin(time) * 300;
            float lightY = hh + (float)Math.Cos(time) * 300;

            for (double angle = 0; angle < Math.PI*2; angle+=Math.PI*2/rays)
            {
                double minDistance = 10000;
                foreach (PointF[] wall in map)
                {
                    PointF intersection = getIntersection(lightX, lightY, lightX +(float)Math.Sin(angle), lightY + (float)Math.Cos(angle), wall[0].X, wall[0].Y, wall[1].X, wall[1].Y);
                    if(intersection != Point.Empty)
                    {
                        float distance = (float)Math.Sqrt(Math.Pow(lightX - intersection.X, 2) + Math.Pow(lightY - intersection.Y,2));
                        if (distance < minDistance)
                            minDistance = distance;
                    }
                }
                c.Line(lightX, lightY, lightX + (float)Math.Sin(angle) * minDistance, lightY + (float)Math.Cos(angle) * minDistance);
            }


            // show where light is (sometimes it is not obvious)
            c.SetColor(Color.Red);
            c.SetPenWidth(5);
            c.Line(lightX + 5, lightY + 5, lightX - 5, lightY - 5);
            c.Line(lightX - 5, lightY + 5, lightX + 5, lightY - 5);

        }
        static PointF getIntersection(float x1, float y1, float x2, float y2, float x3, float y3, float x4, float y4)
        {
            float den = (x1 - x2) * (y3 - y4) - (y1 - y2) * (x3 - x4);
            if (den == 0)
            {
                return PointF.Empty;
            }

            float t = ((x1 - x3) * (y3 - y4) - (y1 - y3) * (x3 - x4)) / den;
            float u = -((x1 - x2) * (y1 - y3) - (y1 - y2) * (x1 - x3)) / den;
            if (t > 0 && u > 0 && u < 1)
            {
                PointF pt = new PointF();
                pt.X = x1 + t * (x2 - x1);
                pt.Y = y1 + t * (y2 - y1);
                return pt;
            }
            else
                return PointF.Empty;
        }
    }
}
