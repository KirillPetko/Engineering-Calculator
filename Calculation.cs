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
        private static Dictionary<string, string> sequences = new Dictionary<string, string>
        {
            //sequences of type:
            ["sci-number"] = "^(\\-)?\\d+[\\.?\\d]*([eE][-+]?[0-9]+)?$", //fractional number, or in sientific notation
            ["number"] = "(\\-)?\\d+[\\.?\\d]*", //fractional number
            ["pos-number"] = "\\d+[\\.?\\d]*", //positive fractional number
            ["arc-trigonometry"] = "(asin|acos|atan)(\\-)?\\d+[\\.?\\d]*", //asin-3 or acos90 (single operation)
            ["trigonometry"] = "(sin|cos|tan|ln|log|sqrt)(\\-)?\\d+[\\.?\\d]*", //sin-3 or cos90 (single operation)
            ["neg-arg-and-power"] = "\\d+[\\.?\\d]*\\-\\^\\-(\\d+[\\.?\\d]*\\-\\^\\-)*\\d+[\\.?\\d]*", //3-^-2-^-4...-^-0.3
            ["pos-arg-neg-power"] = "\\d+[\\.?\\d]*(\\^\\-|\\-\\^)(\\d+[\\.?\\d]*(\\^\\-|\\-\\^))*\\d+[\\.?\\d]*", //3^-2^-0.4...^-3 
            ["pos-arg-pos-power"] = "\\d+[\\.?\\d]*\\^(\\d+[\\.?\\d]*\\^)*\\d+[\\.?\\d]*", //3^2^0.4...^3 
            ["mul-div-neg-arg"] = "\\d+[\\.?\\d]*(\\*\\-|\\/\\-)(\\d+[\\.?\\d]*(\\*\\-|\\/\\-))*\\d+[\\.?\\d]*", //7/-6*-0.03*-.../-123          
            ["mul-div-pos-arg"] = "\\d+[\\.?\\d]*[\\*|\\/](\\d+[\\.?\\d]*[\\*|\\/])*\\d+[\\.?\\d]*", //7/6*0.03*.../123           
            ["add-sub"] = "\\d+[\\.?\\d]*[\\+|\\-](\\d+[\\.?\\d]*[\\+|\\-])*\\d+[\\.?\\d]*", //3+2-3+8.3-...+9
            //one sign operations
            ["one-sign-opr"] = "\\*|\\/|\\+|\\-|\\^",
            ["two-sign-opr"] = "(\\^\\-)|(\\-\\^)|(\\*\\-)|(\\/\\-)",
            ["three-sign-opr"] = "\\-\\^\\-",
            ["math-opr"] = "sin|cos|tan|ln|log|sqrt",
            ["arc-trigonometry-opr"] = "asin|acos|atan",
            ["all-math-opr"] = "sin|cos|tan|asin|acos|atan|ln|log|sqrt",
        };

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

            Regex number = new Regex(sequences["pos-number"]);
            Regex signOperation = new Regex(sequences["one-sign-opr"]);
            Regex textOperation = new Regex(sequences["all-math-opr"]);

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
                    if (Char.IsDigit(c) || c == '.') wordType = 1;
                    else if (c == '*' || c == '/' || c == '+' || c == '-' || c == '^') wordType = 2;
                    else if (Char.IsLetter(c)) wordType = 3;
                    else if (c == '(') wordType = 4;
                    else if (c == ')') return false;
                    else return false;

                    word += c;
                    //false was not returned yet and the end of expression reached - it must be a single number
                    if (i == expression.Length - 1)
                        return true;
                    else
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
                        if (!number.IsMatch(word)) //met operation - check word
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
                        word += c;
                        break;
                    case (3, 1): return false;      //after text opreation only opened bracket (char type 4)
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

        /*
        * function which takes a string in form of expression, then open brackets,
         * taking inner string as argument to genereted recurrent entrance of itself
         * then it divides expression in sequences with priorities of operations while
         * callcucating them sequentially with the help of overloaded examples of itself
         * it also adds 0 in the beginning not to break further calculation process as well
         * as checking validity of gotten result before returning it, otherwise this function 
         * throws an exeption or continues by recurrent enter untill expression
         * becomes a double-passable string, then it returns a result of that pasing 
         */
        static double Calculate(string expression)
        {
            double result;

                
            List<int> openedBracketsIndexes = new List<int>();
            for (int i = 0; i < expression.Length; i++)//iterate over chars to find each corresponding bracket 
            {                                          //pair then hand expression over bracket opening function
                if (i < expression.Length - 1)         //[i+1] shold not be out of range of string
                {
                    if (expression[i] == '(')
                        openedBracketsIndexes.Add(i);
                    else if (expression[i] == ')' && expression[i + 1] == '^')
                    {
                        expression = Calculate(expression, openedBracketsIndexes, i, true);
                        i = openedBracketsIndexes.Last();
                        openedBracketsIndexes.RemoveAt(openedBracketsIndexes.Count - 1);                                                  
                    }
                    else if (expression[i] == ')' && expression[i + 1] != '^')
                    {
                        expression = Calculate(expression, openedBracketsIndexes, i, false);
                        i = openedBracketsIndexes.Last();
                        openedBracketsIndexes.RemoveAt(openedBracketsIndexes.Count - 1);
                    }

                }
                else //')' is met at the end of the expression string
                {
                    if (expression[i] == ')')
                    {
                        expression = Calculate(expression, openedBracketsIndexes, i, false);
                        i = openedBracketsIndexes.Last();
                        openedBracketsIndexes.RemoveAt(openedBracketsIndexes.Count - 1);
                    }
                }
            }

            //0 is inserted to insure proper calculation of sequence
            if (expression[0] == '-') expression = "0" + expression;                                                                
  
            expression = Calculate(sequences["arc-trigonometry"], expression, 4);                                                                                                                    
            expression = Calculate(sequences["trigonometry"], expression, 3);                                 
            expression = Calculate(sequences["neg-arg-and-power"], expression, 2);               
            expression = Calculate(sequences["pos-arg-neg-power"], expression, 1);   
            expression = Calculate(sequences["pos-arg-pos-power"], expression, 0);                           
            expression = Calculate(sequences["mul-div-neg-arg"], expression, 1);   
            expression = Calculate(sequences["mul-div-pos-arg"], expression, 0);

            //sequences of type (3++3--2+-3...) may be just replaced with one sign
            expression = expression.Replace("++", "+");                                                                             
            expression = expression.Replace("--", "+");                                                                           
            expression = expression.Replace("+-", "-");                                                                            
            expression = expression.Replace("-+", "-");

            if (expression[0] == '-') expression = "0" + expression;

            expression = Calculate(sequences["add-sub"], expression, 0);

            //result is not defined (like acos(2)) contains "NaN"
            if (expression.Contains("NaN"))                                                                                         
                throw new Exception("NaN value");    
            //result is infinity (1/0) contains "Infinity"
            else if (expression.Contains("Infinity"))                                                                               
                throw new Exception("Infinity");                                                                                                                                           
            if (Regex.IsMatch(expression, sequences["sci-number"], RegexOptions.IgnoreCase))
                result = Double.Parse(expression, CultureInfo.InvariantCulture); 
            else
                result = Calculate(expression);
            return result;
        }

        //opens corresponding bracket pair in expression calculating inner expression,
        //considering power operation after to avoid broken sequences
        static string Calculate(string expression, List<int> openedBracketsIndexes, int i, bool isPower)
        {
            double subresult;
            Regex subexpBrackets;
            int subexpLength = i + 1 - openedBracketsIndexes.Last();
            string subexpressionWithBrackets = expression.Substring(openedBracketsIndexes.Last(), subexpLength),
                   subexpression = Regex.Replace(subexpressionWithBrackets, "\\(|\\)", String.Empty),
                   replacement, resultStr;
            if (isPower)
            {
                subexpBrackets = new Regex(Regex.Escape(subexpressionWithBrackets)); //Escaping a minimal set of characters                                                                                                                        
                subresult = Calculate(subexpression);                                //is made for cases like (-2)^(-2),                                                  
                if (subresult < 0)                                                   //to avoid sequences like 2-^2-. (*)
                {                                                                    //replacing first match of escaped
                    replacement = Convert.ToString(-1 * subresult) + '-';            //subexpressionWithBrackets
                    expression = subexpBrackets.Replace(expression, replacement, 1); //in expression changing '-' position 
                }                                                                    //to after-number.
                else
                {
                    resultStr = Convert.ToString(subresult);
                    expression = expression.Replace(subexpressionWithBrackets, resultStr);
                }                                                                    //(*) when there are no raise to power
            }                                                                        //operations with negative arguments    
            else                                                                     //replacing all same expressions in     
            {                                                                        //brackets with single calculated result    
                resultStr = Convert.ToString(Calculate(subexpression));              //is acceptable    
                expression = expression.Replace(subexpressionWithBrackets, resultStr);
            }
            expression = expression.Replace(',', '.');
            return expression;
        }

        //finds sequences in input string by specified pattern argument
        static string Calculate(string sequenceStr, string expression, int operationType)
        {
            Regex sequence = new Regex(sequenceStr);
            MatchCollection sequences = sequence.Matches(expression);
            foreach (Match s in sequences)
                expression = expression.Replace(s.Value, Calculate(s, operationType));
            return expression;
        }

        //calculates sequenses of same priority replacing them with fractional number represented in a string
        static string Calculate(Match seqence, int operationType)
        {
            Regex number = new Regex(sequences["pos-number"]); //regex for any double number without sign (consider pos sci number)
            MatchCollection numbers;
            double result;
            string resultS;
            Regex operation;
            switch (operationType)
            {
                case 0:
                    operation = new Regex(sequences["one-sign-opr"]);
                    break;
                case 1:
                    operation = new Regex(sequences["two-sign-opr"]);
                    break;
                case 2:
                    operation = new Regex(sequences["three-sign-opr"]);
                    break;
                case 3:
                    operation = new Regex(sequences["math-opr"]);
                    number = new Regex(sequences["number"]); //argument of function may be negative 
                    break;                                   //(like sin-3 or sqrt-4 sequences)
                case 4:
                    operation = new Regex(sequences["arc-trigonometry-opr"]);
                    number = new Regex(sequences["number"]);
                    break;
                default:
                    throw new ArgumentException();
            }

            numbers = number.Matches(seqence.Value);

            if (operationType != 3 && operationType != 4)
            {
                MatchCollection operations = operation.Matches(seqence.Value);
                result = Double.Parse(numbers[0].Value, CultureInfo.InvariantCulture); //result of each sequence of operations 
                for (int i = 0; i < operations.Count; i++)                             //with same priority is formed by performing 
                {                                                                      //this operation with previous result, forming 
                    double x = Double.Parse(numbers[i + 1].Value, CultureInfo.InvariantCulture); //new value to perform this operation 
                    result = Calculate(result, x, operations[i].Value);                //again on the next step, untill sequence is not 
                }                                                                      //calculated completely, then result is returned 
                                                                                       //to replace the sequence in expression
            }                                                                                       
            //operation is trigonometry one -> return result of that single operation
            else result = Calculate(Double.Parse(numbers[0].Value, CultureInfo.InvariantCulture), operation.Match(seqence.Value).Value);

            resultS = Convert.ToString(result, CultureInfo.InvariantCulture);
            resultS = resultS.Replace(',', '.');
            return resultS;

        }

        //performs mathematical operation between two fractional numbers
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
                    throw new NotImplementedException("Such operation has not been implemented!");

            }

        }
    }
}
