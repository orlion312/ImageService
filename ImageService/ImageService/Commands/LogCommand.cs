using ImageService.Commands;
using ImageService.Infrastructure.Enums;
using ImageService.Logging;
using ImageService.Logging.Modal;
using ImageService.Modal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.ImageService.Commands
{
    class LogCommand:ICommand
    {
        private ILoggingService m_logger;

        //the constructor of thr class
        public LogCommand(ILoggingService loggingService)
        {
            this.m_logger = loggingService;
        }

        /// <summary>
        /// Execute the Log command.
        /// </summary>
        /// <param name="args">The parametrs value of the logs</param>
        /// <param name="result"> True if we succeed the excute</param>
        /// <returns>string of all the logs.</returns>
        public string Execute(string[] args, out bool result)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                foreach (MessageRecievedEventArgs item in m_logger.LogsList)
                {
                    sb.Append(item.ToJson() + " ; ");
                }
                result = true;
                string[] arguments = new string[1];
                arguments[0] = sb.ToString();
                CommandRecievedEventArgs c = new CommandRecievedEventArgs((int)CommandEnum.LogCommand, arguments, null);
                return c.ToJson();
            }
            catch (Exception e)
            {
                result = false;
                return e.ToString();
            }

        }
    }
}
