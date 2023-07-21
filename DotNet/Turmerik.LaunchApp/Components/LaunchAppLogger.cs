using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Turmerik.LaunchApp.Components
{
    public class LaunchAppLogger
    {
        static readonly string loggerDirName = string.Join(".",
            AssmbH.AssemblyName,
            nameof(LaunchAppLogger));

        private static readonly string NL = Environment.NewLine;

        private LaunchAppLogger()
        {
            Directory.CreateDirectory(loggerDirName);
        }

        public static Lazy<LaunchAppLogger> Instance { get; } = new Lazy<LaunchAppLogger>(() => new LaunchAppLogger());

        public AppLogLevel MinLogLevel { get; set; }

        public void Write(
            AppLogLevel level,
            string message,
            bool encodeXml = false)
        {
            if (level < MinLogLevel)
            {
                return;
            }

            var timeStamp = DateTime.UtcNow;

            string logMsgText = GetLogMsgText(
                level,
                message,
                timeStamp,
                encodeXml);

            string logFileName = GetLogFileRelPath(
                timeStamp);

            File.AppendAllText(logFileName, logMsgText);
        }

        public void Debug(
            string message,
            bool encodeXml = false) => Write(
                AppLogLevel.Debug,
                message,
                encodeXml);

        public void Information(
            string message,
            bool encodeXml = false) => Write(
                AppLogLevel.Information,
                message,
                encodeXml);

        public void Warning(
            string message,
            bool encodeXml = false) => Write(
                AppLogLevel.Warning,
                message,
                encodeXml);

        public void Error(
            string message,
            bool encodeXml = false) => Write(
                AppLogLevel.Error,
                message,
                encodeXml);

        public string EncodeXml(string message) => message.Replace(
            "&", "&amp;").Replace(
            "<", "&lt;").Replace(
            ">", "&gt;");

        public string EncodeMsg(string message) => message.Replace("<<<<", "<<<<>>>>");

        private string GetLogMsgText(
            AppLogLevel level,
            string message,
            DateTime timeStamp,
            bool encodeXml)
        {
            if (encodeXml)
            {
                message = EncodeXml(message);
            }
            else
            {
                message = EncodeMsg(message);
            }
            
            string levelStr = level.ToString().ToUpper();
            string timeStampStr = timeStamp.ToString("yyyy-MM-dd HH:mm:ss.FFFFFFFK");

            string logMsgText = $"<<<< [{timeStampStr}] - [{levelStr}] >>>>{NL}{message}{NL}";
            return logMsgText;
        }

        private string GetLogFileRelPath(
            DateTime timeStamp)
        {
            string fileNameDateStr = timeStamp.ToString("yyyy-MM-dd");
            string logFileName = $"log-{fileNameDateStr}.txt";

            string logFilePath = Path.Combine(
                loggerDirName,
                logFileName);

            return logFilePath;
        }
    }

    public enum AppLogLevel
    {
        Debug,
        Information,
        Warning,
        Error
    }
}
