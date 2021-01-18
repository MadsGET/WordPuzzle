using System;
using System.Collections.Generic;
using System.IO;

namespace WordPuzzle
{
    class Program
    {
        static int amount = 200;
        static Random random = new Random();
        static int[]  wordLengthParamater = { 7, 8, 9, 10 };
        static string toCheck = "";

        static void Main(string[] args)
        {
            // Create and assign wordlist
            string[] wordlist = FetchWordlist(wordLengthParamater);
            string[] bindList = FetchWordlist(new int[]{3});
            int droppedCount = 0;

            for(int i = 0; i < amount; i++) 
            {
                string chosenWord = wordlist[random.Next(0, wordlist.Length - 1)];
                string match = "";
                int startPos = 0;

                while (match == "" && startPos != chosenWord.Length) 
                {
                    match = SpliceAndFind(chosenWord, wordlist, startPos);
                    startPos++;
                }

                // Is the checker word valid?
                bool isValid = false;

                foreach (string word in bindList) 
                {
                    if (word == toCheck) 
                    { 
                        isValid = true;
                    }
                }

                if (isValid)
                {
                    Console.WriteLine(chosenWord + " => " + toCheck + " => " + match + "\n");
                }
                else 
                {
                    droppedCount++;
                }
            }

            Console.WriteLine("Amount of invalid puzzles: " + droppedCount);
        }

        public static bool WordValidation(string word) 
        {
            if (string.IsNullOrEmpty(word)) return false;

            foreach (char c in word) 
            {
                if (char.IsPunctuation(c) || char.IsSymbol(c) || char.IsDigit(c) || char.IsWhiteSpace(c)) return false;
            }
            return true;
        }

        public static bool WordLengthValidation(int[] parameters, string word) 
        {
            foreach (int i in parameters) 
            {
                if (word.Length == i) return true;
            }

            return false;
        }

        public static string SpliceAndFind(string word, string[] wordlist, int startPos = 0) 
        {
            char[] chars = word.ToCharArray();
            toCheck = new string(chars, word.Length - 3 - startPos, 3);
            //Console.WriteLine("To Check: " + toCheck);

            foreach (string s in wordlist) 
            {
                // Select the portion to compare
                string toCompare = s.Substring(0, 3);
                
                // If checker and comparerer matches return result.
                if (toCheck == toCompare) return s; 
            }
          
            return "";
        }

        public static string[] FetchWordlist(int[] acceptedWordLengths)
        {
            // Setup result
            List<string> result = new List<string>();

            // Get all lines of text
            string[] wordlist = File.ReadAllLines("ordliste.txt");

            string lastword = "";
            foreach (string line in wordlist)
            {
                string[] parameters = line.Split("\t");
                string newWord = parameters[1].ToLower();

                bool isWordCorrectLength = WordLengthValidation(acceptedWordLengths, newWord);
                bool isWordValid = WordValidation(newWord);

                // Check if the word is a duplicate and if it contains any illegal chars
                if (lastword.ToLower() != newWord && isWordValid && isWordCorrectLength)
                {
                    result.Add(newWord);
                    lastword = newWord;
                }
            }

            return result.ToArray();
        }
    }
}
