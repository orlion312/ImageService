using ImageServiceGui.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageServiceGui.VM
{
    class VM_MainWindow
    {
        private IMainWindowModel model;
        /// <summary>
        /// C'tor.
        /// </summary>
        public VM_MainWindow()
        {
            model = new MainWindowModel();
        }
        /// <summary>
        /// Checks if the GUI is connected to the server 
        /// and change the color of the title.
        /// </summary>
        public string IsConnected
        {
            get { return model.Color(); }
        }
    }
}
