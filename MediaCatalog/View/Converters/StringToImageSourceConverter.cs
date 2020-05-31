using System;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace MediaCatalog.View.Converters
{
    public class StringToImageSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            
            if (value is string)
            {
                if (string.IsNullOrWhiteSpace(value as string))
                {
                    return EmptyImage();
                }

                try
                {
                    Uri fileURI = new Uri(value as string, UriKind.RelativeOrAbsolute);
                    BitmapImage img = new BitmapImage(fileURI);
                    return img;
                }
                catch
                {
                    return EmptyImage();
                }
            }
            else
            {
                return EmptyImage();
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is BitmapImage)
            {
                return (value as BitmapImage).UriSource.ToString();
            }
            else
            {
                throw new FormatException("Поступило некорректное значение");
            }
        }

        private BitmapImage EmptyImage()
        {
            return new BitmapImage(new Uri("/Resources/nopicture.jpg", UriKind.Relative));
        }
    }
}
