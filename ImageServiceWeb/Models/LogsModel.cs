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
    /// <summary>
    /// the model of the logs
    /// </summary>
    public class LogsModel
    {
        private ITcpClient client;
        public event PropertyChangedEventHandler PropertyChanged;

        #region properties
        public ObservableCollection<Tuple<string, string>> LogsList { get; set; }

        #endregion



        //the constructor
        public LogsModel()
        {
            //initialize the parametres
            initialize();
            try
            {
                //connect to the client
                client = TcpClientChannel.ClientInstance;
                client.DataReceived += GetMessageFromClient;
                BindingOperations.EnableCollectionSynchronization(LogsList, new object());
                LogsList.CollectionChanged += (s, e) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("LogsList"));
                
                //send the command to get the logs
                client.Send((new CommandRecievedEventArgs((int)CommandEnum.LogCommand, null, null)).ToJson());
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }

        //the function initialize the parameters of the logs
        public void initialize()
        {
            LogsList = new ObservableCollection<Tuple<string, string>>();
        }

        /// <summary>
        /// the function get the message(logs) from the service
        /// </summary>
        /// <param name="sender">the object that sent the data</param> 
        /// <param name="data">the data that the model received- the logs</param>
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
                            //split the log to type and message
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

        public void tryConnect()
        {
            //check if the client connect
            if (client == null)
            {
                try
                {
                    //connect to the client
                    client = TcpClientChannel.ClientInstance;
                    client.DataReceived += GetMessageFromClient;
                    BindingOperations.EnableCollectionSynchronization(LogsList, new object());
                    LogsList.CollectionChanged += (s, e) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("LogsList"));

                    //send the command to get the logs
                    client.Send((new CommandRecievedEventArgs((int)CommandEnum.LogCommand, null, null)).ToJson());
                }
                catch { }
            }
        }
    }
}