using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgrammingParadigms
{
    /// <summary>
    /// Like IEnumerator but is used between ordinary objects instead of LINQ like monads
    /// Where IEnumerator<T> has void MoveNext(), void Reset() and T Current property, Iterator<T> has almost equivalent set of Task<T> Next(), bool Finished(), and void Reset
    /// JRS should a bool Error() function be added, or is it ok to just use exceptions normally?
    /// In future will also have a pushing version, so equivalent of IObserver , has OnNext, OnCompleted, and OnError
    /// </summary>
    /// <typeparam name="T">Generic Type</typeparam>
    public interface IIterator<T>
    {
        /// <summary>
        /// Judge if the iteration finished
        /// </summary>
        bool Finished { get; }

        /// <summary>
        /// Move cursor to point at the next item, and return it if exists
        /// </summary>
        Task<T> Next();

        /// <summary>
        /// Reset the cursor, move it to the beginning before the first item
        /// </summary>
        void Reset();
    }
}
