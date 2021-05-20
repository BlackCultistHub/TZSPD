using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1
{
    public static class Thread_form
    {
        public static void runThreadedLogParser(string filename, string log)
        {
            System.Windows.Forms.Application.Run(new Form_parse(filename, log));
        }
    }
}
