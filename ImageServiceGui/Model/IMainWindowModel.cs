using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageServiceGui.Model
{
    interface IMainWindowModel
    {
        /// <summary>
        /// Change the color of the title.
        /// </summary>
        /// <returns>The string of the color of the title. </returns>
        string Color();
    }
}
