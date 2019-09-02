using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Data;

namespace Marvin.AbstractionLayer.UI
{
    public class SortedCollectionViewSourceConverter : IValueConverter
    {
        public string Property { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var cvs = new CollectionViewSource { Source = value };
            cvs.SortDescriptions.Add(new SortDescription(Property, ListSortDirection.Ascending));

            return cvs.View;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
