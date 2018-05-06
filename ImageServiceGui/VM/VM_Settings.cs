using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Prism.Commands;
using System.ComponentModel;
using ImageServiceGui.Model;
using System.Collections.ObjectModel;

namespace ImageServiceGui.VM
{
    class VM_Settings : INotifyPropertyChanged
    {
        private ISettingsModel model;

        public event PropertyChangedEventHandler PropertyChanged;

        public VM_Settings()
        {
            model = new SettingsModel();
            model.PropertyChanged += delegate (Object sender, PropertyChangedEventArgs e) {
                NotifyPropertyChanged("VM" + e.PropertyName);
            };
        }

        private void NotifyPropertyChanged(string propName)
        {
            PropertyChangedEventArgs propertyChangedEventArgs = new PropertyChangedEventArgs(propName);
            this.PropertyChanged?.Invoke(this, propertyChangedEventArgs);
        }

        public string VM_OutputDir
        {
            get { return model.OutputDirectory; }
        }
        public string VM_LogName
        {
            get { return model.LogName; }
        }
        public string VM_SourceName
        {
            get { return model.SourceName; }
        }
        public string VM_ThumbnailSize
        {
            get { return model.ThumbnailSize; }
        }
        public ObservableCollection<string> VM_Handlers
        {
            get { return model.Handlers; }
        }

    }
}
