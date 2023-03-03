using System;

namespace FauxperatingSystem
{
	public static partial class BytecodeParser
	{
		public static void ParseNextBytecode()
		{
            short x = BitConverter.ToInt16(new byte[] { FauxSystem.Memory[FauxSystem.PC], FauxSystem.Memory[FauxSystem.PC+1] });

			int opcode = (x >> 3) & ((1 << 5) - 1); // First 5 bits. Really annoying.
			int I = (x >> 0) & ((1 << 11) - 1); // Last 11 bits.
			int R1 = 7 - (x >> 5) & ((1 << 3) - 1); // First 3 bits after the first 5
			int R2 = 7 - (x >> 8) & ((1 << 3) - 1); // Second 3 bits after the first 5
            int D1 = x >> 8;//x & 0xFF; // destination 1. Last 8 bits.
			int D2 = x >> 11;//x & 0x1F; // destination 2. Last 5 bits.

            switch (opcode)
			{
				case 0b00000: // J I
					FauxSystem.PC += (ushort)(I*2); // not particularly good practice
					break;
				case 0b00001: // JR R1
					FauxSystem.PC = (ushort)FauxSystem.Register[R1];
					break;
                case 0b00010: // JZ R1 R2
					if (FauxSystem.Register[R2] == 0)
						FauxSystem.PC = (ushort)FauxSystem.Register[R1];
					else
                        FauxSystem.PC += 2;
                    break;
                case 0b00011: // JNZ R1 R2
                    if (FauxSystem.Register[R2] != 0)
                        FauxSystem.PC = (ushort)FauxSystem.Register[R1];
                    else
                        FauxSystem.PC += 2;
                    break;
                case 0b11111: // NOP
                    FauxSystem.PC += 2;
                    break;
				case 0b10000: // MUL R1 R2 D
					FauxSystem.Register[D2] = (short)(FauxSystem.Register[R1] * FauxSystem.Register[R2]);
                    FauxSystem.PC += 2;
                    break;
                case 0b10001: // DIV R1 R2 D
                    FauxSystem.Register[D2] = (short)(FauxSystem.Register[R1] / FauxSystem.Register[R2]);
                    FauxSystem.PC += 2;
                    break;
                case 0b10010: // ADD R1 R2 D
                    FauxSystem.Register[D2] = (short)(FauxSystem.Register[R1] + FauxSystem.Register[R2]);
                    FauxSystem.PC += 2;
                    break;
                case 0b10011: // SUB R1 R2 D
                    FauxSystem.Register[D2] = (short)(FauxSystem.Register[R1] - FauxSystem.Register[R2]);
                    FauxSystem.PC += 2;
                    break;
                case 0b10100: // AND R1 R2 D
                    FauxSystem.Register[D2] = (short)(FauxSystem.Register[R1] & FauxSystem.Register[R2]);
                    FauxSystem.PC += 2;
                    break;
                case 0b10101: // OR R1 R2 D
                    FauxSystem.Register[D2] = (short)(FauxSystem.Register[R1] | FauxSystem.Register[R2]);
                    FauxSystem.PC += 2;
                    break;
                case 0b10110: // XOR R1 R2 D
                    FauxSystem.Register[D2] = (short)(FauxSystem.Register[R1] ^ FauxSystem.Register[R2]);
                    FauxSystem.PC += 2;
                    break;
                case 0b10111: // XNOR R1 R2 D. this seems useless lolololol
                    FauxSystem.Register[D2] = (short)(FauxSystem.Register[R1] == FauxSystem.Register[R2] ? 1 : 0);
                    FauxSystem.PC += 2;
                    break;
                case 0b11000: // NOT R1 D
                    FauxSystem.Register[D1] = (short)(~FauxSystem.Register[R1]);
                    FauxSystem.PC += 2;
                    break;
                case 0b11001: // SHR R1 D
                    FauxSystem.Register[D1] = (short)(FauxSystem.Register[R1] >> 1);
                    FauxSystem.PC += 2;
                    break;
                case 0b11010: // SHL R1 D
                    FauxSystem.Register[D1] = (short)(FauxSystem.Register[R1] << 1);
                    FauxSystem.PC += 2;
                    break;
                case 0b01001: // SEQ R1 R2 D
                    FauxSystem.Register[D2] = FauxSystem.Register[R1] == FauxSystem.Register[R2] ? (short)1 : (short)0;
                    FauxSystem.PC += 2;
                    break;
                case 0b01010: // SGT R1 R2 D
                    FauxSystem.Register[D2] = FauxSystem.Register[R1] > FauxSystem.Register[R2] ? (short)1 : (short)0;
                    FauxSystem.PC += 2;
                    break;
                case 0b01011: // SLT R1 R2 D
                    FauxSystem.Register[D2] = FauxSystem.Register[R1] < FauxSystem.Register[R2] ? (short)1 : (short)0;
                    FauxSystem.PC += 2;
                    break;
                case 0b01100: // INT I
                    InvokeInterrupt((ushort)x); // x, not I, because of how INT works
                    FauxSystem.PC += 2;
                    break;
                case 0b01111: // CALL R1
                    FauxSystem.SP++;
                    FauxSystem.Stack.Push(FauxSystem.PC);
                    FauxSystem.PC = (ushort)FauxSystem.Register[R1];
                    break;
                case 0b11011: // RET
                    FauxSystem.PC = FauxSystem.Stack.Peek();
                    FauxSystem.SP--;
                    break;
                case 0b11100: // MOV R, R
                    FauxSystem.Register[R2] = FauxSystem.Register[R1];
                    FauxSystem.PC += 2;
                    break;
                case 0b11101: // MOVV R, D
                    FauxSystem.Register[R1] = (short)FauxSystem.Variables[D1];
                    FauxSystem.PC += 2;
                    break;
                case 0b01101:
                    FauxSystem.Register[R1]++;
                    FauxSystem.PC += 2;
                    break;
                case 0b01110:
                    FauxSystem.Register[R1]--;
                    FauxSystem.PC += 2;
                    break;
                default:
                    FauxSystem.Halt($"Illegal Instruction at {FauxSystem.PC}");
                    break;
            }
        }
	}
}