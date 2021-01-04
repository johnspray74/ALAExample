using ProgrammingParadigms;

namespace DomainAbstractions
{
    /// <summary>
    /// Persists an elements value which can be persisted by pushing a value to a key/value register
    /// through the IKeyValueDataFlow port.
    /// --------------------------------------------------------------------------------------------
    /// Ports:
    /// 1. IPersistable child: The persistable child.
    /// 2. IKeyValueDataFlow<string, object> storage: Used to persist the value of the child.
    /// 3. IDataFlow<bool> output: The current value of the child.
    /// </summary>
    class Persist
    {
        // properties
        public string InstanceName = "Default";

        // ports
        private IPersistable child;
        private IKeyValueDataFlow<string, object> storage;
        private IDataFlow<bool> output;

        // private fields
        private string key;

        public Persist(string key)
        {
            this.key = key;
        }

        private void PostWiringInitialize()
        {
            bool state = (bool)storage.Get(key, false);

            child.Selected += Selected;
            child.SetState(state);
            if (output != null) output.Data = state;
        }

        private void Selected(bool selected)
        {
            storage.Set(key, selected);
            if (output != null) output.Data = selected;
        }
    }
}
