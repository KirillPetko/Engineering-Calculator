using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Engineering_Calculator
{
    internal static class ExpressionValidator
    {
        static ExpressionValidator() { }

        //parses an expression string in order to check its validity
        public static bool VerifyExpression(string expression, string _posNumSequence, string _oneSignOpr, string _textOpr)
        {
            if (expression == String.Empty) return false;

            Regex number = new Regex(_posNumSequence);
            Regex signOperation = new Regex(_oneSignOpr);
            Regex textOperation = new Regex(_textOpr);

            string word = "";
            int wordType = 0;   //0 - not assigned 1 - number 2 - sign 3 - text operation 4 - bracket "(.." 5 - "..)"
            int charType = 0;   //0 - not assigned 1 - digit 2 - sign 3 - letter 4 - parentesis '(' 5 - ')'
            char c = expression[0];
            int openedBracketsCount = 0;
            int closedBracketsCount = 0;

            if (!Char.IsLetter(c) && !Char.IsDigit(c) && c != '-' && c != '(')
                return false;
            for (int i = 0; i < expression.Length; i++)
            {
                c = expression[i];
                if (c == '(') openedBracketsCount++;
                if (c == ')') closedBracketsCount++;

                //word is not yet assigned 
                if (wordType == 0)
                {
                    if (Char.IsDigit(c)) wordType = 1;
                    else if (c == '-') wordType = 2;
                    else if (Char.IsLetter(c)) wordType = 3;
                    else if (c == '(') wordType = 4;
                    else return false;

                    word += c;
                    if (i == expression.Length - 1)
                    {
                        if (c == '(')
                            return false;
                        else if (Char.IsLetter(c))
                            return false;
                        //end of expression reached - it must be a single number
                        else return true;
                    }
                    else
                        i++;
                    c = expression[i];
                    if (c == '(') openedBracketsCount++;
                    if (c == ')') closedBracketsCount++;
                }
                if (Char.IsDigit(c) || c == '.') charType = 1;
                else if (c == '*' || c == '/' || c == '+' || c == '-' || c == '^') charType = 2;
                else if (Char.IsLetter(c)) charType = 3;
                else if (c == '(') charType = 4;
                else if (c == ')') charType = 5;
                else return false;


                switch (wordType, charType)
                {
                    case (1, 1):
                        word += c;
                        break;
                    case (1, 2):                   //met operation - check word
                        if (!number.IsMatch(word) || countChar(word, '.') > 1 || word[0] == '.')
                            return false;
                        wordType = 2;              //change word type to new one
                        word = "";                 //clear word
                        word += c;                 //add 1st letter to the new word
                        break;
                    case (1, 3):
                        if (!number.IsMatch(word))
                            return false;
                        wordType = 3;
                        word = "";
                        word += c;
                        break;
                    case (1, 5):
                        if (!number.IsMatch(word) || countChar(word, '.') > 1 || word[0] == '.')
                            return false;
                        wordType = 5;
                        word = "";
                        word += c;
                        break;
                    case (2, 1):
                        if (!signOperation.IsMatch(word))
                            return false;
                        wordType = 1;
                        word = "";
                        word += c;
                        break;
                    case (2, 2): return false;      //this case means sign contents more than 1 letter
                    case (2, 3):
                        if (!signOperation.IsMatch(word))
                            return false;
                        wordType = 3;
                        word = "";
                        word += c;
                        break;
                    case (2, 4):
                        wordType = 4;
                        word = "";
                        word += c;
                        break;
                    case (2, 5): return false;      //sign before ')' -> Invalid expression
                    case (3, 3):
                        if (textOperation.IsMatch(word))
                            return false;           //operation already exists in word
                        word += c;
                        break;
                    case (3, 1): return false;      //after text opreation only opened bracket (char type 4)
                    case (3, 2): return false;
                    case (3, 4):
                        if(!textOperation.IsMatch(word))
                            return false;
                        wordType = 4;
                        word = "";
                        word += c;
                        break;
                    case (4, 1):
                        wordType = 1;
                        word = "";
                        word += c;
                        break;
                    case (4, 2):
                        if (c != '-') return false; //any sign after '(' exept '-' -> Invalid expression
                        break;
                    case (4, 3):                    //no need to check word type for parenteses
                        wordType = 3;
                        word = "";
                        word += c;
                        break;
                    case (4, 4): break;             //"((" possible sequence -> break to avoid false return
                    case (4, 5): return false;      //"()" - empty parentesises -> Invalid expression
                    case (5, 1): return false;      //number after ')' -> Invalid expression
                    case (5, 2):
                        wordType = 2;
                        word = "";
                        word += c;
                        break;
                    case (5, 3): return false;      //text after ) -> Invalid expression
                    case (5, 4): return false;      //")(" -> Invalid expression
                    case (5, 5): break;             //"))" possible sequence -> break to avoid false return
                    default: return false;
                }
            }
            switch (wordType)                       //last word check (valid cases have a break operator)
            {
                case 0: return false;
                case 1:
                    if (!number.IsMatch(word) || countChar(word, '.') > 1 || word[0] == '.')
                        return false;
                    break;
                case 2: return false;
                case 3: return false;
                case 4: return false;
                case 5: break;
                default: return false;
            }
            if (openedBracketsCount != closedBracketsCount) return false;
            return true;
        }

        public static int countChar(string source, char toFind)
        {
            int count = 0;
            foreach (char c in source)
                if (c == toFind)
                    count++;
            return count;
        }
    }
}
