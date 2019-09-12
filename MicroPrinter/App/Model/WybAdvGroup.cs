namespace MicroPrinter.App.Model
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class WybAdvGroup
    {
        public Dictionary<string, WybAdvGroupItem> wybAdvGroupItems = new Dictionary<string, WybAdvGroupItem>();

        public string Id { get; set; }

        public bool IsTemporary { get; set; }

        public DateTime LastedTime { get; set; }

        public string Name { get; set; }
    }
}

