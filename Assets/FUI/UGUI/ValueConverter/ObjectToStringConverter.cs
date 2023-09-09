namespace FUI.UGUI.ValueConverter
{
    /// <summary>
    /// 将一个object转换成String
    /// </summary>
    public class ObjectToStringConverter : ValueConverter<object, string>
    {
        public override string Convert(object value)
        {
            return value.ToString();
        }

        public override object ConvertBack(string value)
        {
            return value;
        }
    }

    /// <summary>
    /// 将一个int转换成string
    /// </summary>
    public class IntToStringConverter : ValueConverter<int, string>
    {
        public override string Convert(int value)
        {
            return value.ToString();
        }
        public override int ConvertBack(string value)
        {
            return int.Parse(value);
        }
    }

    /// <summary>
    /// 将一个long转换成string
    /// </summary>
    public class LongToStringConverter : ValueConverter<long, string>
    {
        public override string Convert(long value)
        {
            return value.ToString();
        }
        public override long ConvertBack(string value)
        {
            return long.Parse(value);
        }
    }

    /// <summary>
    /// 将一个float转换成string
    /// </summary>
    public class FloatToStringConverter : ValueConverter<float, string>
    {
        public override string Convert(float value)
        {
            return value.ToString();
        }
        public override float ConvertBack(string value)
        {
            return float.Parse(value);
        }
    }

    /// <summary>
    /// 将一个double转换成string
    /// </summary>
    public class DoubleToStringConverter : ValueConverter<double, string>
    {
        public override string Convert(double value)
        {
            return value.ToString();
        }
        public override double ConvertBack(string value)
        {
            return double.Parse(value);
        }
    }

    /// <summary>
    /// 将一个bool转换成string
    /// </summary>
    public class BoolToStringConverter : ValueConverter<bool, string>
    {
        public override string Convert(bool value)
        {
            return value.ToString();
        }
        public override bool ConvertBack(string value)
        {
            return bool.Parse(value);
        }
    }
}
