using ImageCommunication;
using ImageCommunication.Events;
using ImageService.Infrastructure.Enums;
using ImageService.Logging.Modal;
using ImageService.Modal;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Web;
using System.Windows.Data;

namespace ImageServiceWeb.Models
{
    public class LogsModel
    {
        private ITcpClient client;
        public event PropertyChangedEventHandler PropertyChanged;

        #region properties
        public ObservableCollection<Tuple<string, string>> LogsList { get; set; }

        #endregion




        public LogsModel()
        {
            LogsList = new ObservableCollection<Tuple<string, string>>();
            try
            {
                client = TcpClientChannel.ClientInstance;
                client.DataReceived += GetMessageFromClient;
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
                            Tuple<string, string> tup = new Tuple<string, string>(messageRecieved.Status.ToString(), messageRecieved.Message);
                            LogsList.Add(tup);
                        }
                        catch (Exception e) { throw e; }

                    }
                }
            }

        }
    }
}