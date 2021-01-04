using ProgrammingParadigms;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace DomainAbstractions
{
    /// <summary>
    /// Contains a list of persistable elements and will persist their value
    /// between sessions by pushing an index value to a key/value register through
    /// the IKeyValueDataFlow port.
    /// --------------------------------------------------------------------------
    /// Ports:
    /// 1. List<IPersistable> children: List of children in the list.
    /// 2. IKeyValueDataFlow<string, object> storage: Used to persist the value of the list.
    /// 3. IDataFlow<object> output: The current value of the child.
    /// </summary>
    class PersistList
    {
        // properties
        public string InstanceName = "Default";

        // ports
        private List<IPersistable> children;
        private IKeyValueDataFlow<string, object> storage;
        private IDataFlow<object> output;

        // private fields
        private string key;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key">The key to use when storing the value of the list.</param>
        public PersistList(string key)
        {
            this.key = key;
        }

        /// <summary>
        /// Sets up listeners for the children of the list.
        /// </summary>
        private void PostWiringInitialize()
        {
            object current = storage.Get(key);
            bool compare, defaultSet = false;

            foreach (IPersistable child in children)
            {
                // add click listener
                child.Selected += (bool selected) =>
                {
                    if (selected) Clicked(child);
                };

                // check current settings
                compare = child.Key.Equals(current);
                child.SetState(compare);

                if (compare) defaultSet = true;
            }

            // if no current setting, set first child to be enabled.
            if (! defaultSet)
            {
                children.First()?.SetState(true);
            }

            if (output != null) output.Data = current;
        }

        /// <summary>
        /// Fired when an element is selected.
        /// </summary>
        /// <param name="child">The child element which was pressed.</param>
        private void Clicked(IPersistable child)
        {
            storage.Set(key, child.Key);
            if (output != null) output.Data = child.Key;
        }
    }
}
