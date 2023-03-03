using System;

namespace FauxperatingSystem
{
    public static class ASMCompiler
    {
        public static void Compile(string fileASM, string tokenFileOutput, string fileOutput)
        {
            List<Token> tokens = ASMLexer.Lex(fileASM);
            string tokenstring = "";
            for(int i = 0; i < tokens.Count; i++)
                tokenstring += tokens[i].Type + " " + tokens[i].value + "\n";
            File.WriteAllText(tokenFileOutput, tokenstring);
            ASMParseException exception = ASMAnalyzer.Analyze(tokens);
            if (exception != null)
                Console.WriteLine($"{exception.Exception}Exception | Line: {exception.LineNumber} \n {exception.Description}");

            Assembler.GetAssemblyInformation(tokens);
            byte[] bytes = Assembler.Assemble();
            File.WriteAllBytes(fileOutput, bytes);
        }
    }
}