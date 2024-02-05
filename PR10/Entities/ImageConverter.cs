using System;
using System.Globalization;
using System.IO;
using Avalonia;
using Avalonia.Data.Converters;
using Avalonia.Media.Imaging;

public class ImageConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is byte[] bytes)
        {
            using (var stream = new MemoryStream(bytes))
            {
                var bitmap = new Bitmap(stream);
                return bitmap;
            }
        }
        
        return AvaloniaProperty.UnsetValue;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}