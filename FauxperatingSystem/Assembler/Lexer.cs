using System;

namespace FauxperatingSystem
{
    public static class ASMLexer
    {
        public const char DELIMETER = ' ';
        public const char SEPERATOR = ',';
        public const char VAR_DEFINE = ':';

        public const char COMMENT_START = ';';
        public const char STRING_DELIM = '"';

        public const string DATA_SECT = ".data";
        public const string TEXT_SECT = ".text";

        public const string DEFINE_BYTES = "db";


        /// <summary>
        /// This is held together with duct tape. I have no idea how it works, and I'm too afraid to ask. It worked first try.
        /// </summary>
        public static List<Token> Lex(string fileIn) // TODO: string escape sequences
        {
            List<Token> tokens = new List<Token>();
            string[] lines = File.ReadAllLines(fileIn);

            for(int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
                string curToken = "";
                bool stringMode = false;
                for(int j = 0; j < line.Length; j++)
                {
                    char x = line[j];
                    if(!stringMode && /*!string.IsNullOrEmpty(curToken) && */(x == DELIMETER || x == SEPERATOR || x == VAR_DEFINE))
                    {
                        tokens.Add(Token.ParseToken(curToken));
                        if(x==SEPERATOR)
                            tokens.Add(new Token(TokenType.Seperator));
                        if (x == VAR_DEFINE)
                            tokens.Add(new Token(TokenType.VarDefine));
                        curToken = "";
                        continue;
                    }
                    if (x == STRING_DELIM)
                    {
                        if(!stringMode)
                        {
                            stringMode = true;
                            continue;
                        }
                        tokens.Add(new Token(TokenType.String, curToken));
                        curToken = "";
                        stringMode = false;
                        continue;
                    }
                    if (!stringMode && x == COMMENT_START)
                        break;
                    curToken += x;

                }
                if (!string.IsNullOrEmpty(curToken))
                    tokens.Add(Token.ParseToken(curToken));
                tokens.Add(new Token(TokenType.Newline));
            }

            for (int i = 0; i < tokens.Count; i++)
                if (tokens[i].Type == TokenType.Data && string.IsNullOrEmpty(tokens[i].value))
                    tokens.RemoveAt(i);

            return tokens;
        }
    }
}