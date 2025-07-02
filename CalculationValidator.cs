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
        //by comparing next symbol entity with previous token entity
        //entities have value and type of value as fields
        public static bool VerifyInput(string expression, string _posNumSequence, string _textOpr)
        {
            if (String.IsNullOrEmpty(expression)) return false;

            Regex numberRegex = new Regex(_posNumSequence);
            Regex textOpRegex = new Regex(_textOpr);

            int openedBrackets = 0;
            int closedBrackets = 0;

            TokenEntity tokenEntity = new TokenEntity(String.Empty, TokenType.None);

            for (int i = 0; i < expression.Length; i++)
            {
                CharEntity charEntity = new CharEntity(expression[i], GetCharType(expression[i]));
                if (charEntity.type == CharType.Invalid)
                    return false;
                if (charEntity.type == CharType.OpenParen) openedBrackets++;
                if (charEntity.type == CharType.CloseParen) closedBrackets++;

                if (tokenEntity.type == TokenType.None) //not yet assigned
                {
                    if (charEntity.type == CharType.Digit) tokenEntity.type = TokenType.Number;
                    else if (charEntity.type == CharType.Sign && charEntity.c == '-') tokenEntity.type = TokenType.Sign; //first sign is only minus
                    else if (charEntity.type == CharType.Letter) tokenEntity.type = TokenType.TextOperation;
                    else if (charEntity.type == CharType.OpenParen) tokenEntity.type = TokenType.OpenParen;
                    else return false;

                    tokenEntity.token += charEntity.c;
                    if (i == expression.Length - 1)
                        return IsValidFinalToken(tokenEntity, numberRegex, textOpRegex);
                    continue;
                }

                tokenEntity = ValidationTokenCharSequence(tokenEntity, charEntity, numberRegex, textOpRegex);
                if (!tokenEntity.isValidNextChar)
                    return false;   
            }
            if (!IsValidFinalToken(tokenEntity, numberRegex, textOpRegex))
                return false;

            return openedBrackets == closedBrackets;
        }

        //compares assigned entities of token and next char
        private static TokenEntity ValidationTokenCharSequence(TokenEntity tokenEntity, CharEntity charEntity, Regex numberRegex, Regex textOpRegex)
        {
            switch (tokenEntity.type, charEntity.type)
            {
                case (TokenType.Number, CharType.Digit):
                    tokenEntity = ModifyTokenEntity(tokenEntity, charEntity, false);
                    break;
                case (TokenType.Number, CharType.Sign):
                    if (!IsValidNumber(tokenEntity.token, numberRegex))       //met operation - check token   
                    {
                        tokenEntity.isValidNextChar = false;
                        break;
                    }
                    tokenEntity = ModifyTokenEntity(tokenEntity, charEntity, true);                                                                              
                    break;                                                    
                case (TokenType.Number, CharType.CloseParen):
                    if (!IsValidNumber(tokenEntity.token, numberRegex))
                    {
                        tokenEntity.isValidNextChar = false;
                        break;
                    }
                    tokenEntity = ModifyTokenEntity(tokenEntity, charEntity, true);
                    break;
                case (TokenType.Sign, CharType.Digit):
                    tokenEntity = ModifyTokenEntity(tokenEntity, charEntity, true);
                    break;
                case (TokenType.Sign, CharType.Letter):
                    tokenEntity = ModifyTokenEntity(tokenEntity, charEntity, true);
                    break;
                case (TokenType.Sign, CharType.OpenParen):
                    tokenEntity = ModifyTokenEntity(tokenEntity, charEntity, true); ;
                    break;
                case (TokenType.TextOperation, CharType.Letter):
                    if (textOpRegex.IsMatch(tokenEntity.token))
                    {
                        tokenEntity.isValidNextChar = false;
                        break;
                    }
                    tokenEntity = ModifyTokenEntity(tokenEntity, charEntity, false); 
                    break;
                case (TokenType.TextOperation, CharType.OpenParen):
                    if (!textOpRegex.IsMatch(tokenEntity.token))
                    {
                        tokenEntity.isValidNextChar = false;
                        break;
                    }
                    tokenEntity = ModifyTokenEntity(tokenEntity, charEntity, true);
                    break;
                case (TokenType.OpenParen, CharType.Digit):
                    tokenEntity = ModifyTokenEntity(tokenEntity, charEntity, true);
                    break;
                case (TokenType.OpenParen, CharType.Sign):
                    if (charEntity.c != '-')
                    {
                        tokenEntity.isValidNextChar = false;
                        break;
                    }
                    tokenEntity = ModifyTokenEntity(tokenEntity, charEntity, true);
                    break;
                case (TokenType.OpenParen, CharType.OpenParen):
                    tokenEntity.isValidNextChar = true;
                    break;
                case (TokenType.CloseParen, CharType.Sign):
                    tokenEntity = ModifyTokenEntity(tokenEntity, charEntity, true);
                    break;
                case (TokenType.CloseParen, CharType.CloseParen):
                    tokenEntity.isValidNextChar = true;
                    break;
                default:
                    break;
            }
            return tokenEntity;
        }

        private static TokenEntity ModifyTokenEntity(TokenEntity tokenEntity, CharEntity charEntity, bool needReset)
        {
            if (needReset) // reinitiallize tokenEntity with new type and value
            {
                switch (charEntity.type)
                {
                    case CharType.Digit:
                        tokenEntity.type = TokenType.Number;
                        break;
                    case CharType.Sign:
                        tokenEntity.type = TokenType.Sign;
                        break;
                    case CharType.Letter:
                        tokenEntity.type = TokenType.TextOperation;
                        break;
                    case CharType.OpenParen:
                        tokenEntity.type = TokenType.OpenParen;
                        break;
                    case CharType.CloseParen:
                        tokenEntity.type = TokenType.CloseParen;
                        break;
                }
                tokenEntity.token = charEntity.c.ToString();
                tokenEntity.isValidNextChar = true;
            }
            else  // add char to tokenEntity.token
            {
                tokenEntity.token += charEntity.c;
                tokenEntity.isValidNextChar = true;
            }
            return tokenEntity;
        }

        private static bool IsValidFinalToken(TokenEntity tokenEntity, Regex numberRegex, Regex textOpRegex)
        {
            switch (tokenEntity.type)  
            {
                case TokenType.Number:
                    return IsValidNumber(tokenEntity.token, numberRegex);
                case TokenType.CloseParen: return true;
                default: return false;
            }
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

        private struct TokenEntity
        {
            public string token;
            public TokenType type;
            public bool isValidNextChar;
            public TokenEntity(string _token, TokenType _type)
            { 
                token = _token;
                type = _type;
                isValidNextChar = false;
            }
        }
        private struct CharEntity
        { 
            public char c;
            public CharType type;
            public CharEntity(char _c, CharType _type)
            { 
                c = _c;
                type = _type;
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
