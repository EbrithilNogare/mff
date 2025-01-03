﻿using MathSupport;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace Modules
{
    public class ModuleHSV : DefaultRasterModule
    {
        /// <summary>
        /// Mandatory plain constructor.
        /// </summary>
        public ModuleHSV()
        { }

        /// <summary>
        /// Author's full name.
        /// </summary>
        public override string Author => "David Napranvik";

        /// <summary>
        /// Name of the module (short enough to fit inside a list-boxes, etc.).
        /// </summary>
        public override string Name => "120";

        /// <summary>
        /// Tooltip for Param (text parameters).
        /// </summary>
        public override string Tooltip => "[dH=<double>][,mulS=<double>][,mulV=<double>][,gamma=<double>][,slow][,par]\n... dH is absolute, mS, mV, dGamma relative";

        /// <summary>
        /// Default HSV transform parameters (have to by in sync with the default module state).
        /// </summary>
        protected string param = "mulS=1.4,par";

        /// <summary>
        /// True if 'param' has to be parsed in the next recompute() call.
        /// </summary>
        protected bool paramDirty = true;

        /// <summary>
        /// Current 'Param' string is stored in the module.
        /// Set reasonable initial value.
        /// </summary>
        public override string Param
        {
            get => param;
            set
            {
                if (value != param)
                {
                    param = value;
                    paramDirty = true;
                    recompute();
                }
            }
        }

        /// <summary>
        /// Usually read-only, optionally writable (client is defining number of inputs).
        /// </summary>
        public override int InputSlots => 1;

        /// <summary>
        /// Usually read-only, optionally writable (client is defining number of outputs).
        /// </summary>
        public override int OutputSlots => 1;

        /// <summary>
        /// Input raster image.
        /// </summary>
        protected Bitmap inImage = null;

        /// <summary>
        /// Output raster image.
        /// </summary>
        protected Bitmap outImage = null;

        /// <summary>
        /// Absolute Hue delta in degrees.
        /// </summary>
        protected double dH = 0.0;

        /// <summary>
        /// Saturation multiplier.
        /// </summary>
        protected double mS = 1.4;

        /// <summary>
        /// Value multiplier.
        /// </summary>
        protected double mV = 1.0;

        /// <summary>
        /// Gamma-correction coefficient (visible value = inverse value).
        /// </summary>
        protected double gamma = 1.0;

        /// <summary>
        /// Slow computation (using GetPixel/SetPixel).
        /// </summary>
        protected bool slow = false;

        /// <summary>
        /// Parallel computation (most useful for large pictures).
        /// </summary>
        protected bool parallel = true;

        protected double minH = -30.0;
        protected double maxH = 40.0;
                              
        protected double minS = 0.0;
        protected double maxS = 1.0;
                              
        protected double minV = 0.0;
        protected double maxV = 1.0;

        /// <summary>
        /// Active HSV form.
        /// </summary>
        protected FormHSV hsvForm = null;

        protected void updateParam()
        {
            if (paramDirty)
            {
                paramDirty = false;

                // 'param' parsing.
                Dictionary<string, string> p = Util.ParseKeyValueList(param);
                if (p.Count > 0)
                {
                    // dH=<double> [+- number in degrees]
                    Util.TryParse(p, "dH", ref dH);

                    // mulS=<double> [relative number .. multiplicator]
                    Util.TryParse(p, "mulS", ref mS);

                    // mulV=<double> [relative number .. multiplicator]
                    Util.TryParse(p, "mulV", ref mV);

                    // gamma=<double> [gamma correction .. exponent]
                    if (Util.TryParse(p, "gamma", ref gamma))
                    {
                        // <= 0.0 || 1.0.. nothing
                        if (gamma < 0.001)
                            gamma = 1.0;
                    }

                    // par .. use Parallel.For
                    parallel = p.ContainsKey("par");

                    // slow .. set GetPixel/SetPixel computation
                    slow = p.ContainsKey("slow");


                    Util.TryParse(p, "minH", ref minH);
                    Util.TryParse(p, "maxH", ref maxH);
                                                     
                    Util.TryParse(p, "minS", ref minS);
                    Util.TryParse(p, "maxS", ref maxS);
                                                     
                    Util.TryParse(p, "minV", ref minV);
                    Util.TryParse(p, "maxV", ref maxV);
                }
            }

            formUpdate();
        }

        /// <summary>
        /// Recompute the image.
        /// </summary>
        protected void recompute()
        {
            if (inImage == null)
                return;

            // Update module values from 'param' string.
            updateParam();

            int wid = inImage.Width;
            int hei = inImage.Height;

            // Output image must be true-color.
            outImage = new Bitmap(wid, hei, PixelFormat.Format24bppRgb);

            // Convert pixel data.
            double gam = (gamma < 0.001) ? 1.0 : 1.0 / gamma;

            

            if (slow)
            {
                // Slow GetPixel/SetPixel code.

                for (int y = 0; y < hei; y++)
                {
                    // !!! TODO: Interrupt handling.
                    double R, G, B;
                    double H, S, V;

                    for (int x = 0; x < wid; x++)
                    {
                        Color ic = inImage.GetPixel(x, y);

                        // Conversion to HSV.
                        Arith.ColorToHSV(ic, out H, out S, out V);
                        // 0 <= H <= 360, 0 <= S <= 1, 0 <= V <= 1

                        // Conversion back to RGB.
                        pixelFiltration(out R, out G, out B, ref H, ref S, ref V, ic);
                        // [R,G,B] is from [0.0, 1.0]^3

                        // Optional gamma correction.
                        if (gam != 1.0)
                        {
                            // Gamma-correction.
                            R = Math.Pow(R, gam);
                            G = Math.Pow(G, gam);
                            B = Math.Pow(B, gam);
                        }

                        Color oc = Color.FromArgb(
                        Convert.ToInt32(Util.Clamp(R * 255.0, 0.0, 255.0)),
                        Convert.ToInt32(Util.Clamp(G * 255.0, 0.0, 255.0)),
                        Convert.ToInt32(Util.Clamp(B * 255.0, 0.0, 255.0)));

                        outImage.SetPixel(x, y, oc);
                    }
                }
            }
            else
            {
                // Fast memory-mapped code.
                PixelFormat iFormat = inImage.PixelFormat;
                if (!PixelFormat.Format24bppRgb.Equals(iFormat) &&
                    !PixelFormat.Format32bppArgb.Equals(iFormat) &&
                    !PixelFormat.Format32bppPArgb.Equals(iFormat) &&
                    !PixelFormat.Format32bppRgb.Equals(iFormat))
                    iFormat = PixelFormat.Format24bppRgb;

                BitmapData dataIn = inImage.LockBits(new Rectangle(0, 0, wid, hei), ImageLockMode.ReadOnly, iFormat);
                BitmapData dataOut = outImage.LockBits(new Rectangle(0, 0, wid, hei), ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);
                unsafe
                {
                    int dI = Image.GetPixelFormatSize(iFormat) / 8;
                    int dO = Image.GetPixelFormatSize(PixelFormat.Format24bppRgb) / 8;

                    Action<int> inner = y =>
                    {
                        // !!! TODO: Interrupt handling.
                        double R, G, B;
                        double H, S, V;

                        byte* iptr = (byte*)dataIn.Scan0 + y * dataIn.Stride;
                        byte* optr = (byte*)dataOut.Scan0 + y * dataOut.Stride;

                        for (int x = 0; x < wid; x++, iptr += dI, optr += dO)
                        {
                            // Recompute one pixel (*iptr -> *optr).
                            // iptr, optr -> [B,G,R]

                            // Conversion to HSV.
                            Arith.RGBtoHSV(iptr[2] / 255.0, iptr[1] / 255.0, iptr[0] / 255.0, out H, out S, out V);
                            // 0 <= H <= 360, 0 <= S <= 1, 0 <= V <= 1


                            // Conversion back to RGB.
                            pixelFiltration(out R, out G, out B, ref H, ref S, ref V, Color.FromArgb(iptr[2], iptr[1], iptr[0]));
                            // [R,G,B] is from [0.0, 1.0]^3

                            // Optional gamma correction.
                            if (gam != 1.0)
                            {
                                // Gamma-correction.
                                R = Math.Pow(R, gam);
                                G = Math.Pow(G, gam);
                                B = Math.Pow(B, gam);
                            }

                            optr[0] = Convert.ToByte(Util.Clamp(B * 255.0, 0.0, 255.0));
                            optr[1] = Convert.ToByte(Util.Clamp(G * 255.0, 0.0, 255.0));
                            optr[2] = Convert.ToByte(Util.Clamp(R * 255.0, 0.0, 255.0));
                        }
                    };

                    if (parallel)
                        Parallel.For(0, hei, inner);
                    else
                        for (int y = 0; y < hei; y++)
                            inner(y);
                }

                outImage.UnlockBits(dataOut);
                inImage.UnlockBits(dataIn);
            }
        }

        private void pixelFiltration(out double R, out double G, out double B, ref double H, ref double S, ref double V, Color ic)
        {
            if (
                (
                    (H >= minH + 360 && H <= maxH + 360) ||
                    (H >= minH + 360 && H <= maxH) ||
                    (H >= minH && H <= maxH)
                ) &&
                (S >= minS && S <= maxS) &&
                (V >= minV && V <= maxV)
            )
            {
                /**///switch
                R = ic.R / 255.0;
                G = ic.G / 255.0;
                B = ic.B / 255.0;
                /*/
                R = 255;
                G = 0;
                B = 0;
                /**/
            }
            else
            {
                // HSV transform.
                H = H + dH;
                S = Util.Clamp(S * mS, 0.0, 1.0);
                V = Util.Clamp(V * mV, 0.0, 1.0);

                Arith.HSVToRGB(H, S, V, out R, out G, out B);
            }
        }

        /// <summary>
        /// Send ModuleHSV values to the form elements.
        /// </summary>
        protected void formUpdate()
        {
            if (hsvForm == null)
                return;

            hsvForm.numericHue.Value = Convert.ToDecimal(dH);
            hsvForm.textSaturation.Text = string.Format(CultureInfo.InvariantCulture, "{0:g5}", mS);
            hsvForm.textValue.Text = string.Format(CultureInfo.InvariantCulture, "{0:g5}", mV);
            hsvForm.textGamma.Text = string.Format(CultureInfo.InvariantCulture, "{0:g5}", gamma);
            hsvForm.checkParallel.Checked = parallel;
            hsvForm.checkSlow.Checked = slow;

            hsvForm.Invalidate();
        }

        /// <summary>
        /// Notification: GUI window changed its values (sync GUI -> module is needed).
        /// </summary>
        public override void OnGuiWindowChanged()
        {
            if (hsvForm == null)
                return;

            dH = Convert.ToDouble(hsvForm.numericHue.Value);
            if (!double.TryParse(hsvForm.textSaturation.Text, NumberStyles.Number, CultureInfo.InvariantCulture, out mS))
                mS = 1.0;
            if (!double.TryParse(hsvForm.textValue.Text, NumberStyles.Number, CultureInfo.InvariantCulture, out mV))
                mV = 1.0;
            if (!double.TryParse(hsvForm.textGamma.Text, NumberStyles.Number, CultureInfo.InvariantCulture, out gamma))
                gamma = 1.0;
            parallel = hsvForm.checkParallel.Checked;
            slow = hsvForm.checkSlow.Checked;


            minH = Convert.ToDouble(hsvForm.minHue.Value);
            maxH = Convert.ToDouble(hsvForm.maxHue.Value);

            minS = Convert.ToDouble(hsvForm.minSaturation.Value);
            maxS = Convert.ToDouble(hsvForm.maxSaturation.Value);

            minV = Convert.ToDouble(hsvForm.minValue.Value);
            maxV = Convert.ToDouble(hsvForm.maxValue.Value);

            // Update 'param'.
            List<string> pars = new List<string>();

            if (dH != 0.0)
                pars.Add(string.Format(CultureInfo.InvariantCulture, "dH={0:g}", dH));
            if (mS != 1.0)
                pars.Add(string.Format(CultureInfo.InvariantCulture, "mulS={0:g5}", mS));
            if (mV != 1.0)
                pars.Add(string.Format(CultureInfo.InvariantCulture, "mulV={0:g5}", mV));
            if (gamma != 1.0)
                pars.Add(string.Format(CultureInfo.InvariantCulture, "gamma={0:g5}", gamma));
            if (parallel)
                pars.Add("par");
            if (slow)
                pars.Add("slow");


            pars.Add(string.Format(CultureInfo.InvariantCulture, "minH={0:g}", minH));
            pars.Add(string.Format(CultureInfo.InvariantCulture, "maxH={0:g}", maxH));

            pars.Add(string.Format(CultureInfo.InvariantCulture, "minS={0:g}", minS));
            pars.Add(string.Format(CultureInfo.InvariantCulture, "maxS={0:g}", maxS));

            pars.Add(string.Format(CultureInfo.InvariantCulture, "minV={0:g}", minV));
            pars.Add(string.Format(CultureInfo.InvariantCulture, "maxV={0:g}", maxV));

            param = string.Join(",", pars);
            paramDirty = false;

            ParamUpdated?.Invoke(this);
        }

        /// <summary>
        /// Notification: GUI window has been closed.
        /// </summary>
        public override void OnGuiWindowClose()
        {
            hsvForm?.Hide();
            hsvForm = null;
        }

        /// <summary>
        /// Assigns an input raster image to the given slot.
        /// Doesn't start computation (see #Update for this).
        /// </summary>
        /// <param name="inputImage">Input raster image (can be null).</param>
        /// <param name="slot">Slot number from 0 to InputSlots-1.</param>
        public override void SetInput(
          Bitmap inputImage,
          int slot = 0)
        {
            inImage = inputImage;
        }

        /// <summary>
        /// Recompute the output image[s] according to input image[s].
        /// Blocking (synchronous) function.
        /// #GetOutput() functions can be called after that.
        /// </summary>
        public override void Update() => recompute();

        /// <summary>
        /// Returns an output raster image.
        /// Can return null.
        /// </summary>
        /// <param name="slot">Slot number from 0 to OutputSlots-1.</param>
        public override Bitmap GetOutput(
          int slot = 0) => outImage;

        /// <summary>
        /// Returns true if there is an active GUI window associted with this module.
        /// You can open/close GUI window using the setter.
        /// </summary>
        public override bool GuiWindow
        {
            get => hsvForm != null;
            set
            {
                if (value)
                {
                    // Show GUI window.
                    if (hsvForm == null)
                    {
                        hsvForm = new FormHSV(this);
                        formUpdate();
                        hsvForm.Show();
                    }
                }
                else
                {
                    hsvForm?.Hide();
                    hsvForm = null;
                }
            }
        }
    }
}
