using System.Globalization;
using System.Numerics;
using System.Windows.Data;
using System.Windows.Media;

namespace Cable.App.Views.Converters;

public class Vector4ToBrushConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not Vector4 vec)
            return value;

        return new SolidColorBrush(Color.FromArgb(
            (byte)(vec.W * byte.MaxValue),
            (byte)(vec.X * byte.MaxValue),
            (byte)(vec.Y * byte.MaxValue),
            (byte)(vec.Z * byte.MaxValue)));
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is SolidColorBrush sb)
            return new Vector4(sb.Color.R, sb.Color.G, sb.Color.B, sb.Color.A);

        if (value is Color c)
            return new Vector4(c.R, c.G, c.B, c.A);

        return value;
    }
}