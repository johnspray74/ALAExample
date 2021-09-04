using System;
using System.IO;
using System.Diagnostics;

namespace DomainAbstractions
{
    class LoggerToWindow
    {
        public string InstanceName { get; set; } = "anonymous";


        public void WriteLine(string content = "")
        {
            Debug.WriteLine(content);
        }
    }
}