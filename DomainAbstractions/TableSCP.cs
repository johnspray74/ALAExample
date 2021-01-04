using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLink_ALA.DomainAbstractions
{
    //TODO: add summary and code

    /// <summary>
    /// Knows how to send SCP commands to the device for settings that have a single parameter.The parameter is an integer (0..n)
    /// To read the value from the device, just send the SCP command e.g. { STXX } and the device will respond with a number in square brackets e.g. [2]
    ///To set the value in the device, send the SCP command with the value concatenated.e.g. {STXX2}
    ///SettingSCP has a port that triggers reading of the setting from the device.
    /// </summary>

    class TableSCP
    {
    }
}
