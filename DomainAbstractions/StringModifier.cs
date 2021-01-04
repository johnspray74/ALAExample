using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProgrammingParadigms;
using System.Text.RegularExpressions;

namespace DomainAbstractions
{
    public delegate string StringModifierDelegate(string s);

    /// <summary>
    /// [DEPRECATED: can be replaced by Apply&lt;string, string&gt;]
    /// <para>Applies a lambda to a string input and outputs the result.</para>
    /// <para>Ports:</para>
    /// <para>1. IDataFlow&lt;string&gt; inputString: the input string</para>
    /// <para>2. IDataFlow&lt;string&gt; stringOutput: the output string</para>
    /// </summary>
    public class StringModifier : IDataFlow<string> // inputString
    {
        // Properties
        public string InstanceName = "Default";
        public StringModifierDelegate Lambda;

        // Ports
        private IDataFlow<string> stringOutput;

        /// <summary>
        /// <para>Applies a lambda to a string input and outputs the result.</para>
        /// </summary>
        public StringModifier() { }

        // IDataFlow<string> implementation
        string IDataFlow<string>.Data
        {
            set
            {
                string s = Lambda(value);
                if (stringOutput != null) stringOutput.Data = s;
            }
        }
    }
}
