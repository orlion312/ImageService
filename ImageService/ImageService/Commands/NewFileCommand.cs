using ImageService.Infrastructure;
using ImageService.Modal;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Commands
{
    public class NewFileCommand : ICommand
    {
        private IImageServiceModal m_modal;

        /// <summary>
        /// the constructor of the class, get a Imodel
        /// </summary>
        /// <param name="modal">an object from IImageServiceModal that storing the model</param>
        public NewFileCommand(IImageServiceModal modal)
        {
            m_modal = modal;            // Storing the Modal
        }

        /// <summary>
        /// the method get an array of strings and a boolean, retuen the new path and the boolean
        /// </summary>
        /// <param name="args">an array of strings that represent the file</param>
        /// <param name="result">a boolean that represent if the method succesed(true) or not(false)</param>
        /// <returns>a string that represent the new path</returns>
        public string Execute(string[] args, out bool result)
        {
            try
            {
                // The String Will Return the New Path if result = true, and will return the error message
                if (args.Length == 0)
                {
                    throw new Exception("args is empty");
                } else if (File.Exists(args[0]))
                {
                    return m_modal.AddFile(args[0], out result);
                }
                result = true;
                return args[0].ToString();
            
            } catch (Exception e)
            {
                result = false;
                return e.ToString();
            }
        }
    }
}
