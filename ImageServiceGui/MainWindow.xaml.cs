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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ImageServiceGui
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private SettingsWindow st;
        private LogWindow lg;
        VM_MainWindow m_VM;

        public MainWindow()
        {
            InitializeComponent();
            m_VM = new VM_MainWindow();
            //st = new SettingsWindow();
            //lg = new LogWindow();
            DataContext = m_VM;
        }
    }
}
