using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Translator
{
    class Program
    {
        private static char[] Vowel = { 'a', 'e', 'i', 'o', 'u', 'y', 'A', 'E', 'I', 'O', 'U', 'Y' };
        private static char[] Punctuation = { ',', '!', '?' };

        // False = use yay for words that begin with vowels, True = only for words without consanants
        private static bool Ignite = false;

        static void Main(string[] args)
        {
            Dictionary<char, int> vowelDictionary = createDictionary(Vowel);
            Dictionary<char, int> punctuationDictionary = createDictionary(Punctuation);

            string[] lineArray = processInput(Console.ReadLine());

            while (!checkForExit(lineArray))
            {
                // holds the entire line
                string result = "";
                if (!lineArray[0].Equals("", StringComparison.Ordinal))
                {
                    for (int i = 0; i < lineArray.Length; i++)
                    {
                        // grab one word at a time to translate
                        string word = lineArray[i];

                        // partial strings used to form the result
                        string prefix = "";
                        string stem = "";
                        string ay = "ay";
                        string punctuation = "";

                        // preserve punctions that exist in the hash
                        char lastChar = word[word.Length - 1];
                        bool hasPunctuation = isPunctuation(lastChar, punctuationDictionary);
                        if (hasPunctuation)
                        {
                            word = word.Substring(0, word.Length - 1);
                            punctuation = lastChar.ToString();
                        }

                        int charCount = Regex.Matches(word, @"[a-zA-Z]").Count;
                        int consanantCount = Regex.Matches(word, @"[b-df-hj-np-tv-xz]", RegexOptions.IgnoreCase).Count;
                        if (Ignite && charCount == 0)
                        {
                            ay = "";
                        }
                        else if (consanantCount == 0)   // case where if no consonants append yay
                        {

                            stem = word;
                            ay = "yay";
                        }
                        else
                        {
                            bool firstUpper = char.IsUpper(word, 0);
                            word = word.ToLower();

                            // save up to but not including the first vowel in the prefix
                            for (int j = 0; j < word.Length; j++)
                            {
                                // save the first vowel and rest of the word in the stem
                                if (isVowel(word[j], vowelDictionary))
                                {
                                    if (firstUpper)
                                    {
                                        stem = Char.ToUpper(word[j]) + word.Substring(j + 1);
                                    }
                                    else
                                    {
                                        stem = word.Substring(j);
                                    }

                                    // used to determine if is a word that begins with a vowel
                                    if (!Ignite && charCount != 0 && String.Equals(word, stem.ToLower(), StringComparison.Ordinal))
                                    {
                                        ay = "yay";
                                    }

                                    break;
                                }

                                prefix += word[j];
                            }
                        }

                        //Console.WriteLine("Word: " + word);
                        //Console.WriteLine("Stem: " + stem);
                        //Console.WriteLine("Prefix " + prefix);

                        result += (stem + prefix + ay + punctuation + " ");
                    }
                }

                result = result.TrimEnd();

                Console.WriteLine(result);

                lineArray = processInput(Console.ReadLine());
            }

        }

        // fill the dictionary from defined arrays.
        static Dictionary<char, int> createDictionary(char[] foo)
        {
            Dictionary<char, int> fooDictionary = new Dictionary<char, int>();
            for (int i = 0; i < foo.Length; i++)
            {
               fooDictionary.Add(foo[i], 1);
            }

            return fooDictionary;
        }

        static bool isPunctuation(char c, Dictionary<char, int> punctuationDictionary)
        {
            return punctuationDictionary.ContainsKey(c);
        }

        static bool isVowel(char c, Dictionary<char, int> vowelDictionary)
        {
            return vowelDictionary.ContainsKey(c);
        }

        static string[] processInput(String line)
        {
            line = Regex.Replace(line, @"\t |\n |\r", string.Empty);
            line = Regex.Replace(line.TrimEnd(), @"[\s]+", " ");

            return line.Split(' ');
        }

        // assuming only if just exit is typed
        static bool checkForExit(string[] line)
        {
            if (line[0].Equals("exit", StringComparison.Ordinal) && line.Length == 1)
            {
                return true;
            }

            return false;
        }
    }

}
