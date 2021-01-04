namespace ProgrammingParadigms
{
    /// <summary>
    /// Defines a data flow in which a "client" can get and set values by key.
    /// Examples may include REDIS, JSON, database tables etc;
    /// The key and value types are constant but this could be abstracted.
    /// </summary>
    /// <typeparam name="K">The type of the key.</typeparam>
    /// <typeparam name="V">The type of the value.</typeparam>
    public interface IKeyValueDataFlow<K, V>
    {
        /// <summary>
        /// Gets a given key. Takes a default value to be returned if the
        /// key does not exist, or another error occurs.
        /// </summary>
        /// <param name="key">The key to search for.</param>
        /// <param name="defaultValue">The default value to return.</param>
        /// <returns></returns>
        V Get(K key, V defaultValue = default);

        /// <summary>
        /// Sets a given key with a given value.
        /// </summary>
        /// <param name="key">The key to set.</param>
        /// <param name="value">The value to set the key to.</param>
        void Set(K key, V value);
    }
}
