namespace FauxperatingSystem
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "FauxperatingSystem";
            // args: 
            // -compileasm:[file]    |   Compile a .asm file to bytecode
            // -compilefl:[file]    |   Compile a .fl file to bytecode
            Storage.Init();
            ASMAnalyzer.LoadAnalyzers("analyzers.fana");
            ASMCompiler.Compile("testasm.asm", "output.tokens", "output.faux");
            BytecodeParser.LoadProgram("output.faux");
            while(FauxSystem.PC < FauxSystem.Memory.Length)
            {
                BytecodeParser.ParseNextBytecode();
            }
            Console.Title = "FauxperatingSystem | Done, press any key";
            Console.ReadKey();
        }
    }
}