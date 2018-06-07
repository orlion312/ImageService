using ImageService.Infrastructure.Enums;
using ImageService.Logging.Modal;
using ImageService.Modal;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using ImageCommunication;
using ImageCommunication.Events;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Windows.Data;

namespace ImageServiceGui.Model
{
    public class LogModel : ILogModel
    {
        private ITcpClient client;
        private ObservableCollection<MessageRecievedEventArgs> m_logs;
        public event PropertyChangedEventHandler PropertyChanged;

        #region properties
        public ObservableCollection<MessageRecievedEventArgs> LogsList
        {
            get
            {
                return this.m_logs;
            }
            set
            {
                this.m_logs = value;
            }
        }
        #endregion




        public LogModel()
        {
            try
            {
                client = TcpClientChannel.ClientInstance;
                client.DataReceived += GetMessageFromClient;
                LogsList = new ObservableCollection<MessageRecievedEventArgs>();
                BindingOperations.EnableCollectionSynchronization(LogsList, new object());
                LogsList.CollectionChanged += (s, e) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("LogsList"));
                client.Send((new CommandRecievedEventArgs((int)CommandEnum.LogCommand, null, null)).ToJson());
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }


        public void GetMessageFromClient(object sender, DataReceivedEventArgs data)
        {
            System.Diagnostics.Debug.WriteLine(data.Message);
            string message = data.Message;
            if (message[0] != '{') return;
            CommandRecievedEventArgs commandRecieved = CommandRecievedEventArgs.FromJson(message);
            if (commandRecieved.CommandID == (int)CommandEnum.LogCommand)
            {
                ObservableCollection<MessageRecievedEventArgs> tempList = new ObservableCollection<MessageRecievedEventArgs>();
                string[] logsStrings = commandRecieved.Args[0].Split(';');
                foreach (string s in logsStrings)
                {
                    if (s.Contains("Status") && s.Contains("Message"))
                    {
                        try
                        {
                            JObject jObject = (JObject)JsonConvert.DeserializeObject(s);
                            int messageType = (int)jObject["Status"];
                            string msg = (string)jObject["Message"];
                            MessageRecievedEventArgs messageRecieved = new MessageRecievedEventArgs((MessageTypeEnum)messageType, msg);
                            LogsList.Add(messageRecieved);
                        }
                        catch (Exception e) { throw e; }

                    }
                }
            }

        }

        public string Color()
        {
            if (client != null && client.Connect())
            {
                return "Blue";
            }
            return "Black";
        }
    }
}
