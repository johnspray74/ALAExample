using Newtonsoft.Json.Linq;
using ProgrammingParadigms;
using System;
using System.IO;

namespace DomainAbstractions
{
    /// <summary>
    /// Reads and writes from a JSON file. Operates with a key and value type.
    /// TODO: abstract to allow mixed key types, as long as it is within
    /// the JValue requirements.
    /// ----------------------------------------------------------------------
    /// 1. IEvent NEEDNAME: Saves the file.
    /// 2. IKeyValueDataFlow<string, object> NEEDNAME: Used to get and set values.
    /// 3. IEvent fileLoaded: fired when the file has been loaded.
    /// </summary>
    class JsonReaderWriter : IEvent, IKeyValueDataFlow<string, object>
    {
        // properties
        public string InstanceName = "Default";
        public string FilePath { get => filePath; set { filePath = value; OpenFile(); } }

        // ports
        private IEvent fileLoaded;

        // private fields
        private string filePath;
        private bool saveOnUpdate;
        private JObject content;

        /// <summary>
        /// Reads and writes from a JSON file. Operates with a key and value type.
        /// TODO: abstract to allow mixed key types, as long as it is within
        /// the JValue requirements.
        /// </summary>
        /// <param name="filePath">Path to the JSON file.</param>
        /// <param name="saveOnUpdate">Whether the JSON file should be saved when a key is set.</param>
        public JsonReaderWriter(string filePath = default, bool saveOnUpdate = false)
        {
            this.filePath = filePath;
            this.saveOnUpdate = saveOnUpdate;

            OpenFile();
        }

        /// <summary>
        /// Opens the given filepath and parses the JSON object into memory.
        /// </summary>
        private async void OpenFile()
        {
            if (filePath == default) return;

            using (StreamReader reader = new StreamReader(filePath))
            {
                content = JObject.Parse(await reader.ReadToEndAsync());
            }

            fileLoaded?.Execute();
        }

        /// <summary>
        /// Saves the JSON object at the given filepath.
        /// </summary>
        private async void SaveFile()
        {
            if (filePath == default || content == default) return;

            using (StreamWriter writer = new StreamWriter(filePath))
            {
                await writer.WriteAsync(content.ToString());
            }
        }

        // IEvent implementation
        void IEvent.Execute() => SaveFile();

        // IKeyValueDataFlow implementation
        object IKeyValueDataFlow<string, object>.Get(string key, object defaultValue = default)
        {
            JToken token = content[key];

            if (token == null) return defaultValue;

            try
            {
                return token.ToObject<object>();
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        void IKeyValueDataFlow<string, object>.Set(string key, object value)
        {
            content[key] = new JValue(value);
            if (saveOnUpdate) SaveFile();
        }
    }
}
