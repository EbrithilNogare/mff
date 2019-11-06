using Raster;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using Utilities;

namespace Modules
{
    public class ModuleFullColor : DefaultRasterModule
    {
        /// <summary>
        /// Mandatory plain constructor.
        /// </summary>
        public ModuleFullColor()
        {
            // Default cell size (wid x hei).
            param = "wid=4096,hei=4096";
        }

        /// <summary>
        /// Author's full name.
        /// </summary>
        public override string Author => "David Napravnik";

        /// <summary>
        /// Name of the module (short enough to fit inside a list-boxes, etc.).
        /// </summary>
        public override string Name => "121";

        /// <summary>
        /// Tooltip for Param (text parameters).
        /// </summary>
        public override string Tooltip => "nothing to change";

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
        /// Output message (color check).
        /// </summary>
        protected string message;

        /// <summary>
        /// Assigns an input raster image to the given slot.
        /// Doesn't start computation (see #Update for this).
        /// </summary>
        /// <param name="inputImage">Input raster image (can be null).</param>
        /// <param name="slot">Slot number from 0 to InputSlots-1.</param>
        public override void SetInput(Bitmap inputImage, int slot = 0) { inImage = inputImage; }

        /// <summary>
        /// Recompute the output image[s] according to input image[s].
        /// Blocking (synchronous) function.
        /// #GetOutput() functions can be called after that.
        /// </summary>
        public override void Update()
        {
            // Input image is optional.
            // Starts a new computation.
            UserBreak = false;

            // Default values.
            int wid = 4096;
            int hei = 4096;
            bool check = true;
            outImage = new Bitmap(wid, hei, PixelFormat.Format24bppRgb);
            BitmapData dataOut = outImage.LockBits(new Rectangle(0, 0, wid, hei), ImageLockMode.WriteOnly, outImage.PixelFormat);

            unsafe
            {
                byte* iptr, optr, newOptr;
                byte ri, gi, bi;
                
                int dO = Image.GetPixelFormatSize(outImage.PixelFormat) / 8;  // pixel size in bytes

                //byte* baseIptr = (byte*)dataIn.Scan0;
                byte* baseOptr = (byte*)dataOut.Scan0;
                int blockSize = 16;
                for (int x = 0; x < wid / blockSize; x++) // fill with all colors
                {
                    if (UserBreak) break;
                    for (int y = 0; y < hei / blockSize; y++)
                    {
                        for (int xx = 0; xx < blockSize; xx++)
                        {
                            for (int yy = 0; yy < blockSize; yy++)
                            {
                                int picX = (x * blockSize + xx);
                                int picY = (y * blockSize + yy);
                                optr = baseOptr + (picX * wid + picY) * dO;
                                optr[0] = (byte)x;
                                optr[1] = (byte)y;
                                optr[2] = (byte)(xx*blockSize+yy);
                            }
                        }
                    }
                }
                if (inImage != null)
                {
                    PixelFormat iFormat;
                    int width = 0;
                    int height = 0;
                    BitmapData dataIn = null;
                    int dI;
                    // Convert pixel data (fast memory-mapped code).
                    iFormat = inImage.PixelFormat;
                    if (!PixelFormat.Format24bppRgb.Equals(iFormat) &&
                        !PixelFormat.Format32bppArgb.Equals(iFormat) &&
                        !PixelFormat.Format32bppPArgb.Equals(iFormat) &&
                        !PixelFormat.Format32bppRgb.Equals(iFormat))
                        iFormat = PixelFormat.Format24bppRgb;

                    width = inImage.Width;
                    height = inImage.Height;
                    dataIn = inImage.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadOnly, iFormat);
                    dI = Image.GetPixelFormatSize(iFormat) / 8;               // pixel size in bytes
                    byte* baseIptr = (byte*)dataIn.Scan0;


                    blockSize = 512;
                    for (int x = 0; x < wid; x++) // shuffle blocks of pixels
                    {
                        if (UserBreak) break;
                        for (int y = 0; y < hei; y ++)
                        {
                            optr = baseOptr + (x * wid + y) * dO;
                            iptr = baseIptr + ((x% height) * width + (y%width)) * dI;

                            optr[0] = (byte)(iptr[0] + optr[0]);
                            optr[1] = (byte)(iptr[1] + optr[1]);
                            optr[2] = (byte)(iptr[2] + optr[2]);
                        }
                    }










                    inImage.UnlockBits(dataIn);
                }
                else
                {
                    Random rand = new Random();
                    blockSize = 512;
                    for (int x = 0; x < wid; x += blockSize) // shuffle blocks of pixels
                    {
                        if (UserBreak) break;
                        for (int y = 0; y < hei; y += blockSize)
                        {
                            for (int xx = 0; xx < blockSize; xx++)
                            {
                                for (int yy = 0; yy < blockSize; yy++)
                                {
                                    int randX = rand.Next(0, blockSize);
                                    int randY = rand.Next(0, blockSize);
                                    byte[] temp = new byte[3];

                                    int picX = (x + xx);
                                    int picY = (y + yy);
                                    optr = baseOptr + (picX * wid + picY) * dO;

                                    int newPicX = (x + randX);
                                    int newPicY = (y + randY);
                                    newOptr = baseOptr + (newPicX * wid + newPicY) * dO;

                                    temp[0] = optr[0];
                                    temp[1] = optr[1];
                                    temp[2] = optr[2];

                                    optr[0] = newOptr[0];
                                    optr[1] = newOptr[1];
                                    optr[2] = newOptr[2];

                                    newOptr[0] = temp[0];
                                    newOptr[1] = temp[1];
                                    newOptr[2] = temp[2];
                                }
                            }
                        }
                    }
                }
            }

            outImage.UnlockBits(dataOut);            

            // Output message.
            if (check &&
                !UserBreak)
            {
                long colors = Draw.ColorNumber(outImage);
                message = colors == (1 << 24) ? "Colors: 16M, Ok" : $"Colors: {colors}, Fail";
            }
            else
                message = null;
        }
        
        /// <summary>
        /// Returns an output raster image.
        /// Can return null.
        /// </summary>
        /// <param name="slot">Slot number from 0 to OutputSlots-1.</param>
        public override Bitmap GetOutput(int slot = 0) => outImage;

        /// <summary>
        /// Returns an optional output message.
        /// Can return null.
        /// </summary>
        /// <param name="slot">Slot number from 0 to OutputSlots-1.</param>
        public override string GetOutputMessage(int slot = 0) => message;
    }
}
