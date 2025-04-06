using System.Reflection;

namespace FantasyLanguageGenerator
{
    public static class Utility
    {
        /// <summary>
        /// Random number generator...
        /// </summary>
        public static Random RNG = new Random();

        /// <summary>
        /// Static constructor
        /// </summary>
        static Utility() { }

        /// <summary>
        /// Randomly selects a string from a dictionary. 
        /// The dictionary contains weighted values, which are used to determine the likelihood of each string being selected.
        /// </summary>
        /// <param name="weightedChoices"></param>
        /// <returns>A string literal; currently a consonant / vowel pattern.</returns>
        public static string WeightedRandomChoice(Dictionary<string, int> weightedChoices)
        {
            int cumulative = 0;
            int totalWeight = weightedChoices.Values.Sum();
            int randomValue = RNG.Next(totalWeight);            
            foreach (var entry in weightedChoices)
            {
                cumulative += entry.Value;
                if (randomValue < cumulative)
                    return entry.Key;
            }
            return weightedChoices.First().Key ?? string.Empty;
        }

        /// <summary>
        /// Randomly selects a string from a dictionary.
        /// The dictionary contains weighted values, which are used to determine the likelihood of each string being selected.
        /// The list of valid choices is used to filter the dictionary, ensuring that only the specified choices are considered.
        /// </summary>
        /// <param name="weightedChoices"></param>
        /// <param name="validChoices"></param>
        /// <returns></returns>
        public static string WeightedRandomChoice(Dictionary<string, int> weightedChoices, List<string> validChoices)
        {
            int cumulative = 0;
            int totalWeight = validChoices.Sum(pair => weightedChoices[pair]);
            int randomValue = RNG.Next(totalWeight);        
            foreach (var pair in validChoices)
            {
                cumulative += weightedChoices[pair];
                if (randomValue < cumulative)
                    return pair;
            }
            return validChoices.FirstOrDefault() ?? string.Empty;
        }

        /// <summary>
        /// Extracts an embedded resource from the assembly and saves it to a specified output path.
        /// </summary>
        /// <param name="resourceName"></param>
        /// <param name="outputPath"></param>
        public static void ExtractEmbeddedResource(string resourceName, string outputPath)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            Stream? resourceStream = assembly.GetManifestResourceStream(resourceName);
            if (resourceStream == null)
            {
                Console.WriteLine("Error - Resource not found: " + resourceName);
                Environment.Exit(1);
            }

            using (resourceStream)
            {
                using (FileStream fileStream = new FileStream(outputPath, FileMode.Create, FileAccess.Write))
                {
                    resourceStream.CopyTo(fileStream);
                }
            }
        }
    }
}