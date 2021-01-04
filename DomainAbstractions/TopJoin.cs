using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLink_ALA.DomainAbstractions
{
    //TODO: add summary and code

    /// <summary>
    /// Has two ITableDataflow interfaces on the input side and one ITableDataflow interface on the output side.
    /// For each row in the top input table, it joins the fields from the bottom input using the appropriate FK of the top table and the PK of the bottom table.
    /// </summary>
    
    class TopJoin
    {
    }
}
