using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace MyWeatherApp.converter
{
    class bool2string : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if ((bool)value == true)
            {
                return "查找路况";
            } else
            {
                return "查找地图";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if ((string)value == "查找地图")
            {
                return false;
            } else
            {
                return true;
            }
        }
    }
}
