using ImageService.Modal;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageService.Infrastructure;
using ImageService.Infrastructure.Enums;
using ImageService.Logging;
using ImageService.Logging.Modal;
using System.Text.RegularExpressions;
using ImageService.Server;

namespace ImageService.Controller.Handlers
{
    public class DirectoyHandler : IDirectoryHandler
    {
        #region Members
        private IImageController m_controller;              // The Image Processing Controller
        private ILoggingService m_logging;
        private FileSystemWatcher m_dirWatcher;             // The Watcher of the Dir
        private string m_path;                              // The Path of directory
        #endregion

        public event EventHandler<DirectoryCloseEventArgs> DirectoryClose;              // The Event That Notifies that the Directory is being closed

        public DirectoyHandler(ILoggingService m_logging, IImageController m_controller)
        {
            this.m_controller = m_controller;
            this.m_logging = m_logging;
        }

        public void StartHandleDirectory(string dirPath)
        {
            m_path = dirPath;
            m_dirWatcher = new FileSystemWatcher();
            m_dirWatcher.Path = dirPath;
            m_dirWatcher.Created += OnNewFileCreated;
            m_dirWatcher.EnableRaisingEvents = true;
        }

        private void OnNewFileCreated(object sender, FileSystemEventArgs e)
        {
            string[] args = new string[] { e.FullPath };
            bool result;
            string msg = m_controller.ExecuteCommand((int)CommandEnum.NewFileCommand, args, out result);
            m_logging.Log(msg, MessageTypeEnum.INFO);
        }

        public void OnCommandRecieved(object sender, CommandRecievedEventArgs e)
        {
            bool result;
            string msg = m_controller.ExecuteCommand(e.CommandID, e.Args, out result);
            if (result)
            {
                m_logging.Log(msg, MessageTypeEnum.INFO);
            }
            else
            {
                m_logging.Log(msg, MessageTypeEnum.FAIL);
            }
        }

        public void onCloseService(object sender, CommandRecievedEventArgs e)
        {
            ImageServer server = (ImageServer)sender;
            m_dirWatcher.EnableRaisingEvents = false;
            m_dirWatcher.Dispose();
            m_logging.Log("Handler closed " + m_path, MessageTypeEnum.INFO);
            server.CloseService -= OnCommandRecieved;

        }
    }
}
