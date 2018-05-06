using ImageServiceGui.Model;
using ImageServiceGui.VM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ImageServiceGui
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : UserControl
    {
        private VM_Settings m_VM;
        public SettingsWindow()
        {
            InitializeComponent();
            m_VM = new VM_Settings();
            DataContext = m_VM;
        }
    }
}
