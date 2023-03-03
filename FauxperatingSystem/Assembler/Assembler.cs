using System;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace FauxperatingSystem
{
    public static class Assembler
    {
        public static (string instruction, int opcode)[] INSTRUCTION_SET = {
            ("J", 0b00000),
            ("JR", 0b00001),
            ("JZ", 0b00010),
            ("JNZ", 0b00011),
            ("NOP", 0b11111),
            ("MUL", 0b10000),
            ("DIV", 0b10001),
            ("ADD", 0b10010),
            ("SUB", 0b10011),
            ("AND", 0b10100),
            ("OR", 0b10101),
            ("XOR", 0b10110),
            ("XNOR", 0b10111),
            ("NOT", 0b11000),
            ("SHR", 0b11001),
            ("SHL", 0b11010),
            ("SEQ", 0b01001),
            ("SGT", 0b01010),
            ("SLT", 0b01011),
            ("INT", 0b01100),
            ("CALL", 0b01111),
            ("RET", 0b11011),
            ("MOV", 0b11100),
            ("MOVV", 0b11101),
            ("INC", 0b01101),
            ("DEC", 0b01110)
        };

        private static List<(string name, string value)> Variables = new List<(string name, string value)>();
        private static List<List<Token>> Instructions = new List<List<Token>>();

        public static void GetAssemblyInformation(List<Token> tokens)
        {
            List<Token> currentLine = new List<Token>();

            int mode = -1;
            for(int i = 0; i < tokens.Count; i++)
            {
                Token curToken = tokens[i];

                if(curToken.Type == TokenType.Newline)
                {
                    if(mode == 0) // Instruction
                        Instructions.Add(currentLine.Where(x => x.Type != TokenType.Seperator).ToList());
                    if (mode == 1)
                        Variables.Add((currentLine[0].value, currentLine[3].value));

                    currentLine.Clear();
                    mode = -1;
                }

                if(curToken.Type == TokenType.Instruction)
                    mode = 0;

                if (curToken.Type == TokenType.DefineBytes)
                {
                    mode = 1;
                    if (i - 2 >= 0)
                    {
                        currentLine.Add(tokens[i-2]);
                        currentLine.Add(tokens[i-1]);
                    }
                }

                if(mode != -1)
                    currentLine.Add(curToken);
            }
        }
        
        public static byte[] Assemble()
        {
            List<byte> program = new List<byte>();
            for(int i = 0; i < Variables.Count; i++)
            {
                byte[] bytes = Encoding.ASCII.GetBytes(Variables[i].value);
                program.AddRange(bytes);
                program.Add(0x00); // this isn't gonna work out in the long run (get it?)
            }
            short textLocation = (short)(program.Count + 2);    

            for(int i = 0; i < Instructions.Count; i++)
            {
                var instruc = Instructions[i];
                if (instruc[0].value.ToLower() == "int")
                {
                    var s = BitConverter.GetBytes(short.Parse(instruc[1].value));
                    program.Add(s[1]);
                    program.Add(s[0]);
                    continue;
                }

                BitArray x = new BitArray(16);
                x = x.AddIntAtIndices(INSTRUCTION_SET.Where(d => d.instruction.ToLower() == instruc[0].value.ToLower()).FirstOrDefault().opcode, 0, 5);

                var instrucName = instruc[0].value.ToLower();
                if (instruc.Count > 1 && instrucName == "j")
                    x.AddIntAtIndices(int.Parse(instruc[1].value), 5, 16);
                else if(instruc.Count > 1 && (instrucName != "j" && instrucName != "int"))
                {
                    if(instrucName != "movv")
                    {
                        for(int j = 1; j < instruc.Count; j++) // every argument
                        {
                            var arg = instruc[j];
                            x.AddIntAtIndices(int.Parse(arg.value), 5+(3*(j-1)), 5+(3*(j-1))+3);
                        }
                    }
                    else
                    {
                        x.AddIntAtIndices(int.Parse(instruc[1].value),5,8);
                        x.AddIntAtIndices(Variables.FindIndex(x => x.name == instruc[2].value),8,16);
                    }
                }

                x = BitsReverse(x);

                byte[] shrarray = new byte[2];
                x.CopyTo(shrarray, 0);

                program.Add(shrarray[1]);
                program.Add(shrarray[0]);
            }

            program.Insert(0, (byte)(textLocation & 0xFF)); // location of text
            program.Insert(1, (byte)(textLocation >> 8)); // location of text
            return program.ToArray();
        }

        private static BitArray AddIntAtIndices(this BitArray x, int integer, int start, int end)
        {
            for(int i = end - 1; i >= start; i--)
            {
                x.Set(i, (integer & (1 << end - 1 - i)) != 0);
            }
            return x;
        }

        private static BitArray BitsReverse(BitArray bits)
        {
            int len = bits.Count;
            BitArray a = new BitArray(bits);
            BitArray b = new BitArray(bits);

            for (int i = 0, j = len - 1; i < len; ++i, --j)
            {
                a[i] = a[i] ^ b[j];
                b[j] = a[i] ^ b[j];
                a[i] = a[i] ^ b[j];
            }

            return a;
        }
    }
}