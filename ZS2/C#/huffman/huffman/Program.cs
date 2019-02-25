using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace huffman
{
    class Program
    {
        static void Main(string[] args)
        {
            if (!checkArguments(args))
            {
                Console.WriteLine("Argument Error");
                Console.ReadKey();
                return;
            }
            try
            {
                using (var br = new BinaryReader(File.Open(args[0], FileMode.Open)))
                using (var bw = new BinaryWriter(File.Open(args[0] + ".huff", FileMode.Create)))
                {



                    var huffman = new Huffman(br, bw);
                    huffman.Encode();




                }
            }
            catch (IOException e)
            {
                Console.WriteLine("File Error");
                Console.ReadKey();
                return;
            }
        }

        static bool checkArguments(string[] args)
        {
            return (args.Length == 1);
        }
    }

    class Huffman
    {
        private List<Node> nodes = new List<Node>();
        public Node root { get; set; }
        private Dictionary<byte, ulong> frequency = new Dictionary<byte, ulong>();
        private Dictionary<byte, List<bool>> translator = new Dictionary<byte, List<bool>>();
        private BinaryReader br;
        private BinaryWriter bw;

        public Huffman(BinaryReader br, BinaryWriter bw)
        {
            this.br = br;
            this.bw = bw;
        }

        /// <summary>
        /// buil huffman tree
        /// </summary>
        /// <param name="br">from where to get binary data</param>
        public void BuildTreeFromFrequency()
        {
            ulong[] frequencyArray = new ulong[256];
            while (br.BaseStream.Position != br.BaseStream.Length)
            {
                byte[] inputByte = br.ReadBytes(4 * 1024);
                for (int i = 0; i < inputByte.Length; i++)
                {
                    frequencyArray[inputByte[i]]++;
                }
            }

            for (int i = 0; i < frequencyArray.Length; i++)
            {
                if (frequencyArray[i] != 0)
                    frequency.Add((byte)i, frequencyArray[i]);
            }


            foreach (KeyValuePair<byte, ulong> symbol in frequency.OrderBy(i => i.Key))
            {
                nodes.Add(new Node() { value = symbol.Key, count = symbol.Value });
            }

            while (nodes.Count > 1)
            {
                List<Node> orderedNodes = nodes.OrderBy(node => node.count).ToList<Node>();

                if (orderedNodes.Count >= 2)
                {
                    List<Node> taken = orderedNodes.Take(2).ToList<Node>();

                    Node parent = new Node()
                    {
                        value = 0,
                        count = taken[0].count + taken[1].count,
                        left = taken[0],
                        right = taken[1]
                    };

                    nodes.Remove(taken[0]);
                    nodes.Remove(taken[1]);
                    nodes.Add(parent);
                }

                this.root = nodes.FirstOrDefault();

            }

        }

        public void buildTranslator(Node node, List<bool> translation)
        {
            if (node.left == null && node.right == null)
            {

                translator[node.value] = new List<bool>(translation);
            }

            if (node.left != null)
            {
                translation.Add(false);
                buildTranslator(node.left, translation);
                translation.RemoveAt(translation.Count - 1);
            }

            if (node.right != null)
            {
                translation.Add(true);
                buildTranslator(node.right, translation);
                translation.RemoveAt(translation.Count - 1);
            }
        }

        /// <summary>
        /// Show binary tree of huffman tree
        /// </summary>
        /// <param name="tw">where to show tree</param>
        /// <param name="node">first node to start</param>
        /// <param name="first">if it's first call in recursion</param>
        public void ShowTree(TextWriter tw, Node node, bool first = true)
        {
            if (!first)
            {
                tw.Write(" ");
            }

            if (node.left == null && node.right == null)
            {

                tw.Write("*{0}:{1}", (int)node.value, node.count);
            }
            else
            {
                tw.Write(node.count);
            }

            if (node.left != null)
            {
                ShowTree(tw, node.left, false);
            }
            if (node.right != null)
            {
                ShowTree(tw, node.right, false);
            }

        }

        /// <summary>
        /// show translator values
        /// </summary>
        /// <param name="tw"></param>
        public void ShowTranslator(TextWriter tw)
        {
            foreach (KeyValuePair<byte, List<bool>> translatorPair in translator)
            {
                tw.Write(translatorPair.Key.ToString("X2") + ": ");
                foreach (var pair in translatorPair.Value)
                {
                    tw.Write(pair ? "1" : "0");
                }
                tw.WriteLine();
            }
        }

        /// <summary>
        /// encode data from input stream
        /// </summary>
        public void Encode()
        {
            BuildTreeFromFrequency();
            //huffman.ShowTree(Console.Out, huffman.root);

            buildTranslator(this.root, new List<bool>());
            //huffman.ShowTranslator(Console.Out);
                       
            br.BaseStream.Position = 0;

            writeFileHeader();
            writeTreeDescription(this.root);
            encodeContext();
        }

        private void encodeContext()
        {
            var outputBuffer = new bool[8];
            byte outputBufferIndex = 0;
            
            for (int i = 0; i < 8; i++)
            {
                bw.Write((byte)0x00);
            }

            while (br.BaseStream.Position != br.BaseStream.Length)
            {
                byte[] inputByte = br.ReadBytes(4 * 1024);
                for (int i = 0; i < inputByte.Length; i++)
                {
                    foreach (var pair in translator[inputByte[i]])
                    {
                        outputBuffer[outputBufferIndex++] = pair;
                        if (outputBufferIndex == 8)
                        {
                            bw.Write(byteConstructor(outputBuffer));
                            outputBufferIndex = 0;
                        }
                    }
                }
            }
            if (outputBufferIndex != 0)
            {
                while (outputBufferIndex != 8)
                {
                    outputBuffer[outputBufferIndex] = false;
                    outputBufferIndex++;
                }
                bw.Write(byteConstructor(outputBuffer));
            }
        }

        /// <summary>
        /// Makes byte from bool array
        /// </summary>
        /// <param name="outputBuffer">must have length equal to 8</param>
        /// <returns>LE byte</returns>
        private byte byteConstructor(bool[] outputBuffer)
        {
            if (outputBuffer.Length != 8)
            {
                throw new Exception("overflowed byte in 'byteConstructor'");
            }
            byte result = 0;
            int index = 7; //edited for LE
            foreach (bool b in outputBuffer)
            {
                if (b)
                    result |= (byte)(1 << (7 - index));
                index--; //edited for LE
            }
            return result;
        }

        private void writeTreeDescription(Node node)
        {
            byte[] byteTemp = new byte[8];
            if (node.left == null && node.right == null)
            {
                byteTemp = BitConverter.GetBytes(node.count * 2 + 1);
                byteTemp[7] = node.value;
                bw.Write(byteTemp);
            }
            else
            {
                byteTemp = BitConverter.GetBytes(node.count * 2);
                byteTemp[7] = 0;
                bw.Write(byteTemp);
            }

            if (node.left != null)
            {
                writeTreeDescription(node.left);
            }

            if (node.right != null)
            {
                writeTreeDescription(node.right);
            }


        }

        private void writeFileHeader()
        {
            foreach (byte i in new byte[] { 0x7B, 0x68, 0x75, 0x7C, 0x6D, 0x7D, 0x66, 0x66 })
                bw.Write(i);
        }
    }

    /// <summary>
    /// leaf with 2 childs, value and count
    /// </summary>
    class Node
    {
        public byte value { get; set; }
        public ulong count { get; set; }
        public Node left { get; set; }
        public Node right { get; set; }
    }
}
