using System;

namespace FauxperatingSystem
{
	public static class FauxSystem
	{
		public static short[] Register = new short[8];
		public static Stack<ushort> Stack = new Stack<ushort>(8);
		public static ushort PC;
		public static short SP; // stack pointer
		public static byte[] Memory = new byte[65536];
		public static ushort[] Variables = new ushort[256]; // locations in memory

		public static string OpenFileLOCAL;
		public static string OpenFileName;

		public static void Halt(string message)
		{
			Console.Clear();
			Console.WriteLine(message);
			Console.WriteLine("Strike any key to exit");
			Console.ReadKey();
			Environment.Exit(0);
		}
	}
}