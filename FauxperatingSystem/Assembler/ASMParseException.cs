using System;

namespace FauxperatingSystem
{
    public enum ASMException { Exception, SyntaxError, InvalidRegister, WrongSection, InvalidInt, MaxVarExceeded, UndefinedVariable, InvalidSection }

    public class ASMParseException
    {
        public ASMException Exception;
        public int LineNumber;
        public string Description;

        public ASMParseException(ASMException exception, int lineNumber, string description)
        {
            Exception = exception;
            LineNumber = lineNumber;
            Description = description;
        }
    }
}