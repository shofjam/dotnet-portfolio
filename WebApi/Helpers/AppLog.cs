namespace WebApi.Helpers
{
    using System.Linq;
    using System;

    public class AppLog
    {
        public static void Write(string message, string fileName = null)
        {
            string strDateTime;

            //Cek dan delete old file
            DeleteOldFile();

            using (System.IO.StreamWriter sw = System.IO.File.AppendText($"AppLog\\{(string.IsNullOrWhiteSpace(fileName) ? "Exception" : fileName)}_{DateTime.Now.ToString("dd-MM-yyyy")}.txt"))
            {
                try
                {
                    strDateTime = string.Format(System.Globalization.CultureInfo.CreateSpecificCulture("en-US"),
                        "[{0:dd-MMM-yyyy}, {0:HH:mm:ss}]", DateTime.Now);

                    if (!string.IsNullOrEmpty(message))
                    {
                        message += System.Environment.NewLine;
                        sw.WriteLine("{0} : {1}", strDateTime, message);
                    }

                    sw.Flush();
                    sw.Close();
                }
                catch
                {

                }
            }

        }

        public static void DeleteOldFile()
        {
            try
            {
                System.IO.DirectoryInfo dirInfo = new System.IO.DirectoryInfo("AppLog");
                DateTime dtFileToDelete = DateTime.Now.AddDays(-3);

                if (!dirInfo.Exists)
                {
                    dirInfo.Create();
                    dirInfo = new System.IO.DirectoryInfo("AppLog");
                }

                var dirs = from dir in dirInfo.GetFiles()
                           select dir;
                foreach (var afile in dirs)
                {
                    if (afile.LastWriteTimeUtc < dtFileToDelete)
                    {
                        System.Diagnostics.Debug.WriteLine(afile.Name + " " + afile.LastWriteTimeUtc);
                        afile.Delete();
                    }
                }

            }
            catch //(Exception ex)
            {
                //System.Diagnostics.Debug.WriteLine("ErrorLog error:\r\n" + ex.Message);
            }
        }
    }
}
