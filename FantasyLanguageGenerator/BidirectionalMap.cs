namespace FantasyLanguageGenerator
{
    /// <summary>
    /// A bidirectional map that allows for fast lookups in both directions.
    /// Snagged from https://stackoverflow.com/questions/10966331/two-way-bidirectional-dictionary-in-c
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public class BidirectionalMap<TKey, TValue> where TKey : notnull where TValue : notnull
    {
        private readonly Dictionary<TKey, TValue> forward = new Dictionary<TKey, TValue>();

        private readonly Dictionary<TValue, TKey> reverse = new Dictionary<TValue, TKey>();

        public bool Add(TKey key, TValue value)
        {
            if (forward.ContainsKey(key) || reverse.ContainsKey(value))
                return false;

            forward[key] = value;
            reverse[value] = key;
            return true;
        }

        public bool TryGetValueByKey(TKey key, out TValue? value)
        {
            return forward.TryGetValue(key, out value);
        }

        public bool TryGetKeyByValue(TValue value, out TKey? key)
        {
            return reverse.TryGetValue(value, out key);
        }

        public TValue this[TKey key]
        {
            get { return forward[key]; }
        }

        public TKey this[TValue value]
        {
            get { return reverse[value]; }
        }

        public IEnumerable<TKey> Keys
        {
            get { return forward.Keys; }
        }

        public IEnumerable<TValue> Values
        {
            get { return reverse.Keys; }
        }
    }
}