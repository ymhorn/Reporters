using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reporters
{
    //Shell for report
    internal class Report
    {
        //Name of the reporter
        public string ReporterName { get; set; }

        //Name of the reported
        public string ReportedName { get; set; }

        //The report itself
        public string ReportText {  get; set; }

        //Date and time of the report
        public DateTime DateTime { get; set; }
    }
}
