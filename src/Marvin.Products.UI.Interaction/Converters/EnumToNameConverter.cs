using System;
using System.Globalization;
using System.Windows.Data;

namespace Marvin.Products.UI.Interaction.Converters
{
    internal class EnumToNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;

            var type = value.GetType();

            return type.IsEnum ? value.ToString() : value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;

            if (!targetType.IsEnum)
                return value;

            return Enum.Parse(targetType, value as string);
        }
    }
}
