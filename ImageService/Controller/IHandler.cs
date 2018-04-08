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
    interface IHandler
    {
        event EventHandler
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Path"></param>
        void startHandler(string path);

        void OnRecieve(object sender, Command )

    }
}
