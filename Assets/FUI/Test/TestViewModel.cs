﻿using System;
using System.Collections.Generic;
using System.Text;

namespace FUI.Test
{
    [Binding("SampleView")]
    public class SampleViewModel : ViewModel
    {
        [Binding("testText", typeof(StringToTextAdpator))]
        public string Name { get; set; }

        public override void Initialize()
        {
            Name = "aaaa";
        }
    }
}