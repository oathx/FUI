using System;

namespace FUI
{
    public interface IValueConverter
    {
        object Convert(object value, Type targetType);
        object ConvertBack(object value, Type targetType);
    }

    public interface IValueConverter<TValueType, TTargetType>
    {
        TTargetType Convert(TValueType value);
        TValueType ConvertBack(TTargetType value);
    }
}
