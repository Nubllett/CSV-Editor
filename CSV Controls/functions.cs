using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CSV_Controls
{
    internal static class functions
    {
        internal static string GetEOL(string data)
        {
            string eol = "";

            if (data.Contains("\r\n")) 
                eol = "\r\n";
            else if (data.Contains('\n')) 
                eol = "\n";
            else 
                eol = "\0";

            return eol;
        }

        internal static string[] SplitEOL(string data)
        {
            string[] arr = null;

            if (data.Contains("\r\n"))
                arr = data.Split(new char[] { '\r', '\n' });
            else if (data.Contains("\n"))
                arr = data.Split('\n');
            else if (data.Contains('\0'))
                arr = data.Split('\0');
            else
            {
                arr = new string[1];
                arr[0] = data;
            }

            return arr;
        }
    }
}
