using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Controller
{
    /// <summary>
    /// 
    /// </summary>
    interface IController
    {
        /// <summary>
        /// Excute the command. 
        /// </summary>
        /// <param name="commandID"> the command id. </param>
        /// <param name="args"> arguments for the  command. </param> 
        /// <param name="res"> boolean parameter for the result. </param>
        /// <returns></returns>
        string ExecuteCommand(int commandID, string[] args,out bool res);
    }
}
