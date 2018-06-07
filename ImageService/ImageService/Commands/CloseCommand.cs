using ImageService.Commands;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.ImageService.Commands
{
    public class CloseCommand : ICommand
    {

        public string Execute(string[] args, out bool result)
        {
            try
            {
                if(args.Length != 1)
                {
                    throw new Exception("too many args to remove");
                }
                string[] handlers = ConfigurationManager.AppSettings.Get("Handler").Split(';');
                StringBuilder newHandlers = new StringBuilder();
                foreach (string handler in handlers)
                {
                    if(String.Compare(handler, args[0]) != 0)
                    {
                        newHandlers.Append(handler + ';');
                    }
                }
                ConfigurationManager.AppSettings.Set("Handler", newHandlers.ToString());
                result = true;
                return "";
            } catch (Exception e)
            {
                result = false;
                return e.ToString();

            }
        }
    }
}
