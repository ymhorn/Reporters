using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Reporters
{
    internal static class InputReport
    {
        public static Report input()
        {
            Report report = new Report();
            Console.WriteLine("");
            report.ReporterName = Console.ReadLine();
            Console.WriteLine("");
            report.ReportedName = Console.ReadLine();
            Console.WriteLine("");
            report.ReportText = Console.ReadLine();
            report.DateTime = DateTime.Now;

            return report;
        }
    }
}
