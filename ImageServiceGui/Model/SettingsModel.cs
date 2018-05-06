using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageServiceGui.Model
{
    class SettingsModel : ISettingsModel
    {
        private string m_outputDirectory;
        private string m_sourceName;
        private string m_logName;
        private string ThumbnailSize;
        private ObservableCollection<string> m_handlers;

        public SettingsModel()
        {
            /**JObject jsonObject = JObject.Parse("s");
            m_outputDirectory = (string)jsonObject["outputDirectory"];
            m_sourceName = (string)jsonObject["surceName"];
            m_logName = (string)jsonObject["mlogName"];
            ThumbnailSize = (string)jsonObject["thubnailSize"];
            */
            m_outputDirectory = "hi";
            m_sourceName = "s";
            m_logName = "Or";
            ThumbnailSize = "12";
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
                this.NotifyPropertyChanged("ISettingsModel.OutputDirectory");
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
                this.NotifyPropertyChanged("ISettingsModel.SourceName");
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
                this.NotifyPropertyChanged("ISettingsModel.LogName");
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
                this.NotifyPropertyChanged("ISettingsModel.ThumbnailSize");
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
                this.NotifyPropertyChanged("ISettingsModel.Handlers");
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
    }
}
