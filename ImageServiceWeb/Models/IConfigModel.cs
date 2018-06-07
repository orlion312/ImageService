﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageServiceWeb.Models
{
    interface IConfigModel
    {
        /// <summary>
        /// All the propperties of the settings window. 
        /// </summary>
        #region Properties
        string OutputDirectory { get; set; }
        string SourceName { get; set; }
        string LogName { get; set; }
        string ThumbnailSize { get; set; }
        ObservableCollection<string> Handlers { get; set; }
        #endregion

        /// <summary>
        /// Removes the handler from the list in the GUI.
        /// </summary>
        /// <param name="selected"> The selected handler the user want to remove.</param>
        void RemoveHandler(string selected);
    }
}
