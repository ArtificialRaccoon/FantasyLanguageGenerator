namespace FantasyLanguageGenerator
{
    public static class Language
    {
        /// <summary>
        /// Translation dictionary for the source language (English) to the target language.
        /// </summary>
        public static Dictionary<string, BidirectionalMap<string, string>> Translations = new Dictionary<string, BidirectionalMap<string, string>>();

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
    }
}