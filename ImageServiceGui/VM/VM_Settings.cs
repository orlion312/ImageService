using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Prism.Commands;
using System.ComponentModel;
using ImageServiceGui.Model;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Threading;
using System.Windows.Data;

namespace ImageServiceGui.VM
{
    class VM_Settings : INotifyPropertyChanged
    {
        private ISettingsModel model;
        private string selectedHandler;
        private static Mutex mut=new Mutex();

        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// C'tor.
        /// </summary>
        public VM_Settings()
        {
            model = new SettingsModel(); 
            model.PropertyChanged += delegate (Object sender, PropertyChangedEventArgs e) {
                NotifyPropertyChanged("VM_" + e.PropertyName);
            };
            CloseHandler = new DelegateCommand<object>(this.removeHandler, this.canRemoveHandler);
            PropertyChanged += RemoveSelectedHandler;
        }

        private void NotifyPropertyChanged(string propName)
        {
            PropertyChangedEventArgs propertyChangedEventArgs = new PropertyChangedEventArgs(propName);
            this.PropertyChanged?.Invoke(this, propertyChangedEventArgs);
        }
        #region Properties.
        public string ColorTitle
        {
            get { return model.Color(); }
        }
        public string VM_OutputDir
        {
            get { return model.OutputDirectory; }
        }
        public string VM_LogName
        {
            get { return model.LogName; } set { model.LogName = value; }
        }
        public string VM_SourceName
        {
            get { return model.SourceName; }
        }
        public string VM_ThumbnailSize
        {
            get { return model.ThumbnailSize; } set { model.ThumbnailSize = value; }
        }
        public ObservableCollection<string> VM_Handlers
        {
            get { return model.Handlers; } 
        }
        public string Selected
        {
            get { return selectedHandler; }
            set
            {
                selectedHandler = value;
                NotifyPropertyChanged("Selected");
            }
        }
        public ICommand CloseHandler { get; private set; }
        #endregion
        /// <summary>
        /// Sends the command to close the handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="p"> Change the property.</param>
        private void RemoveSelectedHandler(Object sender, PropertyChangedEventArgs p)
        {
            var command = this.CloseHandler as DelegateCommand<object>;
            command.RaiseCanExecuteChanged();
        }
        /// <summary>
        /// Removes the handler.
        /// </summary>
        /// <param name="obj"></param>
        private void removeHandler(object obj)
        {
            this.model.RemoveHandler(selectedHandler);
            this.model.Handlers.Remove(selectedHandler);
            selectedHandler = null;
        }
        /// <summary>
        /// Checks if we can remove the handler.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private bool canRemoveHandler(object obj)
        {
            if (string.IsNullOrEmpty(this.selectedHandler))
            {
                return false;
            }
            return true;
        }
    }
}