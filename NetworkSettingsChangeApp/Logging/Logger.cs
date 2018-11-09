using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace Ozgurtek.Analyst.AnalistBase.Logging
{
    public delegate void LogChangedEventHandler(object sender, LogChangedEventArgs e);
    
    public sealed class Logger
    {
        public event LogChangedEventHandler LogChanged;

        private static readonly object _syncRoot = new Object();
        private static volatile Logger _instance;
        
        private StreamWriter _logWriter;
        private bool _firstTime = true;        
        private bool _userCancelled;
        private bool _systemCancelled;
        
        private Logger() 
        {            
            InitializeLogger();
        }        
       
        private static bool CreateDirectory(string path)
        {
            if (Directory.Exists(path))
                return true;

            try
            {
                Directory.CreateDirectory(path);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private void InitializeLogger()
        {
            string path = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            if ((path != null) && !path.EndsWith("\\"))
                path += "\\";
            path += "log";

            if (!CreateDirectory(path) || !CreateFile(path))
                _systemCancelled = true;    
        }        

        private bool CreateFile(string path)
        {
            string fileName = string.Format(
                "{0:00}{1:00}{2:0000}.log",
                DateTime.Today.Day,
                DateTime.Today.Month,
                DateTime.Today.Year);
            string fullPath = Path.Combine(path, fileName);
            try
            {
                _logWriter = !File.Exists(fullPath) ? new StreamWriter(fullPath) : File.AppendText(fullPath);
                return true;
            }
            catch
            {               
                return false;
            }            
        }
        
        public static Logger Current
        {
            get
            {
                if (_instance == null)
                {
                    lock (_syncRoot)
                    {
                        if (_instance == null)
                            _instance = new Logger();
                    }
                }

                return _instance;
            }
        }

        public bool Recording
        {
            get 
            { 
                return !_systemCancelled && !_userCancelled;
            }
            set 
            {
                _userCancelled = value;   
            }
        }

        public void Log(string line, LogType type)
        {
            if (!Recording)
                return;

            StringBuilder builder = new StringBuilder();
            if (_firstTime)
            {
                builder.Append(Environment.NewLine);
                _firstTime = false;
            }
            DateTime dt = DateTime.Now;
            builder.Append(string.Format("{0} - {1}", type, dt));                       
            builder.Append(Environment.NewLine);
            string replaced = Regex.Replace(line, @"\t|\n|\r", "");
            builder.Append(replaced);
            _logWriter.WriteLine(builder.ToString());
            _logWriter.Flush();
            OnLogChanged(new LogChangedEventArgs(line,dt,type));
        }

        public void LogException(Exception e)
        {
            Log(e.Message + Environment.NewLine + e.StackTrace, LogType.Exception);
        }

        private void OnLogChanged(LogChangedEventArgs e)
        {
            LogChanged?.Invoke(this, e);
        }
    }

    public class LogChangedEventArgs : EventArgs
    {
        public LogChangedEventArgs(string inputString, DateTime time, LogType type)
        {
            this.Log = inputString;
            this.Time = time;
            this.Type = type;
        }
        public string Log { get; }
        public LogType Type { get; }
        public DateTime Time { get; }
    }
}
