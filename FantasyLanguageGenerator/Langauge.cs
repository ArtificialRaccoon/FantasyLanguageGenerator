using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace FantasyLanguageGenerator
{
    public static class Language
    {
        /// <summary>
        /// Translation dictionary for the source language (English) to the target language.
        /// </summary>
        public static Dictionary<string, BidirectionalMap<string, string>> Translations = new Dictionary<string, BidirectionalMap<string, string>>();

        public static HashSet<string> UntranslatedWords = new HashSet<string>();

        /// <summary>
        /// Static Constructor to initialize the Translations dictionary.
        /// </summary>
        static Language()
        {
            Translations = new Dictionary<string, BidirectionalMap<string, string>>
            {
                { "Prefixes", new BidirectionalMap<string, string>() },
                { "Suffixes", new BidirectionalMap<string, string>() },
                { "Roots", new BidirectionalMap<string, string>() }
            };
        }

        public static void LoadLanguage(string filePath)
        {
            string jsonText = File.ReadAllText(filePath);
            JObject jsonData = JObject.Parse(jsonText);

            foreach (KeyValuePair<string, BidirectionalMap<string, string>> category in Translations)
            {
                JToken? section = jsonData[category.Key];
                if (section != null)
                {
                    JArray? keys = section["Keys"] as JArray;
                    JArray? values = section["Values"] as JArray;

                    if (keys != null && values != null && keys.Count == values.Count)
                    {
                        for (int i = 0; i < keys.Count; i++)
                        {
                            string key = keys[i]?.ToString() ?? string.Empty;
                            string value = values[i]?.ToString() ?? string.Empty;
                            category.Value.Add(key, value);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Saves the generated language to a JSON file.
        /// </summary>
        /// <param name="filePath">The output path to the generated json</param>
        public static void SaveLanguage(string filePath)
        {
            File.WriteAllText(filePath, JsonConvert.SerializeObject(Translations, Formatting.Indented));
        }
    }
}