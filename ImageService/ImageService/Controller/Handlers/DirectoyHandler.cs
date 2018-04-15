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

        /// <summary>
        /// the constructor of the class, get ILoggingService and a IImageController
        /// </summary>
        /// <param name="m_logging">an ILoggingService that represent the logging</param>
        /// <param name="m_controller">IImageController that represent the controller</param>
        public DirectoyHandler(ILoggingService m_logging, IImageController m_controller)
        {
            this.m_controller = m_controller;
            this.m_logging = m_logging;
        }

        /// <summary>
        /// a method that get a string and start handle the directory that it get
        /// </summary>
        /// <param name="dirPath">a string that represent the directory that need to handle</param>
        public void StartHandleDirectory(string dirPath)
        {
            m_path = dirPath;
            m_dirWatcher = new FileSystemWatcher();
            m_dirWatcher.Path = dirPath;
            m_dirWatcher.Created += OnNewFileCreated;
            m_dirWatcher.EnableRaisingEvents = true;
        }

        /// <summary>
        /// a method that get an object and a FileSystemEventArgs and notify the user about the file
        /// </summary>
        /// <param name="sender">an object</param>
        /// <param name="e">a FileSystemEventArgs that represent the path of the file</param>
        private void OnNewFileCreated(object sender, FileSystemEventArgs e)
        {
            string[] args = new string[] { e.FullPath };
            bool result;
            string msg = m_controller.ExecuteCommand((int)CommandEnum.NewFileCommand, args, out result);
            m_logging.Log(msg, MessageTypeEnum.INFO);
        }

        /// <summary>
        /// a method that get an object and a CommandRecievedEventArgs and execute the command recieved
        /// </summary>
        /// <param name="sender">an object</param>
        /// <param name="e">a CommandRecievedEventArgs that represent the command who recieved</param>
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

        /// <summary>
        /// a method that get an object and a CommandRecievedEventArgs and close the service
        /// </summary>
        /// <param name="sender">an object yhat reoresent the ImageServer</param>
        /// <param name="e">a CommandRecievedEventArgs</param>
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
