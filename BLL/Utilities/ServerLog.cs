using System;
using System.IO;
using System.Configuration;
using System.Text;

namespace BLL.Utilities
{
    public class ServerLog
    {
        private static String ServerLogFile = ConfigurationManager.AppSettings.Get("ServerLogFile");
        private static String ServerLogFileQUA = ConfigurationManager.AppSettings.Get("ServerLogFile_QUA");
        private static String InvalidLoginLogFile = ConfigurationManager.AppSettings.Get("InvalidLoginLogFile");
        private static String ExceptionLogFile = ConfigurationManager.AppSettings.Get("ExceptionLogFile");
        private static String ThemeLogFile = ConfigurationManager.AppSettings.Get("ThemeLogFile");

        public static void Log(String message)
        {
            //FileInfo fi = new FileInfo(ServerLogFile);
            //if (!fi.Exists)
            //    ServerLogFile = "c:\\inetpub\\wwwroot\\SBSPortal\\UIL\\UploadDownload\\log.txt";

            FileStream fs = new FileStream(ServerLogFile, FileMode.Append, FileAccess.Write, FileShare.Write);
            fs.Close();
            StreamWriter sw = new StreamWriter(ServerLogFile, true, Encoding.ASCII);
            sw.WriteLine("-----------------------------------------------------------");
            sw.WriteLine(message);
            sw.WriteLine("-----------------------------------------------------------");
            sw.Close();
        }
        public static void Log_QUA(String message)
        {
            //FileInfo fi = new FileInfo(ServerLogFile);
            //if (!fi.Exists)
            //    ServerLogFile = "c:\\inetpub\\wwwroot\\SBSPortal\\UIL\\UploadDownload\\log.txt";

            FileStream fs = new FileStream(ServerLogFileQUA, FileMode.Append, FileAccess.Write, FileShare.Write);
            fs.Close();
            StreamWriter sw = new StreamWriter(ServerLogFileQUA, true, Encoding.ASCII);
            sw.WriteLine("-----------------------------------------------------------");
            sw.WriteLine(message);
            sw.WriteLine("-----------------------------------------------------------");
            sw.Close();
        }
        public static void InvalidLoginLog(String message)
        {
            FileStream fs = new FileStream(InvalidLoginLogFile, FileMode.Append, FileAccess.Write, FileShare.Write);
            fs.Close();
            StreamWriter sw = new StreamWriter(InvalidLoginLogFile, true, Encoding.ASCII);
            sw.WriteLine(message);
            sw.Close();

        }
        public static void ExceptionLog(String message)
        {
            FileStream fs = new FileStream(ExceptionLogFile, FileMode.Append, FileAccess.Write, FileShare.Write);
            fs.Close();
            StreamWriter sw = new StreamWriter(ExceptionLogFile, true, Encoding.ASCII);
            sw.WriteLine(message);
            sw.Close();

        }
        public static void ThemeLog(String message)
        {
            FileStream fs = new FileStream(ThemeLogFile, FileMode.Append, FileAccess.Write, FileShare.Write);
            fs.Close();
            StreamWriter sw = new StreamWriter(ThemeLogFile, true, Encoding.ASCII);
            sw.WriteLine(message);
            sw.Close();

        }
        public static void Flush()
        {
            File.Delete(ServerLogFile);
        }
    }
}
