using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLink_ALA.DomainAbstractions
{
    //TODO: add summary and code

    /// <summary>
    /// Reads a table of values out of the device using SCP commands.
    /// Produces and ITableDataflow interface (with a single column) with one row for each value read from the device.
    /// To read the values from the device, first send the first SCP command to get the number of values.
    /// Then send the second SCP command with an indexes from 0 up to the said number of values minus 1.
    /// e.g. { STN} returns[3], then {STXX0}, {STXX1}, {STXX2}, and the device will respond with a strings values in square brackets e.g. [string].
    /// Send these commands one at a time. To set the value in the device, send the SCP command with the index and value e.g. {STXX0,string}
    ///TableSCP must be triggered by a TransactLeft or TransactRight downstream of the ITableDataflow interface.
    /// </summary>

    class SettingSCP
    {
    }
}
