using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Requester
{
    public class Beautifier
    {
        public string JSON(string input, Dictionary<string, Color> pallete)
        {
            string output;
            int indent;
            const int indentSize = 2;
            for (int i = 0; i < input.Length; i++)
            {
                char v = input[i];



            }


            throw new NotImplementedException();
        }
    }

    public struct Color
    {
        public byte r { get; set; }
        public byte g { get; set; }
        public byte b { get; set; }

        public Color(string input)
        {
            r = g = b = 0;
            if (input[0] == '#')
            {
                switch (input.Length)
                {
                    case 4:
                        r = HexToByte(input[1], input[1]);
                        g = HexToByte(input[2], input[2]);
                        b = HexToByte(input[3], input[3]);
                        break;
                    case 7:
                        r = HexToByte(input[1], input[2]);
                        g = HexToByte(input[3], input[4]);
                        b = HexToByte(input[5], input[6]);
                        break;
                    default:
                        throw new ArgumentException("input hasnt correct size");
                }
            }else if(input.Substring(0, 4) == "rgb(" && input[input.Length-1]==')')
                
            {
                input.Replace(" ", String.Empty);
                input = input.Substring(4, input.Length - 5);
                string[] colors = input.Split(',');
                r = Convert.ToByte(colors[0]);
                g = Convert.ToByte(colors[1]);
                b = Convert.ToByte(colors[2]);
            }
            else
            {
                throw new ArgumentException("input hasnt correct size");
            }
        }
        public Color(byte[] input)
        {
            if (input.Length == 4)
            {
                r = (byte)(input[0] * 16);
                g = (byte)(input[1] * 16);
                b = (byte)(input[2] * 16);
            }
            else
                throw new ArgumentException("input color not fit");

        }
        public Color(byte r, byte g, byte b, byte a = byte.MaxValue)
        {
            this.r = r;
            this.g = g;
            this.b = b;
        }

        private byte HexToByte(char hex)
        {
            if (hex >= 'a' && hex <= 'f')
            {
                return (byte)(hex - 'a' + 10);
            }
            else if (hex >= '0' && hex <= '9')
            {
                return (byte)(hex - '0');
            }
            else
                throw new ArgumentException("input is not hexadecimal");
        }
        private byte HexToByte(char hex1, char hex2)
        {
            return (byte)(HexToByte(hex1) * 16 + HexToByte(hex2));
        }

        public override string ToString()
        {
            StringBuilder hex = new StringBuilder(7);
            hex.Append("#");
            hex.AppendFormat("{0:x2}", r);
            hex.AppendFormat("{0:x2}", g);
            hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }
    }
}
