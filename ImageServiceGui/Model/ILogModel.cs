using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageService.Logging.Modal;
using ImageService.Modal;
using ImageCommunication.Events;

namespace ImageServiceGui.Model
{   
    ///
    interface ILogModel : INotifyPropertyChanged
    {
        #region properties
        /// <summary>
        /// List of ObservableCollection that keeps all the log list.
        /// </summary>
        ObservableCollection<MessageRecievedEventArgs> LogsList { get; set; }

        #endregion
        /// <summary>
        /// Change the color of the title.
        /// </summary>
        /// <returns>The string of the color of the title. </returns>
        string Color();
    }
}
