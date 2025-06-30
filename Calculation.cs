using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Metadata;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Engineering_Calculator
{
    //responsible for recurrently shortening (calculating) expression string as well as validating it
    internal class Calculation
    {
        public Calculation() 
        {
            IsValidExpression = false;
        }
        public Calculation(string _input)
        {
            Input = _input;
            IsValidExpression = 
                CalculationValidator.VerifyInput(input, sequencePatterns["pos-number"], sequencePatterns["all-math-opr"]);
            if (IsValidExpression)
            {
                Expression = Input;
                Result = Calculate(Expression);
            }
            else
            {
                Exception e = ErrorFactory.CreateCalculationException("Invalid input", "FormatException", _input, _input);
                throw e;
            }
        }

        private static int currentRecursiveCalls = 0;
        private string input;
        private string expression;
        private bool isValidExpression;
        private double result;

        private static Dictionary<string, string> sequencePatterns = new Dictionary<string, string>
        {
            //sequences of type:
            ["sci-number"] = "(\\-)?\\d+[\\.?\\d]*([eE][-+]?[0-9]+)?", //fractional number, or in sientific notation
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
                return this.input;
            }
            set
            {
                if (value == String.Empty) input = "0";
                else this.input = value;
            }
        }
        public string Expression { get => expression; set => expression = value; }
        public bool IsValidExpression { get => isValidExpression; set => isValidExpression = value; }
        public double Result { get => result; set => result = value; }

        /*
         * function which takes a string in form of expression, then open brackets,
         * taking inner string as argument to recurrent entrance of itself then
         * divides expression in sequencePatterns with priorities of operations
         * callcucating them sequentially with the help of overloaded examples. 
         * Also adds 0 in the beginning not to break further calculation process.  
         * If calculated sequence has signs continues by recurrent enter untill expression
         * becomes a double-parssable string, then returns a result of that parsing 
         */
        private double Calculate(string expression)
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
                else   //')' is at the end of the expression
                {
                    if (expression[i] == ')')
                    {
                        expression = Calculate(expression, openedBracketsIndexes, i, false);
                        i = openedBracketsIndexes.Last();
                        openedBracketsIndexes.RemoveAt(openedBracketsIndexes.Count - 1);
                    }
                }
            }

            //0 is inserted to insure proper emptyCalculation of sequence
            if (expression[0] == '-') expression = "0" + expression;                                                                
  
            expression = Calculate(sequencePatterns["arc-trigonometry"], expression, 4);                                                                                                                    
            expression = Calculate(sequencePatterns["trigonometry"], expression, 3);                                 
            expression = Calculate(sequencePatterns["neg-arg-and-power"], expression, 2);               
            expression = Calculate(sequencePatterns["pos-arg-neg-power"], expression, 1);   
            expression = Calculate(sequencePatterns["pos-arg-pos-power"], expression, 0);                           
            expression = Calculate(sequencePatterns["mul-div-neg-arg"], expression, 1);   
            expression = Calculate(sequencePatterns["mul-div-pos-arg"], expression, 0);
            expression = SimplifyAdditionSubtractionSigns(expression);

            if (expression[0] == '-') expression = "0" + expression;
            expression = Calculate(sequencePatterns["add-sub"], expression, 0);

            if (Regex.IsMatch(expression, sequencePatterns["sci-number"]))
                result = Double.Parse(expression, CultureInfo.InvariantCulture);
            else
            {
                currentRecursiveCalls++;
                result = Calculate(expression);
            }
            return result;
        }

        //opens corresponding bracket pair in expression calculating inner expression,
        //considering power operation after to avoid broken sequencePatterns
        private string Calculate(string expression, List<int> openedBracketsIndexes, int i, bool isPower)
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
                if (subresult < 0)                                                   //to avoid sequencePatterns like 2-^2-. (*)
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

        //finds sequencePatterns in expression string invoking their calculation
        private string Calculate(string sequenceStr, string expression, int operationType)
        {
            //check if wrong opperands appeared in expression during calculation ect
            CalculationValidator.CalculationCheck(expression, input, 
                        sequencePatterns["one-sign-opr"], ref currentRecursiveCalls);
            Regex sequence = new Regex(sequenceStr);
            MatchCollection sequences = sequence.Matches(expression);
            foreach (Match s in sequences)
                expression = expression.Replace(s.Value, Calculate(s, operationType));

            sequences = sequence.Matches(expression);
            if (sequences.Count > 0)
            {   
                //calculated sequnce replaced in expression creates the same one sequnce
                currentRecursiveCalls++;
                expression = Calculate(sequenceStr, expression, operationType);
            }
            return expression;
        }

        //calculates sequenses of same priority replacing them with fractional number represented in a string
        private static string Calculate(Match sequence, int operationType)
        {
            Regex number = new Regex(sequencePatterns["pos-number"]); 
            MatchCollection numbers, operations;
            double result;
            string resultS;
            Regex operation;
            switch (operationType)
            {
                case 0:
                    operation = new Regex(sequencePatterns["one-sign-opr"]);
                    break;
                case 1:
                    operation = new Regex(sequencePatterns["two-sign-opr"]);
                    break;
                case 2:
                    operation = new Regex(sequencePatterns["three-sign-opr"]);
                    break;
                case 3:
                    operation = new Regex(sequencePatterns["math-opr"]);
                    number = new Regex(sequencePatterns["number"]); //argument of function may be negative 
                    break;                                          //(like sin-3 or sqrt-4 sequences)
                case 4:
                    operation = new Regex(sequencePatterns["arc-trigonometry-opr"]);
                    number = new Regex(sequencePatterns["number"]);
                    break;
                default:
                    throw new ArgumentException();
            }

            numbers = number.Matches(sequence.Value);

            if (operationType != 3 && operationType != 4)
            {
                operations = operation.Matches(sequence.Value);
                result = Double.Parse(numbers[0].Value, CultureInfo.InvariantCulture);          //result of each sequence of operations 
                for (int i = 0; i < operations.Count; i++)                                      //with same priority is formed by  
                {                                                                               //performing this operation with previous  
                    double x = Double.Parse(numbers[i + 1].Value, CultureInfo.InvariantCulture);//result, forming new value to perform 
                    result = Calculate(result, x, operations[i].Value);                         //this operation again on the next step,  
                }                                                                               //untill sequence is not calculated  
                                                                                                //completely, then result is returned 
            }                                                                                   //to replace the sequence in expression    
            //operation is trigonometry one -> return result of that single operation
            else result = Calculate(Double.Parse(numbers[0].Value, CultureInfo.InvariantCulture), operation.Match(sequence.Value).Value);

            resultS = Convert.ToString(result, CultureInfo.InvariantCulture);
            resultS = resultS.Replace(',', '.');
            return resultS;

        }

        //performs mathematical operation between two operands (x,y)
        private static double Calculate(double x, double y, string operation)
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

        //performs text-type operation on operand (x)
        private static double Calculate(double x, string operation)
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

        //replaces sequencePatterns of type(3+-+3---2+-3...) with one operation sign
        private static string SimplifyAdditionSubtractionSigns(string expression)
        {
            string result = "";
            for (int i = 0; i < expression.Length; i++)
            {
                char c = expression[i];

                if (c == '+' || c == '-')
                {
                    int minusCount = 0;
                    // Process consecutive + and -
                    int j = i;
                    for (; j < expression.Length; j++)
                    {
                        if (expression[j] == '-')
                            minusCount++;
                        else if (expression[j] != '+')
                            break;  //end of sign sequence
                    }
                    //decide which sign to add
                    if (minusCount % 2 == 0)
                        result += "+";
                    else
                        result += "-";
                    i = j - 1; //move outer loop index to end of processed sequence
                }
                else
                    result += c;
            }
            return result;
        }
    }
}
