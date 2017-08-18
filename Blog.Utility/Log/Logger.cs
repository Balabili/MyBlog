using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Utility
{
    public class Logger
    {
        private ILog log4net;
        private Logger(Type type)
        {
            var fileInfo = new FileInfo(@"C:\Users\Administrator\Documents\Visual Studio 2015\Projects\Blog\Blog.Utility\Log4Net.config");
            XmlConfigurator.ConfigureAndWatch(fileInfo);
            log4net = LogManager.GetLogger(type);
        }

        public static Logger GetInstance(Type type)
        {
            return new Logger(type);
        }

        public void Debug(string log)
        {
            log4net.Debug(log);
        }

        public void Info(string log)
        {
            log4net.Info(log);
        }

        public void Warn(string log)
        {
            log4net.Warn(log);
        }

        public void Error(string log, Exception e = null)
        {
            log4net.Error(log, e);
        }
    }
}
