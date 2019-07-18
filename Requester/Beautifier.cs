using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
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


        public void BeatyRichTextBox(string input, RichTextBox output)
        {
            const int indentSize = 4;
            StringBuilder sb = new StringBuilder();
            JSONobject status = new JSONobject(0);
            colorPalete palete = new colorPalete(false);


            foreach (char item in input)
            {
                if (status.isEscaped)
                {
                    status.isEscaped = false;
                    sb.Append(item);
                }
                else
                {
                    switch (item)
                    {
                        case '{':
                            status.depth++;
                            status.leftRight = true;
                            AppendText(output, item.ToString() + "\n" + new string(' ', status.depth * indentSize), palete.objectBracket);
                            break;
                        case '}':
                            status.depth--;
                            AppendText(output, "\n" + new string(' ', status.depth * indentSize) + item.ToString(), palete.objectBracket);
                            break;
                        case '"':
                            sb.Append(item);
                            if (status.leftRight)
                            { //left
                                if (status.inMarks)
                                { // second
                                    AppendText(output, sb.ToString(), palete.objectName);
                                    sb.Clear();
                                }
                            }
                            else
                            { //right
                                if (status.inMarks)
                                { // second
                                    AppendText(output, sb.ToString(), palete.text);
                                    sb.Clear();
                                }
                            }
                            status.inMarks = !status.inMarks;
                            break;
                        case '[':
                            status.depth++;
                            status.leftRight = true;
                            AppendText(output, item.ToString() + "\n" + new string(' ', status.depth * indentSize), palete.arrayBracket);
                            break;
                        case ']':
                            status.depth--;
                            AppendText(output, "\n" + new string(' ', status.depth * indentSize) + item.ToString(), palete.arrayBracket);
                            break;
                        case ':': //colon
                            if (status.inMarks)
                            {
                                sb.Append(item);
                            }
                            else
                            {
                                status.leftRight = false;
                                AppendText(output, item.ToString(), palete.colon);
                            }
                            break;
                        case ',':
                            status.leftRight = true;
                            sb.Append(item);
                            sb.Append("\n" + new string(' ', status.depth * indentSize));
                            break;
                        case '\\':
                            status.isEscaped = true;
                            break;
                        case '\n':
                        case '\r':
                        case ' ':
                            if (status.inMarks)
                                sb.Append(item);
                            break;
                        default:
                            if (!status.inMarks && (
                                item == '0' ||
                                item == '1' ||
                                item == '2' ||
                                item == '3' ||
                                item == '4' ||
                                item == '5' ||
                                item == '6' ||
                                item == '7' ||
                                item == '8' ||
                                item == '9' ||
                                item == '.'))
                            {
                                AppendText(output, item.ToString(), palete.number);
                            }
                            else
                            {
                                sb.Append(item);
                                if (sb.ToString() == "true" || sb.ToString() == "false")
                                {
                                    AppendText(output, sb.ToString(), palete.boolean);
                                    sb.Clear();
                                }
                            }
                            break;
                    }
                }
            }
        }

        public void AppendText(RichTextBox box, string text, string color)
        {
            var converter = new System.Windows.Media.BrushConverter();
            TextRange tr = new TextRange(box.Document.ContentEnd, box.Document.ContentEnd);
            tr.Text = text;
            tr.ApplyPropertyValue(TextElement.ForegroundProperty, (Brush)converter.ConvertFromString(color));
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

    struct JSONobject
    {
        public int depth;
        public bool isEscaped;
        public bool inMarks;
        /// <summary>
        /// left = true
        /// </summary>
        public bool leftRight;


        public JSONobject(int depth)
        {
            this.depth = depth;
            isEscaped = false;
            inMarks = false;
            leftRight = true;
        }

    }

    struct colorPalete
    {
        public string objectBracket;
        public string arrayBracket;
        public string objectName;
        public string text;
        public string number;
        public string boolean;
        public string colon;
        public colorPalete(bool light)
        {
            objectBracket = "#000000";
            arrayBracket = "#000000";
            objectName = "#000000";
            text = "#000000";
            number = "#000000";
            boolean = "#000000";
            colon = "#000000";

            if (light)
            {

            }
            else
            {
                objectBracket = "#ff2222";
                arrayBracket = "#22ff22";
                objectName = "#2222ff";
                text = "#ffffff";
                number = "#ffff22";
                boolean = "#22ffff";
                colon = "#666666";
            }
        }
    }
}
