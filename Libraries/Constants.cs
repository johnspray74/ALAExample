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
        // this must comply with the format that is listed on the Tru-Test website - they must have the same
        // number of decimal points and there shouldn't be any tags at the end e.g. `1.0.0.0-beta`
        // at the moment most products have their version as `major.minor.patch.svn_revision`
        // check out CompareFirmwareVersions to see the logic for this and change as required
        public static readonly Dictionary<string, string> FilterTypes = new Dictionary<string, string>()
        {
            { "CSV", "Datamars CSV File (*.csv)|*.csv" },
            { "CSV No Header", "Datamars CSV File No Header (*.csv)|*.csv" },
            { "CSV 3000 Format", "Datamars CSV File 3000 format (*.csv)|*.csv" },
            { "CSV Minda Format", "Datamars CSV File Minda format (*.csv)|*.csv" },
            { "CSV EID only", "Datamars CSV File EID Only (*.csv)|*.csv" },
            { "Excel 97-2003", "Microsoft Excel 97-2003 Worksheet (*.xls)|*.xls" },
            { "Excel Worksheet", "Microsoft Excel Worksheet (*.xlsx)|*.xlsx" },
            { "Excel 97-2003 from Template", "Microsoft Excel 97-2003 Worksheet from Template File (*.xls)|*.xls" },
            { "Datamars XML", "Datamars XML file (*.xml)|*.xml" },
            { "Tru-Test Favourite", "Tru- Test favourite file (*.ttfav)|*.ttfav" },
            { "All files", "All files(*.*)|*.*" }
        };
    }
}
