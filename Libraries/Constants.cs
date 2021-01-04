using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Libraries
{
    public static class Constants
    {
        // version control number
        public static string DataLinkPCVersionNumber = "0.201.0.0";

        // HTTP client user-agent
        public static string UserAgent = $"DataLink PC v{DataLinkPCVersionNumber}";

        // SRS2 firmware code == "SRS2\0" in hex => 53 52 53 32 00
        public static string SRS2FirmwareCode = "5352533200";

        // XRS2 firmware code == "XRS2\0" in hex => 58 52 53 32 00
        public static string XRS2FirmwareCode = "5852533200";

        // XRS2/SRS2 microcontroller device ID for programming
        public static string XRS2SRS2DeviceID = "2009";
    }
}
