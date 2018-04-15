using ImageService.Commands;
using ImageService.Infrastructure;
using ImageService.Infrastructure.Enums;
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

        /// <summary>
        /// the constructor of the class, get IImageServiceModal.
        /// </summary>
        /// <param name="modal">an IImageServiceModal that represent the model object</param>
        public ImageController(IImageServiceModal modal)
        {
            m_modal = modal;                    // Storing the Modal Of The System
            commands = new Dictionary<int, ICommand>();
            
            // For Now will contain NEW_FILE_COMMAND
            commands[(int)(CommandEnum.NewFileCommand)] = new NewFileCommand(m_modal); 
            
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
            Task<string[]> task = new Task<string[]>(() =>
            {
                bool resultSuccesful1;
                string msg = this.commands[commandID].Execute(args, out resultSuccesful1);
                string[] arr = { msg, resultSuccesful1.ToString() };
                return arr;
            });
            task.Start();
            if (task.Result[1] == "true")
            {
                resultSuccesful = true;
            }
            else
            {
                resultSuccesful = false;
            }
            return task.Result[0];
        }
    }
}
