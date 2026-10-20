using System;
using System.Collections.Generic;

namespace MealyMachine
{
    // State definitions: A, B, C, D, Trap
    public enum State
    {
        A,      // Start 
        B,      
        C,     
        D,      
        Trap    
    }

    public class ParseResult
    {
        public bool IsAccepted { get; set; }
        public List<int> Outputs { get; set; } = new List<int>();
    }

    public class MealyMachine
    {
        private State _currentState;

        public void Reset()
        {
            _currentState = State.A;
        }

        public int Transition(char inputChar)
        {
            int output = 0;

            switch (_currentState)
            {
                case State.A:
                    if (inputChar == '/')
                    {
                        _currentState = State.B;
                    }
                    else
                    {
                        _currentState = State.Trap;
                    }
                    break;

                case State.B:
                    if (inputChar == '*')
                    {
                        _currentState = State.C;
                    }
                    else
                    {
                        _currentState = State.Trap;
                    }
                    break;

                case State.C:
                    if (inputChar == '*')
                    {
                        _currentState = State.D;
                    }
                    else
                    {
                        _currentState = State.C;
                    }
                    break;

                case State.D:
                    if (inputChar == '/')
                    {
                        _currentState = State.A;
                        output = 1; 
                    }
                    else if (inputChar == '*')
                    {
                        _currentState = State.D; 
                    }
                    else
                    {
                        _currentState = State.C;
                    }
                    break;

                case State.Trap:
                default:
                    _currentState = State.Trap;
                    break;
            }

            return output;
        }

        public ParseResult Parse(string inputString)
        {
            Reset();
            var result = new ParseResult();
            bool emittedOne = false;

            foreach (char c in inputString)
            {
                int output = Transition(c);
                result.Outputs.Add(output);

                if (output == 1)
                {
                    emittedOne = true;
                }
            }

            result.IsAccepted = (_currentState == State.A) && emittedOne;
            return result;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var parser = new MealyMachine();

            string[] acceptedTestCases = new string[]
            {
                "/* a* */",
                "/**/",
                "/*** */",
                "/* aaa* aaa* */",
                "/* a|* a* */"
            };

            string[] rejectedTestCases = new string[]
            {
                "/** * ,",
                "/**/a/*aa*/",
                "aaa/* */aa",
                "/*/",
                "/** a|",
                "// aaaa"
            };

            Console.WriteLine("=== ACCEPTED TEST CASES ===");
            foreach (var testCase in acceptedTestCases)
            {
                PrintResult(parser, testCase);
            }

            Console.WriteLine("\n=== REJECTED TEST CASES ===");
            foreach (var testCase in rejectedTestCases)
            {
                PrintResult(parser, testCase);
            }

            Console.WriteLine("\n==========================================");
            Console.WriteLine("        MEALY MACHINE INTERACTIVE TESTER   ");
            Console.WriteLine("==========================================");
            
            while (true)
            {
                Console.Write("\nEnter input string (or type 'exit' to quit): ");
                string userInput = Console.ReadLine();

                if (string.Equals(userInput, "exit", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("Exiting program...");
                    break;
                }

                PrintResult(parser, userInput);
            }
        }

        static void PrintResult(MealyMachine parser, string testCase)
        {
            ParseResult result = parser.Parse(testCase);
            string outputList = string.Join(", ", result.Outputs);
            Console.WriteLine($"Input: {testCase,-22} | Accepted: {result.IsAccepted,-5} | Outputs: [{outputList}]");
        }
    }
}
