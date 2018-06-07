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

namespace ImageServiceWeb.Models
{
    public class ConfigModel
    {

        private ITcpClient m_client;


        public ConfigModel()
        {
            m_outputDirectory = "";
            m_logName = "";
            m_sourceName = "";
            m_ThumbnailSize = "";
            try
            {
                m_client = TcpClientChannel.ClientInstance;
                m_client.DataReceived += GetMessageFromClient;

                m_handlers = new ObservableCollection<string>();
                m_handlers.CollectionChanged += (s, e) => NotifyPropertyChanged("Handlers");

                m_client.Send(new CommandRecievedEventArgs((int)CommandEnum.GetConfigCommand, null, null).ToJson());
                System.Diagnostics.Debug.WriteLine("settings model");

            }
            catch (Exception e)
            {
                Console.Write(e.ToString());
            }
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
        //string IConfigModel.OutputDirectory
        //{
        //    get
        //    {
        //        return this.m_outputDirectory;
        //    }
        //    set
        //    {
        //        this.m_outputDirectory = value;
        //        this.NotifyPropertyChanged("OutputDirectory");
        //    }
        //}


        //string IConfigModel.SourceName
        //{
        //    get
        //    {
        //        return this.m_sourceName;
        //    }
        //    set
        //    {
        //        this.m_sourceName = value;
        //        this.NotifyPropertyChanged("SourceName");
        //    }
        //}


        //string IConfigModel.LogName
        //{
        //    get
        //    {
        //        return this.m_logName;
        //    }
        //    set
        //    {
        //        this.m_logName = value;
        //        this.NotifyPropertyChanged("LogName");
        //    }
        //}

        //string IConfigModel.ThumbnailSize
        //{
        //    get
        //    {
        //        return m_ThumbnailSize;
        //    }
        //    set
        //    {
        //        this.m_ThumbnailSize = value;
        //        this.NotifyPropertyChanged("ThumbnailSize");
        //    }
        //}

        //ObservableCollection<string> IConfigModel.Handlers
        //{
        //    get
        //    {
        //        return this.m_handlers;
        //    }
        //    set
        //    {
        //        this.m_handlers = value;
        //        this.NotifyPropertyChanged("Handlers");
        //    }
        //}

        public event PropertyChangedEventHandler PropertyChanged;


        public void NotifyPropertyChanged(string propname)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propname));
            }
        }

        public void GetMessageFromClient(object sender, DataReceivedEventArgs data)
        {
            string message = data.Message;
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
                string[] handlersArray = ((string)json["Handler"]).Split(';');
                for (int j = 0; j < handlersArray.Length; ++j)
                {
                    ////m_handlers.Add(handlersArray[j]);
                    //App.Current.Dispatcher.Invoke(() => m_handlers.Add(handlersArray[j]));
                    m_handlers.Add(handlersArray[j]);
                }
                System.Diagnostics.Debug.WriteLine("Done!");
            }
            else
            {
                if ((ITcpClient)sender == m_client)
                {
                    //App.Current.Dispatcher.Invoke(() => this.m_handlers.Remove(data.Message));
                    this.m_handlers.Remove(data.Message);
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("Config model ignored message = " + message);
                }
            }
        }

        public int RemoveHandler(string selected)
        {
            try
            {
                string[] args = new string[1];
                args[0] = selected;
                m_client.Send(new CommandRecievedEventArgs((int)CommandEnum.CloseCommand, args, null).ToJson());
                return 1;
            } catch (Exception e)
            {
                return 0;
            }
        }
    }
}