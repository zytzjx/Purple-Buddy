using PurpleBuddy.Primitive.Commands;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PurpleBuddy.Mobile
{
    public class ConsoleOutput
    {

        // Commands and binary names.

        Essentials Helper = new Essentials();


        public string displayEasyMobileGestaltKeyValue(string udid, string keyvalue)
        {
            StringBuilder parser = new StringBuilder();
            
            using (Process process = new Process())
            {                
                process.StartInfo.FileName                = $"{Helper.BinaryPath}" + "\\" + $"{Helper.DeviceDiagnostics}";
                process.StartInfo.Arguments               = "-u " + udid + " mobilegestalt " + keyvalue;
                process.StartInfo.CreateNoWindow          = true;
                process.StartInfo.UseShellExecute         = false;
                process.StartInfo.RedirectStandardOutput  = true;
                process.StartInfo.RedirectStandardError   = true;
                process.Start();
                
                while (!process.StandardOutput.EndOfStream)
                {
                    string line = process.StandardOutput.ReadLine();

                    if (line.Contains("<string>"))
                    {

                        if (line.Contains("Success"))

                            continue;

                        parser.Append(line);
                    }
                }
            }
            return parser.ToString().Split('>')[1].Split('<')[0];
            
        }
    }

}
