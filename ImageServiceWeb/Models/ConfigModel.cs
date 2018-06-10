using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
using ImageCommunication;
using ImageCommunication.Events;
using ImageService.Modal;
using ImageService.Infrastructure.Enums;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageServiceWeb.Models
{
    //the model of the config
    public class ConfigModel
    {

        public ITcpClient m_client;

        //the constructor of the class
        public ConfigModel()
        {
            //initialize the parameters
            initialize();
            try
            {
                //connect to the client
                m_client = TcpClientChannel.ClientInstance;
                isDelete = false;
                m_client.DataReceived += GetMessageFromClient;
                sendCommand();
            }
            catch (Exception e)
            {
                Console.Write(e.ToString());
            }
        }

        //the function initialize the parameters
        public void initialize()
        {
            m_outputDirectory = "";
            m_logName = "";
            m_sourceName = "";
            m_ThumbnailSize = "";
            m_handlers = new ObservableCollection<string>();
        }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Output Directory: ")]
        public string m_outputDirectory { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Source Name: ")]
        public string m_sourceName { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Log Name: ")]
        public string m_logName { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Thumbnail Size: ")]
        public string m_ThumbnailSize { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Handlers: ")]
        public ObservableCollection<string> m_handlers { get; set; }        //string m_OutputDirectory

        public bool isDelete;

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// the function responsibole to notify if something was changed
        /// </summary>
        /// <param name="propname">the property that was changed</param>
        public void NotifyPropertyChanged(string propname)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propname));
            }
        }

        /// <summary>
        /// the function get the message(the configs of the service and if hadler deleted) from the service
        /// </summary>
        /// <param name="sender">the object that sent the data</param>
        /// <param name="data">the data that the model received</param>
        public void GetMessageFromClient(object sender, DataReceivedEventArgs data)
        {
            string message = data.Message;
            //check if it's config, split the message to the parameters
            if (message.Contains("Config "))
            {
                System.Diagnostics.Debug.WriteLine("Working on config...");
                int i = message.IndexOf(" ") + 1;
                message = message.Substring(i);
                JObject json = JObject.Parse(message);
                m_outputDirectory = (string)json["OutputDir"];
                m_sourceName = (string)json["SourceName"];
                m_ThumbnailSize = (string)json["ThumbnailSize"];
                m_logName = (string)json["LogName"];
                m_handlers = new ObservableCollection<string>();
                string[] handlersArray = ((string)json["Handler"]).Split(';');
                for (int j = 0; j < handlersArray.Length; ++j)
                {
                    if(!String.IsNullOrEmpty(handlersArray[j]))
                        m_handlers.Add(handlersArray[j]);
                }
                System.Diagnostics.Debug.WriteLine("Done!");
            }
            else
            {
                //if it's handler to delete
                if ((ITcpClient)sender == m_client)
                {
                    isDelete = true;
                    this.m_handlers.Remove(data.Message);
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("Config model ignored message = " + message);
                }
            }
        }

        /// <summary>
        /// the function remove the selected handler
        /// </summary>
        /// <param name="selected">a path that represent the selected handler</param>
        public void RemoveHandler(string selected)
        {
            isDelete = false;
            string[] args = new string[1];
            args[0] = selected;
            //send the command to remove the handler
            m_client.Send(new CommandRecievedEventArgs((int)CommandEnum.CloseCommand, args, null).ToJson());
        }

        //the function send the command to the service to get the data(status and photos number)
        public void sendCommand()
        {
            if (m_client != null)
            {
                //check if the client connect
                if (m_client.Connect())
                {
                    m_client.Send(new CommandRecievedEventArgs((int)CommandEnum.GetConfigCommand, null, null).ToJson());
                }
                else
                {
                    initialize();
                }
            } else
            {
                try
                {
                    m_client = TcpClientChannel.ClientInstance;
                    isDelete = false;
                    m_client.DataReceived += GetMessageFromClient;
                    sendCommand();
                } catch { }
            }
        }
    }
}