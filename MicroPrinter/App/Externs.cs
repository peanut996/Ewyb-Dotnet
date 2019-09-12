namespace MicroPrinter.App
{
    using System;
    using System.Runtime.InteropServices;

    internal class Externs
    {
        [DllImport("winspool.drv")]
        public static extern bool SetDefaultPrinter(string Name);
    }
}

