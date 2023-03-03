using System;

namespace FauxperatingSystem
{
    public static class ASMAnalyzer
    {
        static List<Token[]> Analyzers;

        public static List<Token[]> LoadAnalyzers(string filepath)
        {
            string[] lines = File.ReadAllLines(filepath);

            List<Token[]> list = new List<Token[]>();

            for(int i = 0; i < lines.Length; i++)
            {
                string[] tts = lines[i].Split(' ');
                Token[] tokens = new Token[tts.Length];
                for(int j = 0; j < tts.Length; j++)
                {
                    if (!Token.INSTRUCTIONS.Contains(tts[j]))
                        tokens[j] = new Token((TokenType)Enum.Parse(typeof(TokenType), tts[j]));
                    else
                        tokens[j] = new Token(TokenType.Instruction, tts[j]);
                }

                list.Add(tokens);
            }

            Analyzers = list;
            return list;
        }
        /// <summary>
        /// TODO: Fix line number
        /// </summary>
        /// <returns>Null if there are no errors</returns>
        public static ASMParseException Analyze(List<Token> tokens)
        {
            // only check data value if the type is Instruction
            if (tokens.Where(x => x.Type == TokenType.SectionStart).Count() > 2)
                return new ASMParseException(ASMException.InvalidSection, 0, $"Too many sections. Max is 2: .data and .text");

            int curLine = 1;
            int section = 0; // Data: 1   Text: 2
            List<Token> curLineTokens = new List<Token>();
            List<string> definedVarNames = new List<string>();
            bool movvFlag = false;
            int movvLocation = 0;
            bool dataFlag = false;
            for (int i = 0; i < tokens.Count; i++)
            {
                if (tokens[i].Type == TokenType.Newline)
                {
                    if (curLineTokens.Count == 0) continue;
                    List<TokenType> cltt = ToTokenType(curLineTokens);
                    Token[] matchingAnalyzer = null;

                    // WHY THE HELL DO I HAVE TO DO THIS??????? AAAAAAA 
                    // it doesn't work if i use linq or even compare entire lists. wth
                    for (int k = 0; k < Analyzers.Count; k++)
                    {
                        var t = ToTokenType(Analyzers[k]);
                        int tot = 0;
                        for (int l = 0; l < t.Count; l++)
                            if (l < cltt.Count && l < t.Count && t[l] == cltt[l])
                            {
                                tot++;
                                if (Analyzers[k][l].Type == TokenType.Instruction && Analyzers[k][l].value == curLineTokens[l].value)
                                    tot--;
                            }
                        
                        if (tot == Analyzers[k].Length)
                        matchingAnalyzer = Analyzers[k];
                    }

                    if (matchingAnalyzer == null)
                        return new ASMParseException(ASMException.SyntaxError, curLine, "Invalid instruction");

                    curLine++;
                    curLineTokens.Clear();
                    continue;
                }

                if (tokens[i].Type == TokenType.Register && (int.Parse(tokens[i].value) > 7 || int.Parse(tokens[i].value) < 0))
                    return new ASMParseException(ASMException.InvalidRegister, curLine, $"{tokens[i].value} is not at least 0 and at most 7");

                if (tokens[i].Type == TokenType.Int)
                {
                    int k = int.Parse(tokens[i].value);
                    if(k > 1024 || k < -1024)
                        return new ASMParseException(ASMException.InvalidInt, curLine, $"{tokens[i].value} is too big/small to be an I value");
                }

                if(dataFlag)
                {
                    definedVarNames.Add(tokens[i].value);
                    dataFlag = false;
                    if (section == 2)
                        return new ASMParseException(ASMException.WrongSection, curLine, $"Cannot define variable in section {section}");
                }

                if (tokens[i].Type == TokenType.Data && curLineTokens.Count == 0)
                    dataFlag = true;

                if (definedVarNames.Count > 256) // TODO: this can be fixed and it has to be
                    return new ASMParseException(ASMException.MaxVarExceeded, curLine, "Maximum variable count of 256 exceeded. I know, it's stupid.");

                if (tokens[i].Type == TokenType.Instruction && tokens[i].value == "MOVV")
                {
                    movvFlag = true;
                    movvLocation = i;
                }

                if (movvFlag && i - movvLocation == 3)
                {
                    if (!definedVarNames.Contains(tokens[i].value))
                        return new ASMParseException(ASMException.UndefinedVariable, curLine, $"Undefined variable: {tokens[i].value}");
                    else if (i - movvLocation != 3)
                        return new ASMParseException(ASMException.SyntaxError, curLine, $"Invalid instruction");
                }
                else movvFlag = false;

                if (tokens[i].Type == TokenType.SectionStart)
                    section++;

                curLineTokens.Add(tokens[i]);
            }
            return null;
        }

        public static List<TokenType> ToTokenType(IEnumerable<Token> tokens)
        {
            List<TokenType> tts = new List<TokenType>();
            for (int i = 0; i < tokens.Count(); i++)
                tts.Add((TokenType)tokens.ElementAt(i));
            return tts;
        }
    }
}