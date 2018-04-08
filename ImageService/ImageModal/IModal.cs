using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.ImageModal
{
    /// <summary>
    /// 
    /// </summary>
    interface IModal
    {
        /// <summary>
        /// adding a file to the system. 
        /// </summary>
        /// <param name="path"> the path of the location in the system. </param>
        /// <param name="res"> the result of adding a file. </param>
        /// <returns></returns>
        string addFile(string path, out bool res);
    }
}
