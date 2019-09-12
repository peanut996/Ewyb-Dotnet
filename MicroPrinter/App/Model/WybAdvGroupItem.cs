namespace MicroPrinter.App.Model
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class WybAdvGroupItem
    {
        public WybAdvGroupItem()
        {
            this.FileList = new List<string>();
            this.RealFileList = new List<string>();
        }

        public int Duration { get; set; }

        public int DurationCount { get; set; }

        public List<string> FileList { get; set; }

        public string Id { get; set; }

        public int Order { get; set; }

        public int PlayOrder { get; set; }

        public List<string> RealFileList { get; set; }

        public string Thumbnail { get; set; }

        public string Type { get; set; }
    }
}

