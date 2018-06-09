using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageCommunication;
using ImageService.Modal;
using ImageService.Infrastructure.Enums;
using ImageCommunication.Events;

namespace ImageServiceGui.Model
{
    public class SettingsModel : ISettingsModel
    {
        private string m_outputDirectory;
        private string m_sourceName;
        private string m_logName;
        private string ThumbnailSize;
        public ObservableCollection<string> m_handlers { get; set;}
        private ITcpClient m_client;

        public SettingsModel()
        {
            try
            {
                m_client = TcpClientChannel.ClientInstance;
                m_client.DataReceived += GetMessageFromClient;

                m_handlers = new ObservableCollection<string>();
                m_handlers.CollectionChanged += (s, e) => NotifyPropertyChanged("Handlers");

                m_client.Send(new CommandRecievedEventArgs((int)CommandEnum.GetConfigCommand, null, null).ToJson());
            } catch (Exception e)
            {
                Console.Write(e.ToString());
            }
        }

        string ISettingsModel.OutputDirectory
        {
            get
            {
                return this.m_outputDirectory;
            }
            set
            {
                this.m_outputDirectory = value;
                this.NotifyPropertyChanged("OutputDirectory");
            }
        }


        string ISettingsModel.SourceName
        {
            get
            {
                return this.m_sourceName;
            }
            set
            {
                this.m_sourceName = value;
                this.NotifyPropertyChanged("SourceName");
            }
        }


        string ISettingsModel.LogName
        {
            get
            {
                return this.m_logName;
            }
            set
            {
                this.m_logName = value;
                this.NotifyPropertyChanged("LogName");
            }
        }

        string ISettingsModel.ThumbnailSize
        {
            get
            {
                return this.ThumbnailSize;
            }
            set
            {
                this.ThumbnailSize = value;
                this.NotifyPropertyChanged("ThumbnailSize");
            }
        }

        ObservableCollection<string> ISettingsModel.Handlers
        {
            get
            {
                return this.m_handlers;
            }
            set
            {
                this.m_handlers = value;
                this.NotifyPropertyChanged("Handlers");
            }
        }

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
                ThumbnailSize = (string)json["ThumbnailSize"];
                m_logName = (string)json["LogName"];
                string[] handlersArray = ((string)json["Handler"]).Split(';');
                for(int j = 0; j < handlersArray.Length; ++j)
                {
                    //m_handlers.Add(handlersArray[j]);
                    App.Current.Dispatcher.Invoke(() => m_handlers.Add(handlersArray[j]));
                }
                System.Diagnostics.Debug.WriteLine("Done!");
            }
            else
            {
                if ((ITcpClient)sender == m_client) {
                    App.Current.Dispatcher.Invoke(() => this.m_handlers.Remove(data.Message));
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("Config model ignored message = " + message);
                }
            }
        }

        public void RemoveHandler(string selected)
        {
            string[] args = new string[1];
            args[0] = selected;
            m_client.Send(new CommandRecievedEventArgs((int)CommandEnum.CloseCommand, args, null).ToJson());
        }

        public string Color()
        {
            if(m_client != null && m_client.Connect())
            {
                return "Blue";
            }
            return "Black";
        }
    }
}
