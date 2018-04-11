using ImageService.Controller;
using ImageService.Controller.Handlers;
using ImageService.Infrastructure.Enums;
using ImageService.Logging;
using ImageService.Modal;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Server
{
    public class ImageServer
    {
        #region Members
        private IImageController m_controller;
        private ILoggingService m_logging;
        #endregion

        #region Properties
        public event EventHandler<CommandRecievedEventArgs> CommandRecieved;          // The event that notifies about a new Command being recieved
        public event EventHandler<CommandRecievedEventArgs> CloseService;                         
        #endregion

        public ImageServer(IImageController controller, ILoggingService logging)
        {
            this.m_controller = controller;
            this.m_logging = logging;
            string[] directories = (ConfigurationManager.AppSettings.Get("Handler").Split(';'));

            foreach (string path in directories)
            {
                try
                {
                    IDirectoryHandler handler = new DirectoyHandler(m_logging, m_controller);
                    CommandRecieved += handler.OnCommandRecieved;
                    CloseService += handler.onCloseService;
                    handler.StartHandleDirectory(path);
                    this.m_logging.Log("Handler created for " + path, Logging.Modal.MessageTypeEnum.INFO);
                }
                catch (Exception e)
                {
                    this.m_logging.Log("Error creating handler for the directory: " + path + " " + e.ToString(), Logging.Modal.MessageTypeEnum.INFO);
                }
            }
        }

        public void onCloseService()
        {
            CloseService?.Invoke(this, null);
        }

    }
}
