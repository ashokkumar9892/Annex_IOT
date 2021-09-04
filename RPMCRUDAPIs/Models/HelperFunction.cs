using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace RPMCRUDAPIs.Models
{
    public class HelperFunction
    {
        public static void CreateLogs(string header, string values, string logType)
        {
            //string Path = ConfigurationManager.AppSettings["RequestLogDirectoryPath"].ToString();
            string Path = "D:/Annex_StripePayment";
            string filepath = (logType == "Error" ? Path + "ErrorLogs.txt" : Path + "logs.txt");
            if (!Directory.Exists(Path))
            {
                Directory.CreateDirectory(Path);

            }
            if (!File.Exists(filepath))
            {
                File.Create(filepath).Dispose();
            }
            if (logType == "Error")
            {
                using (StreamWriter sw = File.AppendText(filepath))
                {
                    string error = "Error Log Written Date:" + " " + DateTime.Now.ToString();
                    sw.WriteLine(error);
                    sw.WriteLine("---");
                    sw.WriteLine(values);
                    sw.WriteLine("---");
                    sw.Flush();
                    sw.Close();

                }
            }
            else
            {
                using (System.IO.StreamWriter w = new System.IO.StreamWriter(filepath, true))
                {
                    w.WriteLine("Log Written Date:" + " " + DateTime.Now.ToString());
                    w.Write(header + ": ");
                    w.WriteLine(values);
                    w.WriteLine("------------------------------------------------------------------");
                    w.Flush();
                    w.Close();
                }
            }

        }
    }
}
