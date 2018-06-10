using ImageService.Controller;
using ImageService.Controller.Handlers;
using ImageCommunication;
using ImageService.Infrastructure.Enums;
using ImageService.Logging;
using ImageService.Modal;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageCommunication.Events;
using ImageService.Logging.Modal;
using ImageService.ImageService.Commands;

namespace ImageService.Server
{
    public class ImageServer
    {
        #region Members
        private int imagesCounter;
        private IImageController m_controller;
        private ILoggingService m_logging;
        private ITcpServer m_tcpServer;
        private Dictionary<string, IDirectoryHandler> handlers;
        #endregion

        #region Properties
        public event EventHandler<CommandRecievedEventArgs> CommandRecieved;          // The event that notifies about a new Command being recieved
        public event EventHandler<CommandRecievedEventArgs> CloseService;
        #endregion

        /// <summary>
        /// the constructor get Icontroller and Ilogging, take the two paths to directories
        /// that we need to listen from the APP config,
        /// create handlers for the directrories and notify the logging.
        /// </summary>
        /// <param name="controller">the controller that we paa to the handler</param>
        /// <param name="logging">the logging incharge to notify the user about the process</param>
        public ImageServer(IImageController controller, ILoggingService logging, int imagesCounter)
        {
            this.imagesCounter = imagesCounter;
            this.handlers = new Dictionary<string, IDirectoryHandler>();
            this.m_controller = controller;
            this.m_logging = logging;
        
            string[] directories = (ConfigurationManager.AppSettings.Get("Handler").Split(';'));
            this.m_tcpServer = new TcpServer();
            this.m_tcpServer.DataReceived += ExecuteTcpServer;
            foreach (string path in directories)
            {
                try
                {
                    IDirectoryHandler handler = new DirectoyHandler(m_logging, m_controller, m_tcpServer);
                    CommandRecieved += handler.OnCommandRecieved;
                    CloseService += handler.onCloseService;
                    handler.StartHandleDirectory(path);
                    this.m_logging.Log("Handler created for " + path, Logging.Modal.MessageTypeEnum.INFO);
                    this.handlers[path] = handler;
                }
                catch (Exception e)
                {
                    this.m_logging.Log("Error creating handler for the directory: " + path + " " + e.ToString(), Logging.Modal.MessageTypeEnum.INFO);
                }
            }

            this.m_tcpServer.Start();
        }

        /// <summary>
        /// the method incharge to close the hanlers and the all service.
        /// </summary>
        public void onCloseService()
        {
            CloseService?.Invoke(this, null);
            m_logging.Log("On stop", Logging.Modal.MessageTypeEnum.INFO);
            bool res;
            string s = m_controller.ExecuteCommand((int)CommandEnum.LastLogCommand, null, out res);
            this.m_tcpServer.NotifyAll(s);
        }

        /// <summary>
        /// the method get a message and write to the tcp server this message
        /// </summary>
        /// <param name="msg">the msg to write</param>
        public void Write(string msg)
        {
            DataReceivedEventArgs d = new DataReceivedEventArgs();
            d.Message = msg;
            m_tcpServer.Send(this,d);
        }

        /// <summary>
        /// the method execute the commands that the client send
        /// </summary>
        /// <param name="sender">the object that sent the data</param> 
        /// <param name="data">the data that received</param>
        public void ExecuteTcpServer(Object sender, DataReceivedEventArgs data)
        {
            try
            {
                bool res;
                CommandRecievedEventArgs command = CommandRecievedEventArgs.FromJson(data.Message);
                DataReceivedEventArgs eventArgs = new DataReceivedEventArgs();
                eventArgs.Message = m_controller.ExecuteCommand(command.CommandID, command.Args, out res);
                if (command.CommandID == (int)CommandEnum.CloseCommand)
                {
                    this.closeHandler(command.Args[0]);
                    this.m_tcpServer.NotifyAll(command.Args[0]);
                }
                else
                {
                    IClientHandler client = (IClientHandler)sender;
                    client.Write(this, eventArgs);
                }

            } catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.ToString());
            }
        }

        /// <summary>
        /// the method get an handler and responsible to close the handler 
        /// </summary>
        /// <param name="path">a string that represent the handler</param>
        public void closeHandler(string path)
        {
            IDirectoryHandler directory = this.handlers[path];
            directory.onCloseService(this, null);
            CloseService -= directory.onCloseService;
            handlers.Remove(path);
        }
    }
}
