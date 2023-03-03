using System;

namespace FauxperatingSystem
{
	public static partial class BytecodeParser
	{
		public static void InvokeInterrupt(ushort x)
		{
			short R0 = FauxSystem.Register[0];
			short R1 = FauxSystem.Register[1];

			x = BitConverter.ToUInt16(BitConverter.GetBytes(x).Reverse().ToArray()); // why would i make INTs this way

			switch(x)
			{
				case 0x6400: // CLS
					Console.Clear();
					break;
				case 0x6401: // Console.Write
					Console.Write(ReadVariableInMem(R0));
					break;
				case 0x6410: // change foreground color
					Console.ForegroundColor = (ConsoleColor)R0;
					break;
                case 0x6411: // change background color
                    Console.BackgroundColor = (ConsoleColor)R0;
                    break;
				case 0x6412: // reset coor
					Console.ResetColor();
					break;
				case 0x6420: // set cursor pos
					Console.SetCursorPosition(R0, R1);
					break;
                case 0x6421: // get cursor pos
					FauxSystem.Register[0] = (short)Console.CursorLeft;
					FauxSystem.Register[1] = (short)Console.CursorTop;
                    break;
                case 0x6440: // set window size
                    Console.SetWindowSize(R0, R1);
                    break;
                case 0x6441: // get window size
                    FauxSystem.Register[0] = (short)Console.WindowWidth;
                    FauxSystem.Register[1] = (short)Console.WindowHeight;
                    break;
                case 0x6442: // set window left/top
                    Console.SetWindowPosition(R0, R1);
                    break;
                case 0x6443: // get window left/top
                    FauxSystem.Register[0] = (short)Console.WindowLeft;
                    FauxSystem.Register[1] = (short)Console.WindowTop;
                    break;
				case 0x6480: // read line
					string input = Console.ReadLine();
					StoreInMemory(R0, input);
					break;
				case 0x6481: // read line limit length
					string d = "";
					while(true)
					{
						var k = Console.ReadKey();
						if(k.Key == ConsoleKey.Backspace && d.Length > 0)
						{
							d = d.Substring(0, d.Length - 2);
							if (Console.CursorLeft == 0 && d.Length > 0)
								Console.SetCursorPosition(Console.WindowWidth - 1, Console.CursorTop--);
							else
								Console.SetCursorPosition(Console.CursorLeft--, Console.CursorTop);
							continue;
						}
						
						if(k.Key == ConsoleKey.Enter)
						{
							StoreInMemory(R0, d);
							break;
						}

						if (d.Length <= R1)
							d += k.KeyChar;
					}
					break;
				case 0x6482: // read key 
					var key = Console.ReadKey(false);
					StoreInMemory(R0, ((byte)key.Key).ToString());
					break;
                case 0x6483: // read key hidden
                    var key2 = Console.ReadKey(true);
                    StoreInMemory(R0, ((byte)key2.Key).ToString());
                    break;
                case 0x64FF: // shutdown emulator
                    Environment.Exit(0);
                    break;
				case 0x6200: // get current directory
					StoreInMemory(R0, Storage.CurrentDirectory);
					break;
                case 0x6201: // create file
                    Storage.CreateFile(ReadVariableInMem(R0));
                    break;
                case 0x6202: // delete file
                    Storage.DeleteFile(ReadVariableInMem(R0));
                    break;
                case 0x6203: // create directory
                    Storage.CreateDirectory(ReadVariableInMem(R0));
                    break;
                case 0x6204: // delete empty directory
                    Storage.DeleteEmptyDirectory(ReadVariableInMem(R0));
                    break;
                case 0x6205: // delete empty directory
                    Storage.DeleteFullDirectory(ReadVariableInMem(R0));
                    break;
				case 0x6206: // change current directory
					Storage.ChangeCD(ReadVariableInMem(R0));
					break;
                case 0x6210: // directory file count
                    StoreInMemory(R1, Storage.GetDirectoryFileCount(ReadVariableInMem(R0)).ToString());
					break;
				case 0x6211: // directory size
                    StoreInMemory(R1, Storage.GetDirectorySize(ReadVariableInMem(R0)).ToString());
                    break;
                case 0x6212: // find file
                    StoreInMemory(R1, Storage.FindFile(ReadVariableInMem(R0)).ToString());
                    break;
				case 0x6213: // file size
					Storage.MoveFile(ReadVariableInMem(R0), ReadVariableInMem(R1));
                    break;
				case 0x6220: // open file
					FileManipulator.OpenFile(ReadVariableInMem(R0));
					break;
                case 0x6221: // write to file
                    FileManipulator.Write(ReadVariableInMem(R0));
                    break;
                case 0x6222: // append to file
                    FileManipulator.Append(ReadVariableInMem(R0));
                    break;
				case 0x6223: // load into memory
					FileManipulator.LoadIntoMemory(R0, R1);
					break;
				case 0x622F: // close file
					FileManipulator.CloseFile();
					break;
            }
		}

		public static string ReadVariableInMem(short location)
		{
			// convert, bit for bit, the short to a ushort
			ushort l = (ushort)location;
			string variable = "";
			while (FauxSystem.Memory[l] != 0x00)
			{
				variable += (char)FauxSystem.Memory[l];
				l++;
			}
			return variable;
		}

		public static void StoreInMemory(short location, string data)
		{
            // convert, bit for bit, the short to a ushort
            ushort l = (ushort)location;
			for(int i = l; i < data.Length; i++)
				FauxSystem.Memory[i] = (byte)data[i - l];
        }
	}
}