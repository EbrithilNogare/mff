using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace gateNetwork
{
    public class Program
    {
        public static void Main(string[] args)
        {
            if (!checkArguments(args))
            {
                Console.WriteLine("Argument error.");
                return;
            }
            Network network;
            try
            {
                network = new Network(args[0]);

                string input;
                while ((input = Console.ReadLine()) != "end" && input != null)
                {
                    if (inputCorrect(input, network.getInputLength()))
                    {
                        string result = network.runNetwork(input);
                        Console.WriteLine(result);
                    }
                    else
                    {
                        Console.WriteLine("Syntax error.");
                    }
                }
            }
            catch (IOException)
            {
                Console.WriteLine("File error.");
                return;
            }
            catch (InvalidDataException e)
            {
                Console.WriteLine(e.Message);
                return;
            }
        }

        private static bool inputCorrect(string inputs, int inputLength)
        {
            //check for invalid input
            bool inputsNotBinnary = false;
            for (int i = 0; i < inputs.Length; i += 2)
            {
                if (inputs[i] != '0' && inputs[i] != '1' && inputs[i] != '?')
                {
                    inputsNotBinnary = true;
                    break;
                }
                if (i + 1 < inputs.Length && inputs[i + 1] != ' ')
                {
                    inputsNotBinnary = true;
                    break;
                }
            }
            if (inputs.Length != inputLength * 2 - 1 || inputsNotBinnary || inputs == "")
            {
                return false;
            }
            return true;
        }

        private static bool checkArguments(string[] args)
        {
            return args.Length == 1;
        }
    }
    class Gate
    {
        string[] inputs;
        string[] outputs;
        /// <summary>
        /// string = input
        /// string = output
        /// </summary>
        Dictionary<string, string> definitions = new Dictionary<string, string>();

        public Gate(StreamReader sr)
        {
            //inputs
            string input = sr.ReadLine();
            if (input == "inputs")
            {
                inputs = new string[0];
            }
            else
            {
                input = input.Substring("inputs ".Length);
                inputs = input.Split(' ');
            }
            //outputs
            input = sr.ReadLine();
            input = input.Substring("outputs ".Length);
            outputs = input.Split(' ');

            //definitions
            while ((input = sr.ReadLine()) != "end")
            {
                string inputDef;
                string outputDef;

                if (inputs.Length == 0)
                {
                    inputDef = "";
                    outputDef = input;
                }
                else
                {
                    inputDef = input.Substring(0, inputs.Length * 2 - 1);
                    outputDef = input.Substring(inputs.Length * 2);
                }

                inputDef = string.Join("", inputDef.Split(' '));
                outputDef = string.Join("", outputDef.Split(' '));

                definitions.Add(inputDef, outputDef);
            }
        }
        public string computeGate(string inputsForCompute)
        {
            string output;
            if (definitions.ContainsKey(inputsForCompute))
            {
                output = definitions[inputsForCompute];
            }
            else if(inputsForCompute.Contains("?"))
            {
                output = new string('?', outputs.Length);
            }
            else
            {
                output = new string('0', outputs.Length);
            }
            return output;
        }
        public string[] getInputNames()
        {
            return inputs;
        }
        public string[] getOutputNames()
        {
            return outputs;
        }
        public void showGate()
        {
            Console.WriteLine("GATE:");
            Console.WriteLine("inputs:");
            foreach (string input in inputs)
                Console.WriteLine(input);
            Console.WriteLine();

            Console.WriteLine("outputs:");
            foreach (string output in outputs)
                Console.WriteLine(output);
            Console.WriteLine();

            Console.WriteLine("definitions:");
            foreach (KeyValuePair<string, string> definition in definitions)
                Console.WriteLine("{0}|{1}", definition.Key, definition.Value);
            Console.WriteLine("end of GATE");
        }
    }
    class Network
    {
        string[] inputs;
        string[] outputs;

        /// <summary>
        /// string  = output name;
        /// char[0] = actual value;
        /// char[1] = new value;
        /// </summary>
        Dictionary<string, char[]> memory = new Dictionary<string, char[]>();
        /// <summary>
        /// jméno_instance_hradla;
        /// jméno_typu_hradla_;
        /// </summary>
        Dictionary<string, GateInstance> gates = new Dictionary<string, GateInstance>();
        Dictionary<string, Gate> gateDefinitions = new Dictionary<string, Gate>();
        Dictionary<string, string> networkDefinitions = new Dictionary<string, string>();

        public int getInputLength()
        {
            return inputs.Length;
        }

        public Network(string fileName)
        {
            using (var sr = new CountingReader(fileName))
            {
                string input;
                while ((input = sr.ReadLine()) != null)
                {
                    string[] splittedInput = input.Split(' ');
                    switch (splittedInput[0])
                    {
                        case "gate":
                            gateDefinitions.Add(splittedInput[1], new Gate(sr));
                            break;
                        case "network":
                            loadNetwork(sr);
                            break;
                        default:
                            if (!input.Contains(";") && input!="")
                            {
                                throw new InvalidDataException("Line " + sr.LineNumber.ToString() + ": Missing keyword.");//todo
                            }
                            break;
                    }
                }
            }
            memory["0"] = new char[] { '0', '0' };
            memory["1"] = new char[] { '1', '1' };
        }

        private void loadNetwork(StreamReader sr)
        {
            //inputs
            string input = sr.ReadLine();
            input = input.Substring("inputs ".Length);
            inputs = input.Split(' ');

            //outputs
            input = sr.ReadLine();
            input = input.Substring("outputs ".Length);
            outputs = input.Split(' ');

            //definitions
            while ((input = sr.ReadLine()) != "end")
            {
                if (input == ""||input.Contains(";")) //skip empty row
                {

                }
                else if (input.Substring(0, "gate".Length) == "gate") //gate naming
                {
                    string[] splittedInput = input.Split(' ');
                    gates.Add(splittedInput[1], new GateInstance(splittedInput[2]));
                }
                else //gate connections
                {
                    string[] splittedInput = input.Split(new string[] { "->" }, StringSplitOptions.None);                    
                    networkDefinitions.Add(splittedInput[0], splittedInput[1]);

                    if (!memory.ContainsKey(splittedInput[1]))
                    {
                        memory[splittedInput[1]] = new char[] {'?', '?'};
                    }
                }
            }
        }

        public void showNetwork()
        {
            Console.WriteLine("NETWORK:");
            Console.WriteLine("inputs:");
            foreach (string input in inputs)
                Console.WriteLine(input);
            Console.WriteLine();

            Console.WriteLine("outputs:");
            foreach (string output in outputs)
                Console.WriteLine(output);
            Console.WriteLine();

            Console.WriteLine("gates:");
            foreach (KeyValuePair<string, GateInstance> gate in gates)
            {
                Console.WriteLine("{0}|{1}", gate.Key, gate.Value.gateDefinition);
            }
            Console.WriteLine();

            Console.WriteLine("gateDefinitions:");
            foreach (KeyValuePair<string, Gate> gateDefinition in gateDefinitions)
            {
                Console.WriteLine(gateDefinition.Key + ":");
                gateDefinition.Value.showGate();
            }
            Console.WriteLine();

            Console.WriteLine("networkDefinitions:");
            foreach (KeyValuePair<string, string> networkDefinition in networkDefinitions)
                Console.WriteLine("{0}|{1}", networkDefinition.Key, networkDefinition.Value);
            Console.WriteLine("end of NETWORK");
        }

        public void showMemory()
        {
            Console.WriteLine("---------MEMORY---------");
            foreach (KeyValuePair<string, char[]> memoryCell in memory)
            {
                if (memoryCell.Value[0] != memoryCell.Value[1])
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                }
                Console.WriteLine("{0,-15}{1,3} ->{2,3}", memoryCell.Key, memoryCell.Value[0], memoryCell.Value[1]);
                Console.ForegroundColor = ConsoleColor.Gray;
            }
            Console.WriteLine("-----end of MEMORY------");
            Console.WriteLine();
        }

        public void showGateInputs()
        {
            Console.WriteLine("---------Gate inputs---------");

            foreach (var gate in gates)
            {
                Console.WriteLine("{0,-10} -> {1}", gate.Key, gate.Value.previousInputs);
            }

            Console.WriteLine("-----end of Gate inputs------");
            Console.WriteLine();
        }

        public string runNetwork(string networkInputs)
        {
            //load default values to network
            networkInputs = string.Join("", networkInputs.Split(' '));
            int inputIndex = 0;
            foreach(string input in inputs)
            {
                memory[input][0] = networkInputs[inputIndex];
                memory[input][1] = networkInputs[inputIndex];
                inputIndex++;
            }

            int tact = 1;
            bool differencesIn = true;
            int maxTacts = 1000*1000;
            for (; differencesIn && tact <= maxTacts; tact++)
            {
                
                differencesIn = false;
                foreach (KeyValuePair<string, GateInstance> gateName in gates) //for every gate in network
                {
                    Gate gate = gateDefinitions[gateName.Value.gateDefinition];
                    

                    string[] gateOutputNames = gate.getOutputNames();

                    string[] gateInputNames = gate.getInputNames();
                    char[] gateInputs = new char[gateInputNames.Length];
                    int gateInputsIndex = 0;
                    
                    for (int i = 0; i < gateInputNames.Length; i++) //for every input to gate
                    {
                        string fullGateInputName = gateName.Key + "." + gateInputNames[i];
                        string memoryCellName = networkDefinitions[fullGateInputName];

                        if (memory.ContainsKey(memoryCellName)) {
                            gateInputs[gateInputsIndex] = memory[memoryCellName][0];
                        }
                        else
                        {
                            gateInputs[gateInputsIndex] = '?';
                        }
                        gateInputsIndex++;                        
                    }

                    string joinedGateInputs = string.Join("", gateInputs);

                    string outputOfGate = gate.computeGate(joinedGateInputs);
                    
                    if (gateName.Value.previousInputs != joinedGateInputs)//change in inputs to gate
                    {
                        differencesIn = true;
                        gateName.Value.previousInputs = joinedGateInputs;
                    }

                    for (int i = 0; i < gateOutputNames.Length; i++)
                    {
                        string fullGateInputName = gateName.Key + "." + gateOutputNames[i];
                        try{
                            memory[fullGateInputName][1] = outputOfGate[i];
                        }
                        catch (System.Collections.Generic.KeyNotFoundException)
                        {
                            memory[fullGateInputName] = new char[] { '?', outputOfGate[i] };
                        }
                    }
                }

                /*/
                Console.WriteLine(tact);
                showMemory();
                showGateInputs();
                /**/
                //save new memory
                foreach (KeyValuePair<string,char[]> memoryCell in memory)
                {
                    memoryCell.Value[0] = memoryCell.Value[1];
                }
            }
            tact-=tact==2||tact== maxTacts+1? 1 :2;

            var output = new StringBuilder();
            output.Append(tact);
            foreach (string networkOutput in outputs)
                output.Append(" " + memory[networkDefinitions[networkOutput]][0]);//take memory cell that is connected with output
            return output.ToString();
        }
    }
    class CountingReader : StreamReader
    {
        private int _lineNumber = 0;
        public int LineNumber { get { return _lineNumber; } }

        public CountingReader(string fileName) : base(fileName) { }

        public override string ReadLine()
        {
            _lineNumber++;
            return base.ReadLine();
        }
    }
    class GateInstance
    {
        public string gateDefinition;
        public string previousInputs;

        public GateInstance(string gateDefinition)
        {
            this.gateDefinition = gateDefinition;
            previousInputs = "";
        }
    }
}
