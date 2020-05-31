using System;
using System.Globalization;
using System.Windows.Data;

namespace MediaCatalog.View.Converters
{
    public class ActualWidthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double)
            {
                return (double)value > 35 ? (double)value - 35 : 100;
            }
            else
            {
                throw new FormatException("Поступило некорректное значение");
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double)
            {
                return (double)value > 35 ? (double)value + 35 : 100;
            }
            else
            {
                throw new FormatException("Поступило некорректное значение");
            }
        }
    }
}
