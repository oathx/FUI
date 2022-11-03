using System;
using System.Collections.Generic;
using System.Text;

namespace FUI
{
    public class StringToTextAdpator : IAdpator<string>
    {
        public void OnValueChanged(string oldValue, string newValue)
        {
            throw new NotImplementedException();
        }
    }

    public class IntToTextAdpator : IAdpator<int>
    {
        public void OnValueChanged(int oldValue, int newValue)
        {

        }
    }

    public class FloatToTextAdpator : IAdpator<float>
    {
        public void OnValueChanged(float oldValue, float newValue)
        {

        }
    }
}
