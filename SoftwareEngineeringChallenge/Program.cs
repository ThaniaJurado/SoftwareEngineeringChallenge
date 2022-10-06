using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using static SoftwareEngineeringChallenge.Program;

namespace SoftwareEngineeringChallenge
{
    class Program
    {

        /*
         * Title: Software Engineering Challenge
         * Applicant: Thania Elizabeth Jurado García
         */
        #region Description
        /*
            John Smith has a 4 year old son named Bob. Bob has a marble collection and has named all of
            the marbles. Each marble has a weight of 1 oz. or less and is one of the following colors: Red,
            Orange, Yellow, Green, Blue, Indigo, Violet (ROYGBIV).
            John would like to line up the marbles in ROYGBIV order for Bob. John wants to keep all of the
            marbles that weigh at least 0.5 oz and have palindromes for a name, discarding the rest.

            Extra Information:
            Write a solution that takes in a collection of Marbles and returns the filtered &amp; ordered list of
            marbles back. The palindrome should ignore capitalization and punctuation so Bob o’Bob is a
            palindrome. Please include comments describing the time and space complexity of your
            algorithm. Additionally, please include comments that describe what your deployment strategy
            would be to host this workload in (any) cloud platform in a way that is accessible, repeatable, &amp;
            modular (example: as a serverless function or REST service… etc). Discuss any deployment
            technologies &amp; automation strategies you would wrap around the solution. What would you do if
            Bob has millions of marbles to process? To submit, you can use https://ideone.com/ or send us
            a git link.
         */
        #endregion
        static void Main(string[] args)
        {
            //example subset of Bob's marbles (I rewrited this as an Anonymous Type Array)
            var marbles = new []
            {
                new { id = 1, color = "blue", name = "Bob", weight = 0.5 },
                new { id = 2, color = "red", name = "John Smith", weight = 0.25 },
                new { id = 3, color = "violet", name = "Bob O'Bob", weight = 0.5 },
                new { id = 4, color = "indigo", name = "Bob Dad-Bob", weight = 0.75 },
                new { id = 5, color = "yellow", name = "John", weight = 0.5 },
                new { id = 6, color = "orange", name = "Bob", weight = 0.25 },
                new { id = 7, color = "blue", name = "Smith", weight = 0.5 },
                new { id = 8, color = "blue", name = "Bob", weight = 0.25 },
                new { id = 9, color = "green", name = "Bobb Ob", weight = 0.75 },
                new { id = 10, color = "blue", name = "Bob", weight = 0.5 }
           };

            /*
              I cannot send an Anonymous Type Array to a function as a parameter and get it as a return value, 
                so I decided to create and use a class named Marble 
             */

            List<Marble> marbleList =
                marbles.Select(m => new Marble() { id = m.id, color = m.color, name = m.name, weight = m.weight }).ToList();

            /*
             * And then, thinking of recyling this code for either another little list of marbles or a huge list,
             * I coded it to work on a function. This function, if needed, could be implemented in a REST API or un a WCF.
             * For example, it would receive the list of marbles from the client side and return the sortered and filtered list.
             * Also, the function could be reuse in other projects. I would put it in the logic layer.  
             */


            for(int i = 0;i<20;i++)
                marbleList.AddRange(marbleList);


            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            List<Marble> sortedAndFilteredList = SortAndFilter(marbleList);
            stopwatch.Stop();
            TimeSpan ts = stopwatch.Elapsed;
            Console.WriteLine("Sorting and filtering time: {0:00}:{1:00}.{2}", ts.Minutes, ts.Seconds, ts.Milliseconds);

            //Showing the final group of marbles
            Console.WriteLine("---------- Sorted and filtered list of marbles ----------");
            foreach (var marble in sortedAndFilteredList)
            {
                marble.ShowProperties();
            }

            //Just to prevent Console App to close (only for Console App in Visual Studio)
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey(); 
        }


        #region Methods
        /*
         * Even though the statement mentions first the color sorting, I filtered first by weight, then by palinrome names
         * and after that I sorted the list by color. 
         * 
         * I chose this way after doing some time tests with a list of 10,485,760 (adding the list to itself 20 times):
         * 
         *      Sorting first and then filtering by weight and palindrome: 25.500 seconds
         *      Filtering first by palindrome, then by weight and sorgin: 30.600 seconds
         *      Filtering first by weight, then by palindrome and sorting: 22.300 seconds
         *      
         * For the filtering and sorting I used linq. 
         * Being a more complex logic, I decided to create a method just to determine whether the name is a palindrome or not
         * Finally, I created a enum (user-defined value type) to sort the list by a special color order
         */
        public static List<Marble> SortAndFilter(List<Marble> listOfMarbles)
        {
            listOfMarbles = listOfMarbles.Where(m => m.weight >= 0.5 && IsAPalindrome(m.name)).OrderBy(m => (int)((ColorOrder)Enum.Parse(typeof(ColorOrder), m.color))).ToList();
            return listOfMarbles;
        }

        public static bool IsAPalindrome(string word)
        {
            //I uppercased it to avoid differences between upper and lower case
            word = word.ToUpper();

            //I declared a regex that only takes upper case letters
            Regex rgx = new Regex("[^A-Z]"); 

            //Then I replaced whathever is not a uppercase letter with an empty space (to get rid of punctuation marks)
            word = rgx.Replace(word, "");

            //After that, I took the "simplified" text, reversed it and stored it in another variable
            char[] arr = word.ToCharArray();
            Array.Reverse(arr);
            string reversedWord = new string(arr);

            //if the sanitized word and the reversed sanitized word are equal, it must be a palindrome
            return (word == reversedWord);
        }
        #endregion

        #region Auxiliar objects
        public class Marble
        {
            public int id { get; set; }
            public string color { get; set; }
            public string name { get; set; }
            public double weight { get; set; }

            public void ShowProperties()
            {
                Console.WriteLine("id:{0} color: {1} name: {2} weight: {3}" , id, color, name, weight);
            }
        }

        /*
         * Enum created to give colors a specific order
         */
        public enum ColorOrder
        {
            red = 1,
            orange = 2,
            yellow = 3,
            green = 4,
            blue = 5,
            indigo = 6,
            violet = 7
        }
        #endregion
    }
}
