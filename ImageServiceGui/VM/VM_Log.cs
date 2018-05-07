using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Prism.Commands;
using ImageServiceGui.Model;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace ImageServiceGui.VM
{
    class VM_Log : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private ILogModel model;

        public VM_Log()
        {
            model = new LogModel();
            model.PropertyChanged += delegate (Object sender, PropertyChangedEventArgs e)
            {
                NotifyPropertyChanged("VM_" + e.PropertyName);
            };
        }


        private void NotifyPropertyChanged(string v)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(v));
        }
    }
}
