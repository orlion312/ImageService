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
    class LastLogCommand:ICommand
    {
        private ILoggingService m_logger;

        //the constructoe of the class
        public LastLogCommand(ILoggingService loggingService)
        {
            this.m_logger = loggingService;
        }

        /// <summary>
        /// Execute the Last log command.
        /// </summary>
        /// <param name="args">The argument that we want to log</param>
        /// <param name="result"> True if we succeed the excute</param>
        /// <returns>string of the log.</returns>
        public string Execute(string[] args, out bool result)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                int size = m_logger.LogsList.Count;
                sb.Append(m_logger.LogsList[size-1].ToJson() + " ; ");
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
