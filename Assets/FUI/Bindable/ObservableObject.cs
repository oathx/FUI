﻿using System;

namespace FUI.Bindable
{
    public class ObservableObject : INotifyPropertyChanged
    {
        public Action<object, string> PropertyChanged { protected get; set; }
    }
}
