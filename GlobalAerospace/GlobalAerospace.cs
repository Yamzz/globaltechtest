using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalAerospace
{
    public class GlobalAerospace
    {
        //key pair mappings 
        private const int MIN_DURATION = 100;
        private const int WAIT_DURATION = 500;
        private static TimeSpan timeBetweenKeyPresses;
        private static TimeSpan realTimeTotalTimeSpan;
        private static DateTime timeOfKeyChain;
        private static Dictionary<string, List<String>> keyPad = new Dictionary<string, List<String>>();
        private static Dictionary<Char, Tuple<Char, int>> Grid;
        private static String[] letters =
        {
          "0", "1", "abc", "def", "ghi", "jkl", "mno", "pqrs", "tuv", "wxyz"
        };

        ///////////////////////////////////////////////
        //use to test manually 
        string globalAeroSpaceTest = "global aerospace";
        //global aerospace
        //4555666222555233777766677777222233
        //////////////////////////////////////////////

        public static void Main(string[] args)
        {
            Grid = new Dictionary<Char, Tuple<Char, int>>();
            Grid.Add(' ', Tuple.Create('0', 1));
            Grid.Add('a', Tuple.Create('2', 1));
            Grid.Add('b', Tuple.Create('2', 2));
            Grid.Add('c', Tuple.Create('2', 3));
            Grid.Add('d', Tuple.Create('3', 1));
            Grid.Add('e', Tuple.Create('3', 2));
            Grid.Add('f', Tuple.Create('3', 3));
            Grid.Add('g', Tuple.Create('4', 1));
            Grid.Add('h', Tuple.Create('4', 2));
            Grid.Add('i', Tuple.Create('4', 3));
            Grid.Add('j', Tuple.Create('5', 1));
            Grid.Add('k', Tuple.Create('5', 2));
            Grid.Add('l', Tuple.Create('5', 3));
            Grid.Add('m', Tuple.Create('6', 1));
            Grid.Add('n', Tuple.Create('6', 2));
            Grid.Add('o', Tuple.Create('6', 3));
            Grid.Add('p', Tuple.Create('7', 1));
            Grid.Add('q', Tuple.Create('7', 2));
            Grid.Add('r', Tuple.Create('7', 3));
            Grid.Add('s', Tuple.Create('7', 4));
            Grid.Add('t', Tuple.Create('8', 1));
            Grid.Add('u', Tuple.Create('8', 2));
            Grid.Add('v', Tuple.Create('8', 3));
            Grid.Add('w', Tuple.Create('9', 1));
            Grid.Add('x', Tuple.Create('9', 2));
            Grid.Add('y', Tuple.Create('9', 3));
            Grid.Add('z', Tuple.Create('9', 4));


            //heading
            Console.Write("\n An old-style mobile phone has 12 keys for input :\n");
            Console.Write("(‘1’, ‘2’, ‘3’, ‘4’, ‘5’, ‘6’, ‘7’, ‘8’, ‘9’, ‘0’, ‘*’ and ‘#’)\n");
            Console.Write("\n-------------------------------------------------------------\n");
            Console.Write("\n Press Enter to Test your input data: ");
            Console.WriteLine("Write Something : ");

            var inputText = new StringBuilder();
            ConsoleKeyInfo cki;
            timeBetweenKeyPresses = new TimeSpan();
            realTimeTotalTimeSpan = new TimeSpan();
            var keyPresses = 0;
            var number = 1;


            do
            {
                cki = Console.ReadKey(true);

                //get user input
                if (!Char.IsNumber(cki.KeyChar))
                {
                    Console.WriteLine("Input is not part of numeric grid");
                    //reset keypress
                    keyPresses = 0;
                }
                else
                {

                    timeOfKeyChain = DateTime.Now;

                    if (Int32.TryParse(cki.KeyChar.ToString(), out number))
                    {
                        keyPresses++;
                        Console.WriteLine("Key Received: {0} ", number);
                        if (keyPresses > 1)
                        {
                            timeBetweenKeyPresses = DateTime.Now - timeOfKeyChain;
                        }

                        //Console.WriteLine("Time Between Key Presses: " + timeBetweenKeyPresses.Milliseconds.ToString() + " milliseconds.");
                        //test input here
                        inputText.Append(number);
                    }
                    else
                    {
                        Console.WriteLine("Unable to parse input");
                    }
                }

                realTimeTotalTimeSpan += timeBetweenKeyPresses;

                if (keyPresses > 1)
                {
                    //Console.WriteLine("Time Between Key Presses: " + timeBetweenKeyPresses.Milliseconds.ToString() + " milliseconds.");
                    keyPresses = 0;
                    timeBetweenKeyPresses = new TimeSpan();
                }

            }
            while (cki.Key != ConsoleKey.Enter);

            //convert phone numbers to
            Console.WriteLine("Total Milliseconds Time Span in is  {0} ", realTimeTotalTimeSpan.TotalMilliseconds);
            Console.WriteLine("Number entered is {0}", inputText.ToString());

            var myStr = GetStringFromKeypadInput(inputText.ToString());
            Console.WriteLine("Number converted to string is {0}", myStr);

            //calculate time for word given 
            var minTime = CalculateMinimumTime(myStr);
            Console.WriteLine("Minimum Time is for {0} is {1} milliseconds", myStr, minTime);
            Console.ReadLine();

            /////////////////////////////////////////////////////////////////////////////////////////
            //##################
            //Minimum Time is for global aerospace is [4900] milliseconds
            //this is calculated by 100ms threshold between button presses * which is the amount of time that elapses before another key input
            //"455566622,255523377776667777,7222233" 
            //So a delay of 500 milliseconds of where the comma is indicated.  
            //500ms delay is then added to increment of milliseconds delay between key presses please see [*1] on CalculateMinimumTime(String word) 
            //By creating a dictionary data structure we are able to map the keypad to specific letters in the keypad 
            //Dictionary structure cannot include null or duplicated and keys must be unique
            //The Tuple allows us to map the letter to the index position of the key presses for e.g [Grid.Add('s', Tuple.Create('7', 4)); [You need to press the 7 key 4times to get to indicate letter s]
            //####################
            /////////////////////////////////////////////////////////////////////////////////////////////
        }



        /// <summary>
        /// Method to convert keypad integer to word
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private static String GetStringFromKeypadInput(String input)
        {
            var lastDigit = 0;
            var count = 1;
            var result = "";

            for (int i = 0; i < input.Length; i++)
            {
                var currentDigit = input[i] - '0';
                if (currentDigit >= 2 && currentDigit <= 9)
                {
                    if (lastDigit == 0)
                    {
                        lastDigit = currentDigit;
                    }
                    else if (currentDigit == lastDigit)
                    {
                        count++;
                    }
                    else
                    {
                        result += GetChar(lastDigit, count);
                        lastDigit = currentDigit;
                        count = 1;
                    }
                }
            }
            return result + GetChar(lastDigit, count);

        }


        /// <summary>
        /// Get the character at specific position of 
        /// keypad 
        /// </summary>
        /// <param name="digit"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private static char GetChar(int digit, int count)
        {
            while (count > letters[digit].Length)
            {
                count -= letters[digit].Length;
            }
            return letters[digit][count - 1];
        }




        /// <summary>
        /// Calculate the time between the characters pressed
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        public static int CalculateMinimumTime(String word)
        {
            var totalMillis = 0;
            for (int index = 0; index <= word.Length - 1; index++)
            {
                //get the char at this string index position
                var charInfo = Grid[word[index]];
                //get position of char in the tuple
                var Pos = charInfo.Item2;
                //we calculate the key press time by the min e.g pos 2 * 100 = 200miliseconds
                var keyPressTime = Pos * MIN_DURATION;

                if (index != 0)
                {
                    var lastCharInfo = Grid[word[index - 1]];
                    if (lastCharInfo.Item1 == charInfo.Item1)
                        keyPressTime += WAIT_DURATION; //[*1]
                }
                totalMillis += keyPressTime;
            }
            return totalMillis;
        }



        /// <summary>
        /// Another attempt to retrieve keypad entry 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private string GetStringViaKeyPadDigits(String input)
        {
            var result = new StringBuilder();
            for (int i = 0; i < input.Length; i++)
            {
                var currDigit = input[i];
                int count = 0;
                if (currDigit == input[i + 1])
                {
                    //two are the same
                    count++;
                }
                if (currDigit == input[i + 2])
                {
                    //three digits the same 
                    count++;
                }

                if (currDigit == input[i + 3])
                {
                    //three digits the same 
                    count++;
                }


                //get the Curr digit and its occurrences
                //retrieve the string that maps to it
                var str = GetPhoneString(currDigit, count);
                result.Append(str);
            }
            return result.ToString(); 
        }


        /// <summary>
        /// Get the key pad value
        /// </summary>
        /// <param name="currDigit"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private static string GetPhoneString(char currDigit, int count)
        {
            keyPad["1"] = new List<string> { string.Empty };
            keyPad["2"] = new List<string> { "a", "b", "c" };
            keyPad["3"] = new List<string> { "d", "e", "f" };
            keyPad["4"] = new List<string> { "g", "h", "i" };
            keyPad["5"] = new List<string> { "j", "k", "l" };
            keyPad["6"] = new List<string> { "m", "n", "o" };
            keyPad["7"] = new List<string> { "p", "q", "r", "s" };
            keyPad["8"] = new List<string> { "t", "u", "v" };
            keyPad["9"] = new List<string> { "w", "x", "y", "z" };
            keyPad["0"] = new List<string> { "\t" };

            var keyBase = keyPad[currDigit.ToString()];
            var letter = keyBase[count];
            return letter;
        }


    }
}
