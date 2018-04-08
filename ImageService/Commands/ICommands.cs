using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Commands
{
    /// <summary>
    /// 
    /// </summary>
    interface ICommands
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"> arguments for the command. </param>
        /// <param name="result"> the result of the execute. </param>
        /// <returns></returns>
        string execute(string[] args, out bool result);
    }
}
