using System;
using System.Text.RegularExpressions;

namespace FauxperatingSystem
{
    public enum TokenType { Data, Instruction, Newline, Register, Seperator, String, Int, Hex, SectionStart, VarDefine, DefineBytes }
    public class Token
    {
        public static readonly string[] INSTRUCTIONS = { "J", "JR", "JZ", "JNZ", "NOP", "MUL", "DIV", "ADD", "SUB", "AND", "OR", "XOR", "XNOR", "NOT", "SHR", "SHL", "SEQ", "SGT", "SLT", "INT", "CALL", "RET", "MOV", "MOVV", "INC", "DEC" };
        public TokenType Type;
        public string value;

        public Token(TokenType type, string value="")
        {
            Type = type;
            this.value = value;
        }

        public static Token ParseToken(string value)
        {
            value = value.TrimStart();
            if (INSTRUCTIONS.ToList().Exists(x=>x.ToLower() == value.ToLower()))
                return new Token(TokenType.Instruction, value);

            if (value == ASMLexer.DATA_SECT || value == ASMLexer.TEXT_SECT) 
                return new Token(TokenType.SectionStart, value);

            if (value.ToLower() == ASMLexer.DEFINE_BYTES)
                return new Token(TokenType.DefineBytes);

            if (int.TryParse(value, out int num))
                return new Token(TokenType.Int, value);

            if (value.ToLower().StartsWith("r") && value.Length == 2 && int.TryParse(value[1].ToString(), out int rid))
                return new Token(TokenType.Register, rid.ToString());

            Regex isHexReg = new Regex("0[xX][0-9a-fA-F]+");
            if(isHexReg.IsMatch(value))
                return new Token(TokenType.Hex, Convert.ToInt32(value, 16).ToString());

            return new Token(TokenType.Data, value);
        }

        public static implicit operator TokenType(Token t)
        {
            return t.Type;
        }
    }
}