namespace MicroPrinter.App
{
    using System;

    internal class PrinterInfo
    {
        private string name;

        public string Name
        {
            get => 
                this.name;
            set => 
                (this.name = value);
        }
    }
}

