using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageServiceGui.Model;
using System.ComponentModel;
using System.Collections.ObjectModel;
using ImageService.Logging.Modal;

namespace ImageServiceGui.VM
{
    class VM_Log : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private ILogModel model;
        ObservableCollection<MessageRecievedEventArgs> logs;
        /// <summary>
        /// Property of the list of logs.
        /// </summary>
        public ObservableCollection<MessageRecievedEventArgs> VM_LogsList
        {
            get
            {
                return model.LogsList;
            }
            set
            {
                logs = value;
            }
        }
        /// <summary>
        /// C'tor.
        /// </summary>
        public VM_Log()
        {
            this.model = new LogModel();

            model.PropertyChanged +=
                delegate (object sender, PropertyChangedEventArgs e)
                {
                    NotifyPropertyChanged("VM_Logs");
                };
        }
        public string ColorTitle
        {
            get { return model.Color(); }
        }
        public void NotifyPropertyChanged(string propname)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propname));
        }
    }
}