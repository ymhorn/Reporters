using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reporters
{
    internal class Report
    {
        public string ReporterName { get; set; }
        public string ReportedName { get; set; }
        public string ReportText {  get; set; }
        public DateTime DateTime { get; set; }
    }
}
