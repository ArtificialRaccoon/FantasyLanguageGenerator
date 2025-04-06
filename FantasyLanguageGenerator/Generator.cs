using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FantasyLanguageGenerator
{
    public static class Generator
    {
        #region Private Properties
                /// <summary>
        /// List of prefixes from the source language. (English)
        /// </summary>
        public static List<string> Prefixes = new();

        /// <summary>
        /// List of suffixes from the source language. (English)
        /// </summary>
        public static List<string> Suffixes = new();

        /// <summary>
        /// List of roots from the source language. (English)
        /// </summary>
        public static List<string> Roots = new();

        /// <summary>
        /// List of vowels for use in generating words.
        /// </summary>
        public static List<string> Vowels = new();

        /// <summary>
        /// List of consonants for use in generating words.
        /// </summary>
        public static List<string> Consonants = new();

        /// <summary>
        /// Dictionary of syllable patterns and their weights.
        /// </summary>
        private static Dictionary<string, int> SyllablePatterns = new();

        /// <summary>
        /// Dictionary of consonant pairs and their weights.
        /// </summary>
        private static Dictionary<string, int> ConsonantPairWeights = new();                
        #endregion

        #region Constants
        private static readonly int BasicVowelCount = 5;
        private static readonly int PrefixMinLength = 1;
        private static readonly int PrefixMaxLength = 3;
        private static readonly int SuffixMinLength = 2;
        private static readonly int SuffixMaxLength = 3;        
        private static readonly int ConsonantChance = 60;
        private static readonly int BasicVowelChance = 80;
        private static readonly int MaxConsecutiveConsonants = 2;
        private static readonly float MaxWordLength = 1.5f;
        private static readonly string WordListFilename = "word_lists.json";
        #endregion

        /// <summary>
        /// Static constructor.
        /// </summary>
        static Generator() { }

        /// <summary>
        /// Generates the root form of a word with a given length.
        /// </summary>
        /// <param name="length">The number of characters the translated root should be.</param>
        /// <returns>A new word, in the fantasy langauge, for the input root.</returns>
        private static string GenerateRoot(int length)
        {
            string root = string.Empty;
            while (root.Length < length)
            {
                string syllable = BuildSyllable(Utility.WeightedRandomChoice(SyllablePatterns));
                if (root.Length == 0 || root.Length + syllable.Length <= length)
                    root += syllable;
                else
                    break;
            }
            return root;
        }

        /// <summary>
        /// Generates a syllable based on the given pattern.
        /// The pattern consists of 'C' for consonants and 'V' for vowels.
        /// </summary>
        /// <param name="pattern">A syllable pattern, in the form of the characters C and V in different combinations.</param>
        /// <returns>A new fantasy syllable.</returns>
        private static string BuildSyllable(string pattern)
        {
            string syllable = string.Empty;
            foreach (char c in pattern)
            {
                if (c == 'C')
                    syllable += GetConsonant(syllable == string.Empty ? syllable : syllable.LastOrDefault().ToString());
                else if (c == 'V')
                    syllable += GetVowel();
            }
            return syllable;
        }

        /// <summary>
        /// Generates a consonant based on the previous character.
        /// If there was no previous character, a random consonant is chosen.
        /// If the previous character was a vowel, a random consonant is chosen.
        /// If the previous character was a consonant, a valid consonant pair is chosen based on the weights supplied.
        /// If no valid pair is found, a random consonant is chosen.
        /// </summary>
        /// <param name="previousChar">The previous consonant created (normally why generating a syllable)</param>
        /// <returns>A single consonant (as a string).</returns>
        private static string GetConsonant(string previousChar)
        {
            if (string.IsNullOrEmpty(previousChar))
                return Consonants[Utility.RNG.Next(Consonants.Count)].ToString();

            if (Vowels.Contains(previousChar))
                return Consonants[Utility.RNG.Next(Consonants.Count)].ToString();

            List<string> validPairs = ConsonantPairWeights.Keys.Where(pair => pair.StartsWith(previousChar)).ToList();
            if (validPairs.Count > 0)
            {
                string selectedPair = Utility.WeightedRandomChoice(ConsonantPairWeights, validPairs);
                return selectedPair[1].ToString();
            }

            return Consonants[Utility.RNG.Next(Consonants.Count)].ToString();
        }

        /// <summary>
        /// Generates a vowel; where a "basic" vowel is chosen based on the BasicVowelChance.
        /// "Complex" vowels are chosen from the remaining vowels.
        /// </summary>
        /// <returns>A single vowel (as a string)</returns>
        private static string GetVowel()
        {
            if (Utility.RNG.Next(100) < BasicVowelChance) 
                return Vowels[Utility.RNG.Next(BasicVowelCount)];
            return Vowels[Utility.RNG.Next(BasicVowelCount + 1, Vowels.Count)];
        }

        /// <summary>
        /// Generates a fantasy root word based on a root word.
        /// </summary>
        /// <param name="sourceRoot">The source language root word.</param>
        /// <returns>A new root word in the fantasy langauge, based on the source root word.</returns>
        private static string GenerateFantasyRootWord(string sourceRoot)
        {
            int minLength = sourceRoot.Length;
            int maxLength = (int)(sourceRoot.Length * MaxWordLength);
            int wordLength = Utility.RNG.Next(minLength, maxLength + 1);
            return GenerateRoot(wordLength);
        }

        /// <summary>
        /// Generates an adorner (prefix or suffix) based on the given length range.
        /// An apostrophe can be added to the end of the adorner if specified.
        /// </summary>
        /// <param name="minLength">The minimum length for the adorner</param>
        /// <param name="maxLength">The maximum length for the adorner</param>
        /// <param name="addApostrophe">Add an apostrophy to the end, if the length is 1</param>
        /// <returns></returns>
        private static string GenerateAdorner(int minLength, int maxLength, bool addApostrophe = false)
        {
            int repeatCount = 0;
            int length = Utility.RNG.Next(minLength, maxLength + 1);
            string newChar = string.Empty;
            string lastChar = string.Empty;
            string newAdorner = string.Empty;            

            for (int i = 0; i < length; i++)
            {
                newChar = string.Empty;
                if (Utility.RNG.Next(100) < ConsonantChance)
                {
                    //Check if we are exceeding the max consecutive consonants
                    while (newChar == lastChar && repeatCount >= MaxConsecutiveConsonants)
                    {
                        newChar = Consonants[Utility.RNG.Next(Consonants.Count)];
                    } 
                }
                else
                    newChar = GetVowel();

                //If the new character is the same as the last character, increment the repeat count
                if (newChar == lastChar)
                    repeatCount++;
                else
                    repeatCount = 1;

                newAdorner += newChar;
                lastChar = newChar;
            }

            if (addApostrophe && newAdorner.Length == 1)
                newAdorner += "'";

            return newAdorner;
        }

        /// <summary>
        /// Generates a fantasy language by creating prefixes, suffixes, and roots.
        /// The generated langauge (prefixes, suffixes, and roots) are saved to a JSON file.
        /// </summary>
        /// <param name="outputFilePath">The filepath and name to the output</param>
        public static void GenerateLanguage(string outputFilePath)
        {
            LoadWordsFromJson();
            
            foreach (string prefix in Prefixes)
            {
                string generatedPrefix = GenerateAdorner(PrefixMinLength, PrefixMaxLength, true);
                while(Language.Translations["Prefixes"].TryGetKeyByValue(generatedPrefix, out var existingKey))
                {
                    generatedPrefix = GenerateAdorner(PrefixMinLength, PrefixMaxLength, true);
                }
                Language.Translations["Prefixes"].Add(prefix, generatedPrefix);
            }

            foreach (string suffix in Suffixes)
            {
                string generatedSuffix = GenerateAdorner(SuffixMinLength, SuffixMaxLength);
                while(Language.Translations["Suffixes"].TryGetKeyByValue(generatedSuffix, out var existingKey))
                {
                    generatedSuffix = GenerateAdorner(PrefixMinLength, PrefixMaxLength);
                }                
                Language.Translations["Suffixes"].Add(suffix, generatedSuffix);
            }

            foreach (string root in Roots)
            {
                string generatedFantasyWord = GenerateFantasyRootWord(root);
                while(Language.Translations["Roots"].TryGetKeyByValue(generatedFantasyWord, out var existingKey))
                {
                    generatedFantasyWord = GenerateFantasyRootWord(root);
                } 
                Language.Translations["Roots"].Add(root, generatedFantasyWord);
            }                             

            Language.SaveLanguage(outputFilePath);
        }

        /// <summary>
        /// Loads the word list file.  This contains the prefixes, suffixes, roots, vowels, consonants, syllable patterns,
        /// and consonant pair weights for the basis of the fantasy language generation. 
        /// </summary>
        private static void LoadWordsFromJson()
        {
            if (!File.Exists(WordListFilename))
                Utility.ExtractEmbeddedResource("FantasyLanguageGenerator.Resources.word_lists.json", WordListFilename);        

            string json = File.ReadAllText(WordListFilename);
            Dictionary<string, JToken>? jsonData = JsonConvert.DeserializeObject<Dictionary<string, JToken>>(json);

            if (jsonData == null)
                throw new Exception("Failed to load JSON data.");

            Prefixes = jsonData["Prefixes"]?.ToObject<List<string>>() ?? new();
            Suffixes = jsonData["Suffixes"]?.ToObject<List<string>>() ?? new();
            Roots = jsonData["Roots"]?.ToObject<List<string>>() ?? new();
            Vowels = jsonData["Vowels"]?.ToObject<List<string>>() ?? new();
            Consonants = jsonData["Consonants"]?.ToObject<List<string>>() ?? new();
            SyllablePatterns = jsonData["SyllablePatterns"]?.ToObject<Dictionary<string, int>>() ?? new();
            ConsonantPairWeights = jsonData["ConsonantPairWeights"]?.ToObject<Dictionary<string, int>>() ?? new();
        }
    }
}