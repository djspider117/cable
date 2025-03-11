using Cable.Data;
using System.Globalization;
using System.Windows.Data;

namespace Cable.App.Views.Converters;

public class NodeConnectorVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is CableDataType cdt)
            return cdt == CableDataType.None ? Visibility.Collapsed : Visibility.Visible;

        return value;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
