using System;
using System.Windows;
using System.Windows.Data;

namespace TileControlLib
{
    public class TileControlStateToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var result = Visibility.Hidden;
            switch (System.Convert.ToInt32(parameter))
            {
                case 0:
                    result = ((TileControlState)value) == TileControlState.Covered ? Visibility.Visible : Visibility.Hidden;
                    break;
                case 1:
                    result = ((TileControlState)value) == TileControlState.Text ? Visibility.Visible : Visibility.Hidden;
                    break;
                case 2:
                    result = ((TileControlState)value) == TileControlState.Bomb ? Visibility.Visible : Visibility.Hidden;
                    break;
            }
            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var result = TileControlState.Covered;
            switch (System.Convert.ToInt32(parameter))
            {
                case 0:
                    result = ((Visibility)value) == Visibility.Visible ? TileControlState.Covered : TileControlState.Text;
                    break;
                case 1:
                    result = ((Visibility)value) == Visibility.Visible ? TileControlState.Text : TileControlState.Covered;
                    break;
                case 2:
                    result = ((Visibility)value) == Visibility.Visible ? TileControlState.Bomb : TileControlState.Covered;
                    break;
            }
            return result;
        }
    }
}
