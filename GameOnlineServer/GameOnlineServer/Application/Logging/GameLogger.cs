using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOnlineServer.Application.Logging
{
    public class GameLogger : IGameLogger
    {
        public readonly ILogger logger;
        public GameLogger()
        {
            logger = new LoggerConfiguration()
                .WriteTo.Console(outputTemplate: "{Timestamp:dd-MM-yyyy HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
                .WriteTo.File("logging/log.txt",LogEventLevel.Error,outputTemplate: "{Timestamp:dd-MM-yyyy HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}",rollingInterval: RollingInterval.Day)
                .CreateLogger();
        }
        public void Error(string msg)
        {
            logger.Error(msg);
        }

        public void Error(string msg, Exception ex)
        {
            logger.Error(msg, ex);
        }

        public void Info(string msg)
        {
            logger.Information(msg);
        }

        public void Print(string msg)
        {
            logger.Information(">>> " + msg);
        }

        public void Warning(string msg, Exception ex = null)
        {
            logger.Warning(msg, ex);
        }
    }
}
