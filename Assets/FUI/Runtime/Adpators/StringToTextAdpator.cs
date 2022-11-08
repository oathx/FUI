using System;
using System.Collections.Generic;
using System.Text;

namespace FUI
{
    public class StringToTextAdpator : IValueConverter<string>
    {
        public void Convert(string value)
        {
            throw new NotImplementedException();
        }
    }

    public class IntToTextAdpator : IValueConverter<int>
    {
        public void Convert(int value)
        {

        }
    }

    public class FloatToTextAdpator : IValueConverter<float>
    {
        public void Convert(float value)
        {

        }
    }
}
