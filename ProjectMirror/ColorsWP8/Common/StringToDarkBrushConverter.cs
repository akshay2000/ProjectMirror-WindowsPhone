using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace ColorsWP8.Common
{
    public class StringToDarkBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var selectedCategoryColor = value as string;
            switch (selectedCategoryColor)
            {
                case "Emrald":
                    return new SolidColorBrush(Color.FromArgb(255, 0, 138, 0));
                case "Cobalt":
                    return new SolidColorBrush(Color.FromArgb(255, 0, 80, 239));
                case "Violet":
                    return new SolidColorBrush(Color.FromArgb(255, 170, 0, 255));
                case "Magenta":
                    return new SolidColorBrush(Color.FromArgb(255, 216, 0, 115));
                case "Red":
                    return new SolidColorBrush(Color.FromArgb(255, 229, 20, 0));
                case "Pink":
                    return new SolidColorBrush(Color.FromArgb(255, 244, 114, 208));
                case "Orange":
                    return new SolidColorBrush(Color.FromArgb(255, 250, 104, 0));
                case "Lime":
                    return new SolidColorBrush(Color.FromArgb(255, 164, 196, 0));
                case "Cyan":
                    return new SolidColorBrush(Color.FromArgb(255, 27, 161, 226));
                case "Brown":
                    return new SolidColorBrush(Color.FromArgb(255, 130, 90, 44));
                case "Black":
                    return new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));
                case "Yellow":
                    return new SolidColorBrush(Color.FromArgb(255, 227, 200, 0));
                default:
                    return new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
