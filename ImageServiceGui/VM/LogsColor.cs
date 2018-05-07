using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace ImageServiceGui.VM
{
    class LogsColor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(targetType.Name != "Brush")
            {
                throw new Exception("Brush!!");
            }
            if(value.ToString() == "INFO")
            {
                return System.Windows.Media.Brushes.Green;
            }
            else if (value.ToString() == "WARNING")
            {
                return System.Windows.Media.Brushes.Yellow;
            }
            else if(value.ToString() == "FAIL")
            {
                return System.Windows.Media.Brushes.Red;
            }

            return Brushes.Transparent;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
