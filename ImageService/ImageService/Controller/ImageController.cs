using ImageService.Commands;
using ImageService.ImageService.Commands;
using ImageService.Infrastructure;
using ImageService.Infrastructure.Enums;
using ImageService.Logging;
using ImageService.Modal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Controller
{
    public class ImageController : IImageController
    {
        private IImageServiceModal m_modal;                      // The Modal Object
        private Dictionary<int, ICommand> commands;
        private ILoggingService m_log;

        /// <summary>
        /// the constructor of the class, get IImageServiceModal.
        /// </summary>
        /// <param name="modal">an IImageServiceModal that represent the model object</param>
        public ImageController(IImageServiceModal modal, ILoggingService logger)
        {
            m_modal = modal;                    // Storing the Modal Of The System
            commands = new Dictionary<int, ICommand>();
            m_log = logger;
            
            // For Now will contain NEW_FILE_COMMAND
            commands[(int)(CommandEnum.NewFileCommand)] = new NewFileCommand(m_modal);
            commands[(int)(CommandEnum.GetConfigCommand)] = new GetConfigCommand();
            commands[(int)(CommandEnum.CloseCommand)] = new CloseCommand();
            commands[(int)(CommandEnum.LogCommand)] = new LogCommand(m_log);
            commands[(int)(CommandEnum.LastLogCommand)] = new LastLogCommand(m_log);
        }

        /// <summary>
        /// the method get an Integer, array of strings and a boolean and execute the requested command 
        /// </summary>
        /// <param name="commandID">an Integer that represent the requested command </param>
        /// <param name="args">an array of strings</param>
        /// <param name="resultSuccesful">a boolean that represent if the method succeeded(true) or not(false)</param>
        /// <returns>a string</returns>
        public string ExecuteCommand(int commandID, string[] args, out bool resultSuccesful)
        {
            Task<Tuple<string, bool>> task = new Task<Tuple<string, bool>>(() =>
            {
                bool resultSuccesful1;
                string msg = this.commands[commandID].Execute(args, out resultSuccesful1);
                Tuple<string, bool> arr = Tuple.Create(msg, resultSuccesful1);
                return arr;
            });
            task.Start();
            task.Wait();
            Tuple<string, bool> resultTask = task.Result;
            resultSuccesful = resultTask.Item2;
            return resultTask.Item1;
        }
    }
}
