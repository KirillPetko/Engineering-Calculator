using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Engineering_Calculator
{
    internal static class CalculationValidator
    {
        static CalculationValidator() { }

        private const int MAX_RECURSIVE_CALLS = 250;

        //parses an expression string in order to check its validity,
        //by comparing next symbol type with previous token type
        public static bool VerifyInput(string expression, string _posNumSequence, string _textOpr)
        {
            if (String.IsNullOrEmpty(expression)) return false;

            Regex numberRegex = new Regex(_posNumSequence);
            Regex textOpRegex = new Regex(_textOpr);

            int openedBrackets = 0;
            int closedBrackets = 0;

            string token = String.Empty;
            TokenType tokenType = TokenType.None;

            for (int i = 0; i < expression.Length; i++)
            {
                char c = expression[i];
                CharType charType = GetCharType(c);
                if (charType == CharType.Invalid)
                    return false;
                if (charType == CharType.OpenParen) openedBrackets++;
                if (charType == CharType.CloseParen) closedBrackets++;
                if (tokenType == TokenType.None) //token type is not assigned (yet)
                {
                    if (charType == CharType.Digit) tokenType = TokenType.Number;
                    else if (charType == CharType.Sign && c == '-') tokenType = TokenType.Sign; //first sign is only minus
                    else if (charType == CharType.Letter) tokenType = TokenType.TextOperation;
                    else if (charType == CharType.OpenParen) tokenType = TokenType.OpenParen;
                    else return false;

                    token += c;
                    if (i == expression.Length - 1)
                        return IsValidFinalToken(token, tokenType, numberRegex, textOpRegex);
                    continue;
                }

                switch (tokenType, charType)
                {
                    case (TokenType.Number, CharType.Digit):
                        token += c;
                        break;
                    case (TokenType.Number, CharType.Sign):                  
                        if (!IsValidNumber(token, numberRegex)) return false; //met operation - check token
                        tokenType = TokenType.Sign;                           //change token type to new one
                        token = c.ToString();                                 //reinitialize token
                        break;                                               //same algorithm for most valid cases
                    case (TokenType.Number, CharType.CloseParen):
                        if (!IsValidNumber(token, numberRegex)) return false;
                        tokenType = TokenType.CloseParen;
                        token = c.ToString();
                        break;
                    case (TokenType.Sign, CharType.Digit):
                        tokenType = TokenType.Number;
                        token = c.ToString();
                        break;
                    case (TokenType.Sign, CharType.Letter):
                        tokenType = TokenType.TextOperation;
                        token = c.ToString();
                        break;
                    case (TokenType.Sign, CharType.OpenParen):
                        tokenType = TokenType.OpenParen;
                        token = c.ToString();
                        break;
                    case (TokenType.TextOperation, CharType.Letter):
                        if (textOpRegex.IsMatch(token)) return false;
                        token += c;
                        break;
                    case (TokenType.TextOperation, CharType.OpenParen):
                        if (!textOpRegex.IsMatch(token)) return false;
                        tokenType = TokenType.OpenParen;
                        token = c.ToString();
                        break;
                    case (TokenType.OpenParen, CharType.Digit):
                        tokenType = TokenType.Number;
                        token = c.ToString();
                        break;
                    case (TokenType.OpenParen, CharType.Sign):
                        if (c != '-') return false;
                        tokenType = TokenType.Sign;
                        token = c.ToString();
                        break;
                    case (TokenType.OpenParen, CharType.OpenParen):
                        break;
                    case (TokenType.CloseParen, CharType.Sign):
                        tokenType = TokenType.Sign;
                        token = c.ToString();
                        break;
                    case (TokenType.CloseParen, CharType.CloseParen):
                        break;
                    default:
                        return false;
                }
            }
            if (!IsValidFinalToken(token, tokenType, numberRegex, textOpRegex))
                return false;

            return openedBrackets == closedBrackets;
        }

        private static CharType GetCharType(char c)
        {
            if (Char.IsDigit(c) || c == '.') return CharType.Digit;
            if ("+-*/^".Contains(c)) return CharType.Sign;
            if (Char.IsLetter(c)) return CharType.Letter;
            if (c == '(') return CharType.OpenParen;
            if (c == ')') return CharType.CloseParen;
            else return CharType.Invalid;
        }

        //false if doesen't much regex, starts with or has more than one period
        private static bool IsValidNumber(string token, Regex numberRegex)
        {
            return numberRegex.IsMatch(token) && token.Count(ch => ch == '.') <= 1 && token[0] != '.';
        }

        private static bool IsValidFinalToken(string lastToken, TokenType type, Regex numberRegex, Regex textOpRegex)
        {
            switch (type)  
            {
                case TokenType.Number:
                    return IsValidNumber(lastToken, numberRegex);
                case TokenType.CloseParen: return true;
                default: return false;
            }
        }
        private enum TokenType
        {
            None, Number, Sign, TextOperation, OpenParen, CloseParen 
        }
        private enum CharType
        {
            Invalid, Digit, Sign, Letter, OpenParen, CloseParen
        }

        //checks an expression string on validity (called during callculation)
        //and amount of recursive calls, forms and throws propper exceptions
        public static void CalculationCheck(string expression, string input, string oneSignOpr, ref int currentRecursiveCalls)
        {
            Exception e;
            Regex opr = new Regex(oneSignOpr);
            MatchCollection oprs = opr.Matches(expression);
            bool alotOfSci = expression.Count(ch => ch == 'E') > 1,
                 sciOperation = expression.Count(ch => ch == 'E') == 1 && oprs.Count > 1;

            if (currentRecursiveCalls == MAX_RECURSIVE_CALLS)
            {
                currentRecursiveCalls = 0;
                e = ErrorFactory.CreateCalculationException("Failed calculating expression", "StackOverflowException", expression, input);
                throw e;
            }
            else if (expression.Contains("NaN"))
            {
                e = ErrorFactory.CreateCalculationException("NaN value", "ArithmeticException", expression, input);
                throw e;
            }
            else if (expression.Contains("Infinity"))
            {
                e = ErrorFactory.CreateCalculationException("Infinity", "ArithmeticException", expression, input);
                throw e;
            }
            else if (alotOfSci || sciOperation)
            {
                e = ErrorFactory.CreateCalculationException("Wrong operands format", "ArithmeticException", expression, input);
                throw e;
            }
        }
    }
}
