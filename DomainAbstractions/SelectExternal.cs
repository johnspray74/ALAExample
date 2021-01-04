using ProgrammingParadigms;
using System;
using System.Threading.Tasks;

namespace DomainAbstractions
{
    /// <summary>
    /// Like Select, but with two differences:
    /// 1) it takes a ITerator<t1> and maps to IIterator<T2> (instead of ITableDataFlow which handles dynamic columns)
    /// 2) instead of using a lambda expression to do the mapping, you can wire to external abstractions to do the mapping.
    /// There is an IDataflow<T1> output port, and a input port of type T2
    /// For now just support T2 being ITableDataflow
    /// </summary>
    public class SelectExternal : IIterator<ITableDataFlow>

    {
        // properties
        public string InstanceName = "Default";

        // outputs
        private IIterator<Tuple<string, string>> iterator;
        private ITableDataFlow tableDataFlow;
        private IDataFlow<string> dataFlowFileName;

        // IIterator<ITableDataFlow> implementation -------------------------------------------
        bool IIterator<ITableDataFlow>.Finished => iterator.Finished;

        async Task<ITableDataFlow> IIterator<ITableDataFlow>.Next()
        {
            Tuple<string, string> tuple = await iterator.Next();
            dataFlowFileName.Data = tuple.Item1;
            return tableDataFlow;
        }

        void IIterator<ITableDataFlow>.Reset()
        {
            iterator.Reset();
        }
    }
}
