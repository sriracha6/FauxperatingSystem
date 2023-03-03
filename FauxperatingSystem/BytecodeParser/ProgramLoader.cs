using System;

namespace FauxperatingSystem
{
	public static partial class BytecodeParser
	{
		public static void LoadProgram(string file)
		{
			byte[] bytes = File.ReadAllBytes(file);
			FauxSystem.Memory = bytes;
			short textLoc = BitConverter.ToInt16(bytes.Take(2).ToArray());

			FauxSystem.PC = (ushort)textLoc;

			if (textLoc != 2) // there is a 1st variable. this is dumb as hell. and it makes the 2nd loop iterate over area
				FauxSystem.Variables[0] = 2;// kinda needs refactoring
			else return; 

			int varCount = 1;
			for(int i = 2; i < textLoc; i++)
			{
				if (bytes[i] == 0x00)
				{
					FauxSystem.Variables[varCount] = (ushort)i;
					varCount++;
				}
			}
			FauxSystem.Variables[varCount - 1] = 0; // this one doesnt count. god this is dumb
        }
	}
}