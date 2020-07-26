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
		/// <summary>
		/// Method gets text and parse it by its type (supported are JSON and XML, other formats will be interpreted as plain text)
		/// and put it into output with colored keywords etc. and indent by depth of element in context
		/// </summary>
		/// <param name="input">JSON, XML or plain text</param>
		/// <param name="output">RichTextBox which will be cleared and filled with new data</param>
		public void BeautyRichTextBox(string input, RichTextBox output)
        {
            const int indentSize = 4;
            StringBuilder sb = new StringBuilder();
			int depth = 0;
			string color = "#000000";

			if (input.Length == 0)
				return;

			// check if it is JSON, XML(HTML), or plain text
			switch (input[0])
			{
				case '{': // json
					JSONParser jp = new JSONParser();
					foreach (char item in input)
					{
						int previousDepth = depth;
						JSONParser.JSONType type = jp.Step(item, ref depth);
						switch (type)
						{
							case JSONParser.JSONType.objectBracket: color = colorPalete.primary; break;
							case JSONParser.JSONType.arrayBracket: color = colorPalete.secondary; break;
							case JSONParser.JSONType.paramName: color = colorPalete.name; break;
							case JSONParser.JSONType.JSONstring: color = colorPalete.value; break;
							case JSONParser.JSONType.number: color = colorPalete.number; break;
							case JSONParser.JSONType.logic: color = colorPalete.logic; break;
							default: color = colorPalete.specialChar; break;
						}
						if (type == JSONParser.JSONType.whitespace)
							continue;
						
						if (previousDepth != depth || item == ',')
						{
							if(previousDepth > depth)
							{
								AppendText(output, "\n");
								AppendText(output, new String(' ', indentSize * depth), color);
								AppendText(output, item.ToString(), color);
							}
							else
							{
								AppendText(output, item.ToString(), color);
								AppendText(output, "\n");
								AppendText(output, new String(' ', indentSize * depth), color);
							}
						}
						else
						{
							AppendText(output, item.ToString(), color);
						}
					}
					break;
				case '<': // html / xml
					XMLParser xp = new XMLParser();
					int startEndWaiting = -1;
					foreach (var item in input)
					{
						var StepResult = xp.Step(item, out depth);

						switch (StepResult)
						{
							case XMLParser.states.left: color = colorPalete.primary; startEndWaiting = depth; continue;
							case XMLParser.states.tagName: color = colorPalete.name; break;
							case XMLParser.states.metadata: color = colorPalete.value; break;
							case XMLParser.states.right: color = colorPalete.primary; break;
							case XMLParser.states.slash: color = colorPalete.primary; break;
							case XMLParser.states.data: color = colorPalete.logic; break;
							default: color = colorPalete.specialChar; break;
						}

						if (depth > -1 || startEndWaiting > -1) // new line and indent
						{
							AppendText(output, "\n" + new String(' ', indentSize * (depth > -1 ? depth : startEndWaiting)), "#000000");
						}

						if (startEndWaiting > -1)
						{
							AppendText(output, "<", colorPalete.primary);
							startEndWaiting = -1;
						}

						AppendText(output, item.ToString(), color);
					}
					break;
				default: // txt
					AppendText(output, input, "#ffffff");
					break;
			}
        }
		/// <summary>
		/// Add text to end of RichTextBox and apply color to it
		/// </summary>
		/// <param name="box">RichTextBox</param>
		/// <param name="text">data to be placed to RichTextBox </param>
		/// <param name="color">color in format #xxxxxx where x is hexadecimal number 0-F</param>
		public void AppendText(RichTextBox box, string text, string color = "#000000")
        {
            var converter = new System.Windows.Media.BrushConverter();
            TextRange tr = new TextRange(box.Document.ContentEnd, box.Document.ContentEnd);
            tr.Text = text;
            tr.ApplyPropertyValue(TextElement.ForegroundProperty, (Brush)converter.ConvertFromString(color));
        }
    }
	/// <summary>
	/// non-deterministic Automat for parsing XML data
	/// </summary>
	class XMLParser
	{
		/// <summary>
		/// state of XML parsing automat
		/// </summary>
		public enum states
		{
			left,			// <
			tagName,		// a-z
			whitespace,		// ' '
			metadata,		// a-z
			right,			// >
			data,			// a-z0-9
			slash			// /
		}
		states state;
		int depth;
		char[] whitespaces = new char[] { ' ', '\t', '\n', '\r' };

		public XMLParser()
		{
			this.state = states.whitespace;
			this.depth = 0;
		}
		/// <summary>
		/// get symbol and get into new state of automat
		/// </summary>
		/// <param name="input">symbol to be parsed</param>
		/// <param name="depthOut">return depth if needed or -1 if not</param>
		/// <returns>returns state of XML parsing automat</returns>
		public states Step(char input, out int depthOut)
		{
			depthOut = -1;

			if(input == '<')
			{
				depthOut = depth = Math.Max(0, depth);
				state = states.left;
				depth++;
				return state;
			}

			if (input == '/' && state == states.left)
			{
				depth -= 2;
				depthOut = depth = Math.Max(0, depth);
				state = states.slash;
				return state;
			}

			if (state == states.left && input == '>')
			{
				depth -= 1;
				depthOut = depth = Math.Max(0, depth);
				state = states.right;
				return state;
			}

			switch (state)
			{
				case states.left:
					if (input == '?')
						depth--;
					state = states.tagName;
					break;

				case states.tagName:
					if (whitespaces.Contains(input))
						state = states.metadata;
					if(input == '>')
						state = states.right;
					break;

				case states.metadata:
					if (input == '>')
						state = states.right;
					break;

				case states.right:
					state = states.data;
					depthOut = depth = Math.Max(0, depth);
					break;

				case states.slash:
					if (input == '>')
					{
						depth--;
						state = states.right;
					}
					else
						state = states.tagName;
					break;
			}
			return state;
		}



	}
	class JSONParser
	{
		/*
			object bracket	{}
			array bracket	[]
			paramName		"string"
			values	
				string		"string"
				number		42.0E+1
				logic		true, false, null
			specialChar		,:
			whitespace		\n \r \t space
		*/
		List<bool> isObject;
		bool backslashed;
		ValueFormat valueFormat;
		bool afterColon;
		public enum JSONType
		{
			objectBracket,	// {}
			arrayBracket,	// []
			paramName,		// "a-z"
			JSONstring,		// "a-z0-9"
			number,			// 0-9.0-9E+-0-9
			logic,			// true, false, null
			specialChar,	// ,:
			whitespace		// ' '
		}
		/// <summary>
		/// type of value saved in object or array
		/// </summary>
		enum ValueFormat
		{
			none,
			text,
			number,
			logic
		}
		public JSONParser()
		{
			this.isObject = new List<bool>();
			this.backslashed = false;
			this.valueFormat = ValueFormat.none;
			this.afterColon = false;
		}
		char[] whitespaces = new char[] { ' ', '\t', '\n', '\r' };
		/// <summary>
		/// get symbol and get into new state of automat
		/// </summary>
		/// <param name="input">char to be parsed</param>
		/// <param name="depth">depth of object</param>
		/// <returns>returns of which type is that char</returns>
		public JSONType Step(char input, ref int depth)
		{
			// skip whitespaces on input
			if (whitespaces.Contains(input))
				return JSONType.whitespace;

			// parse as string has priority
			if(valueFormat == ValueFormat.text) {
				if (!backslashed && input == '"')
				{
					valueFormat = ValueFormat.none;
				}

				if (input == '\\')
					backslashed = true;
				else
					backslashed = false;

				if (afterColon)
					return JSONType.JSONstring;
				else
					return JSONType.paramName;
			}

			switch (input)
			{
				case '{':
					depth++;
					afterColon = false;
					isObject.Add(true);
					return JSONType.objectBracket;
				case '}':
					depth--;
					isObject.RemoveAt(isObject.Count() - 1);
					return JSONType.objectBracket;
				case '[':
					depth++;
					isObject.Add(false);
					return JSONType.arrayBracket;
				case ']':
					depth--;
					isObject.RemoveAt(isObject.Count() - 1);
					return JSONType.arrayBracket;
				case '"':
					valueFormat = ValueFormat.text;
					if (afterColon)
						return JSONType.JSONstring;
					else
						return JSONType.paramName;
				case ',':
					afterColon = false;
					valueFormat = ValueFormat.none;
					return JSONType.specialChar;
				case ':':
					afterColon = true;
					return JSONType.specialChar;
			}

			if(valueFormat == ValueFormat.none)
			{
				if (input == 't' || input == 'f' || input == 'n')
				{
					valueFormat = ValueFormat.logic;
				}
				else
				{
					valueFormat = ValueFormat.number;
				}
			}

			if (valueFormat == ValueFormat.logic)
				return JSONType.logic;

			if (valueFormat == ValueFormat.number)
				return JSONType.number;


			// there should be no chance to get here
			throw new NotSupportedException();
		}
	}
	/// <summary>
	/// Color structure for colors in Beautifier
	/// </summary>
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

	/// <summary>
	/// color pallete for keywords, same for XML and JSON
	/// </summary>
	static class colorPalete
    {
        public const string primary = "#DA70D6";
        public const string secondary = "#F78C63";
		public const string name = "#FFCB6B";
        public const string value = "#C3E88D";
        public const string number = "#89DDFF";
        public const string logic = "#89DDFF";
        public const string specialChar = "#666666";
	}
}
