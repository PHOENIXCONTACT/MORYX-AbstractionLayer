// Copyright (c) 2020, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Data;

namespace Moryx.AbstractionLayer.UI
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
