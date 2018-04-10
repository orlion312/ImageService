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

        public NewFileCommand(IImageServiceModal modal)
        {
            m_modal = modal;            // Storing the Modal
        }

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
