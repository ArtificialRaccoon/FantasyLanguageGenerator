using System.Text;
using System.Text.RegularExpressions;

namespace FantasyLanguageGenerator
{
    public static class Translator
    {
        /// <summary>
        /// Translates a given string from the source language (English, by default) to the target language.
        /// </summary>
        /// <param name="input">A string of words to be translated.</param>
        /// <returns>A string consisting of the translated text.</returns>
        public static string Translate(string input)
        {
            string output = string.Empty;
            List<string> tokens = Regex.Matches(input, @"\w+|[^\w\s]+|\s+").Select(m => m.Value).ToList();            
            foreach (var token in tokens)
            {
                if (Regex.IsMatch(token.ToLower(), @"\w"))
                    output += TranslateWord(token);
                else
                    output += token;
            }
            return output;
        }

        /// <summary>
        /// Translates a single word from the source language (English, by default) to the target language.
        /// This method checks for prefixes, roots, and suffixes in the word and decomposes the word accordingly.
        /// It then translates each part using the provided translation dictionary.
        /// The translator attempts to find the best match for the input word by checking all combinations of prefixes, roots, and suffixes.
        /// </summary>
        /// <param name="input">An input word to be translated</param>
        /// <returns>The input word, translated into the loaded fantasy language.</returns>
        private static string TranslateWord(string input)
        {
            int bestScore = -1;
            string bestPrefix = "";
            string bestSuffix = "";
            string bestRoot = "";            
            string lower = input.ToLower();

            //Found the word in the roots dictionary, return the translation.
            if (Language.Translations["Roots"].Keys.Contains(lower))
                return ApplyCapitalization(input, FindTranslation("Roots", lower));

            //Find the best match for the word
            foreach (string prefix in Language.Translations["Prefixes"].Keys.Append(""))
            {
                if (!lower.StartsWith(prefix))
                    continue;

                string afterPrefix = lower.Substring(prefix.Length);
                foreach (string suffix in Language.Translations["Suffixes"].Keys.Append(""))
                {
                    if (!afterPrefix.EndsWith(suffix))
                        continue;

                    string root = afterPrefix.Substring(0, afterPrefix.Length - suffix.Length);
                    if (string.IsNullOrEmpty(root) || !Language.Translations["Roots"].Keys.Contains(root))
                        continue;

                    int score = root.Length;
                    if (score > bestScore)
                    {
                        bestScore = score;
                        bestPrefix = prefix;
                        bestRoot = root;
                        bestSuffix = suffix;
                    }
                }
            }

            //No possible translation found, return the original word.
            if (bestScore == -1)
                return input;

            //Build the translated word
            string translatedPrefix = bestPrefix != "" ? FindTranslation("Prefixes", bestPrefix) : "";
            string translatedRoot = FindTranslation("Roots", bestRoot);
            string translatedSuffix = bestSuffix != "" ? FindTranslation("Suffixes", bestSuffix) : "";            
            return ApplyCapitalization(input, translatedPrefix + translatedRoot + translatedSuffix); 
        }

        /// <summary>
        /// Finds the translation for a given "word part" (Prefixes, Suffixes, Roots).
        /// </summary>
        /// <param name="category">The word part we wish to translate.</param>
        /// <param name="input">The string literal we are translating</param>
        /// <returns>The translated string / word part.</returns>
        private static string FindTranslation(string category, string input)
        {
            if (Language.Translations.TryGetValue(category, out var translations))
            {
                translations.TryGetValueByKey(input, out var translated);
                if (translated != null)
                    return translated;
            }
            return input;
        }

        /// <summary>
        /// Applies capitalization to the translated word based on the capitalization
        /// of the original word.
        /// </summary>
        /// <param name="original">The source word, including the capitalization.</param>
        /// <param name="translated">The translated word.</param>
        /// <returns>The translated word, with the same capitalization as the original word.</returns>
        private static string ApplyCapitalization(string original, string translated)
        {
            if (string.IsNullOrEmpty(original))
                return translated;
            if (original.All(char.IsUpper))
                return translated.ToUpper();
            if (char.IsUpper(original[0]))
                return char.ToUpper(translated[0]) + translated.Substring(1).ToLower();
            return translated;
        }
    }
}
