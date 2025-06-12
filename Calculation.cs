using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace Engineering_Calculator
{
    //Class responsible for making calculation from input string as well as validating it
    internal class Calculation
    {
        public Calculation() 
        {
        
        }
        public Calculation(string _input)
        {
            input = _input;
            isValid = Verify(_input);
            if (isValid) result = Calculate(input);
            
        }

        private string input;
        private double result;
        private bool isValid;

        public string Input 
        {
            get 
            {
                return input;
            }
            set
            {
                if (value == null) throw new NotImplementedException("parameter has to be => 0 to be set![Input]");
                else input = value;
                    
            }
        }
        public double Result 
        {
            get 
            {
                return result;
            }
            set
            { 
                result = value;
            }
        }
        public bool IsValid
        {
            get
            { 
                return isValid;
            }
            set
            { 
                isValid = value;
            }
        }

        //function to parse an input string in order to check its validity
        static bool Verify(string expression)
        {
            if (expression == String.Empty) return false;

            Regex number = new Regex("\\d+[\\.?\\d]*");
            Regex signOperation = new Regex("\\*|\\/|\\+|\\-|\\^");
            Regex textOperation = new Regex("sin|cos|tan|asin|acos|atan|ln|log|sqrt");

            string word = "";
            int wordType = 0;                          //0 - not assigned 1 - number 2 - sign 3 - text operation 4 - parentesis "(.." 5 - "..)"
            int charType = 0;                          //0 - not assigned 1 - digit  2 - sign 3 - letter 4 - parentesis '(' 5 - ')'
            char c = expression[0];
            int openedBracketsCount = 0;
            int closedBracketsCount = 0;

            if (!Char.IsLetter(c) && !Char.IsDigit(c) && c != '-') 
                return false;
            for (int i = 0; i < expression.Length; i++)
            {
                c = expression[i];
                if (c == '(') openedBracketsCount++;
                if (c == ')') closedBracketsCount++;

                if (wordType == 0)                      //word is not yet assigned 
                {
                    if (Char.IsDigit(c) || c == '.') wordType = 1;
                    else if (c == '*' || c == '/' || c == '+' || c == '-' || c == '^') wordType = 2;
                    else if (Char.IsLetter(c)) wordType = 3;
                    else if (c == '(') wordType = 4;
                    else if (c == ')') return false;
                    else return false;

                    word += c;
                    i++;
                    c = expression[i];
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
                    case (1, 2):
                        if (!number.IsMatch(word))        //met operation - check word
                            return false;
                        wordType = 2;                    //change word type to new one
                        word = "";                       //clear word
                        word += c;                       //add 1st letter to the new word
                        break;
                    case (1, 3):
                        if (!number.IsMatch(word))
                            return false;
                        wordType = 3;
                        word = "";
                        word += c;
                        break;
                    case (1, 5):
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
                    case (2, 2): return false;      //sign can only be one -> this case means it contents more than 1 letter(previous assign of word to sign type) -> Invalid expression
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
                        word += c;
                        break;
                    case (3, 1): return false;      //after text opreation is only opened parentesis '(' -> char type 4
                    case (3, 2): return false;
                    case (3, 4):
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
                        if (c != '-') return false; // any sign after '(' exept '-' -> Invalid expression
                        break;
                    case (4, 3):                    //no need to check word type for parentesises
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
            switch (wordType)                       //last word check switch (what can be last word has a break- operator)
            {
                case 0: return false;
                case 1:
                    if (!number.IsMatch(word))
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

        //function which takes a string in form of expression, then open brackets, taking inner string as argument to genereted recurrent entrance of itself
        //then it divides expression in sequences with priorities of operations while callcucating them sequentially with the help of overloaded examples of itself
        //it also adds 0 in the beginning not to break further calculation process as well as checking validity of gotten result before returning it, otherwise this function throws
        //an exeption or continues by recurrent enter untill expression becomes a double-pasable string, then it returns a result of that pasing
        static double Calculate(string expression)
        {
            double result;
            List<int> openedBracketIndexes = new List<int>();
            for (int i = 0; i < expression.Length; i++)                                                                                     //cycle to iterate over characters in order to
            {                                                                                                                               //open '(' ')' each corresponding bracket pair

                string subexpressionWithBrackets;
                string subexpression;
                double subresult;
                if (i < expression.Length - 1)                                                                                              //[i+1] shold not be out of range of string
                {
                    if (expression[i] == '(')
                        openedBracketIndexes.Add(i);
                    else if (expression[i] == ')' && expression[i + 1] == '^')
                    {
                        subexpressionWithBrackets = expression.Substring(openedBracketIndexes.Last(), i + 1 - openedBracketIndexes.Last()); //[^1] means last item of the list index
                        subexpression = Regex.Replace(subexpressionWithBrackets, "\\(|\\)", "");                                            //Escapes a minimal set of characters(Regex.Escape)
                        Regex subexpBrackets = new Regex(Regex.Escape(subexpressionWithBrackets));                                          //(\, *, +, ?, |, {, [, (,), ^, $, ., #, .,  
                        i = openedBracketIndexes.Last();                                                                                    //space)by replacing them with their escape codes
                        subresult = Calculate(subexpression);                                                                               //Made for cases like (-2)^(-2), to 
                        if (subresult < 0)                                                                                                  //avoid sequences like 2-^2-. (*)
                            expression = subexpBrackets.Replace(expression, Convert.ToString(-1 * subresult) + '-', 1);                     //replacing first match of escaped subexpressionWithBrackets
                        else                                                                                                                //in expression  changing '-' position to after number
                            expression = expression.Replace(subexpressionWithBrackets, Convert.ToString(subresult));                        //(*) when there is no raise power operations
                                                                                                                                            //with negative arguments replacing all same
                        expression = expression.Replace(',', '.');                                                                          //expressions in brackets with single calculated                                                
                        openedBracketIndexes.RemoveAt(openedBracketIndexes.Count - 1);                                                      //result is acceptable 
                    }
                    else if (expression[i] == ')' && expression[i + 1] != '^')
                    {

                        subexpressionWithBrackets = expression.Substring(openedBracketIndexes.Last(), i + 1 - openedBracketIndexes.Last());
                        subexpression = Regex.Replace(subexpressionWithBrackets, "\\(|\\)", "");
                        i = openedBracketIndexes.Last();
                        expression = expression.Replace(subexpressionWithBrackets, Convert.ToString(Calculate(subexpression)));
                        expression = expression.Replace(',', '.');
                        openedBracketIndexes.RemoveAt(openedBracketIndexes.Count - 1);

                    }

                }
                else                                                                                                                //if '(' is met at the end of the expression string
                {
                    if (expression[i] == ')')
                    {
                        subexpressionWithBrackets = expression.Substring(openedBracketIndexes.Last(), i + 1 - openedBracketIndexes.Last());
                        subexpression = Regex.Replace(subexpressionWithBrackets, "\\(|\\)", "");
                        i = openedBracketIndexes.Last();
                        expression = expression.Replace(subexpressionWithBrackets, Convert.ToString(Calculate(subexpression)));
                        expression = expression.Replace(',', '.');
                        openedBracketIndexes.RemoveAt(openedBracketIndexes.Count - 1);
                    }
                }
            }

            if (expression[0] == '-') expression = "0" + expression;                                                                //0 is inserted to insure proper calculation of sequence
                                                                                                                                    // sequences of type:
            expression = Calculate("(asin|acos|atan)(\\-)?\\d+[\\.?\\d]*", expression, 4);                                          // asin-3 or acos90 (just a single operation)                                                                              
            expression = Calculate("(sin|cos|tan|ln|log|sqrt)(\\-)?\\d+[\\.?\\d]*", expression, 3);                                 // sin-3 or cos90   (just a single operation)
            expression = Calculate("\\d+[\\.?\\d]*\\-\\^\\-(\\d+[\\.?\\d]*\\-\\^\\-)*\\d+[\\.?\\d]*", expression, 2);               // 3-^-2-^-4...-^-0.3
            expression = Calculate("\\d+[\\.?\\d]*(\\^\\-|\\-\\^)(\\d+[\\.?\\d]*(\\^\\-|\\-\\^))*\\d+[\\.?\\d]*", expression, 1);   // 3^-2^-0.4...^-3 
            expression = Calculate("\\d+[\\.?\\d]*\\^(\\d+[\\.?\\d]*\\^)*\\d+[\\.?\\d]*", expression, 0);                           // 3^2^0.4...^3 
            expression = Calculate("\\d+[\\.?\\d]*(\\*\\-|\\/\\-)(\\d+[\\.?\\d]*(\\*\\-|\\/\\-))*\\d+[\\.?\\d]*", expression, 1);   // 7/-6*-0.03*-.../-123
            expression = Calculate("\\d+[\\.?\\d]*[\\*|\\/](\\d+[\\.?\\d]*[\\*|\\/])*\\d+[\\.?\\d]*", expression, 0);               // 7/6*0.03*.../123 

            expression = expression.Replace("++", "+");                                                                             //sequences of such type (3++3--2+-3...)
            expression = expression.Replace("--", "+");                                                                             //may be just replaced by ones with one sign
            expression = expression.Replace("+-", "-");                                                                             //instead of calculation
            expression = expression.Replace("-+", "-");

            if (expression[0] == '-') expression = "0" + expression;

            expression = Calculate("\\d+[\\.?\\d]*[\\+|\\-](\\d+[\\.?\\d]*[\\+|\\-])*\\d+[\\.?\\d]*", expression, 0);               // 3+2-3+8.3-...+9
                                                                                                                                    //regex defining fractional number with sign
            string number = "^(\\-)?\\d+[\\.?\\d]*([eE][-+]?[0-9]+)?$";                                                             //(^ and $ means only what is between them)
                                                                                                                                    //also defining nums like 2.5600000000000013E-06

            if (expression.Contains("NaN"))                                                                                         //if result is not defined (like acos(2))
                throw new Exception("NaN value");                                                                                   //it will contain "NaN" in expression
            else if (expression.Contains("Infinity"))                                                                               //if result is infinity (1/0)
                throw new Exception("Infinity");                                                                                    //it will contain "Infinity" in expression                                                             
            if (Regex.IsMatch(expression, number, RegexOptions.IgnoreCase))
                //result = Double.Parse(expression, CultureInfo.InvariantCulture);
                result = Double.Parse(expression);
            else
                result = Calculate(expression);
            return result;
        }
        //functiom which finds sequences in input string by specified pattern argument
        static string Calculate(string sequenceStr, string expression, int operationType)
        {
            Regex sequence = new Regex(sequenceStr);
            MatchCollection sequences = sequence.Matches(expression);
            foreach (Match s in sequences)
                expression = expression.Replace(s.Value, Calculate(s, operationType));
            return expression;
        }

        //function which calculates sequenses of same priority replacing them with fractional number represented in a string 
        static string Calculate(Match seqence, int operationType)
        {
            Regex number = new Regex("\\d+[\\.?\\d]*");                                        //regex for any double number without sign before it
            MatchCollection numbers;
            double result;
            string resultS;
            Regex operation;
            switch (operationType)
            {
                case 0:
                    operation = new Regex("\\*|\\/|\\+|\\-|\\^");
                    break;
                case 1:
                    operation = new Regex("(\\^\\-)|(\\-\\^)|(\\*\\-)|(\\/\\-)");
                    break;
                case 2:
                    operation = new Regex("\\-\\^\\-");
                    break;
                case 3:
                    operation = new Regex("sin|cos|tan|ln|log|sqrt");
                    number = new Regex("(\\-)?\\d+[\\.?\\d]*");                                    //argument of text function may be negative (sin-3 or sqrt-4 sequences as an example)
                    break;
                case 4:
                    operation = new Regex("asin|acos|atan");
                    number = new Regex("(\\-)?\\d+[\\.?\\d]*");
                    break;
                default:
                    throw new ArgumentException();
            }

            numbers = number.Matches(seqence.Value);

            if (operationType != 3 && operationType != 4)
            {
                MatchCollection operations = operation.Matches(seqence.Value);
                result = Double.Parse(numbers[0].Value, CultureInfo.InvariantCulture);              //result of each sequence of operations with same priority is formed by
                for (int i = 0; i < operations.Count; i++)                                          //performing this operation with previous result, forming new value to
                {                                                                                   //perform this operation again on the next step, untill sequence is not
                    double x = Double.Parse(numbers[i + 1].Value, CultureInfo.InvariantCulture);    //calculated completely, then result is returned to change the sequence
                    result = Calculate(result, x, operations[i].Value);                             //in expression
                }

            }                                                                                       //if operation is trigonometry one -> return result of that single operation
            else result = Calculate(Double.Parse(numbers[0].Value, CultureInfo.InvariantCulture), operation.Match(seqence.Value).Value);

            resultS = Convert.ToString(result, CultureInfo.InvariantCulture);
            resultS = resultS.Replace(',', '.');
            return resultS;

        }
        //function which performs mathematical operation between two fractional numbers
        static double Calculate(double x, double y, string operation)
        {
            switch (operation)
            {
                case "+":
                    return x + y;
                case "-":
                    return x - y;
                case "*":
                    return x * y;
                case "/":
                    return x / y;
                case "^":
                    return Math.Pow(x, y);
                case "*-":
                    return x * (-y);
                case "/-":
                    return x / (-y);
                case "^-":
                    return 1.0 / Math.Pow(x, y);
                case "-^":
                    return Math.Pow(-x, y);
                case "-^-":
                    return 1.0 / Math.Pow(-x, y);
                default:
                    throw new NotImplementedException();
            }
        }
        static double Calculate(double x, string operation)
        {
            switch (operation)
            {
                case "sin":
                    return Math.Sin(x);
                case "cos":
                    return Math.Cos(x);
                case "tan":
                    return Math.Tan(x);
                case "asin":
                    return Math.Asin(x);
                case "acos":
                    return Math.Acos(x);
                case "atan":
                    return Math.Atan(x);
                case "ln":
                    return Math.Log(x, Math.E);
                case "log":
                    return Math.Log10(x); //base is fixed to 10
                case "sqrt":
                    return Math.Sqrt(x);
                default:
                    throw new NotImplementedException();

            }

        }
    }
}
