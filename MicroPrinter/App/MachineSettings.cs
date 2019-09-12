namespace MicroPrinter.App
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Xml.Serialization;

    [XmlRoot("MachineSettings")]
    internal class MachineSettings
    {
        public MachineSettings()
        {
            this.PHOTO_7INCH_LANDSCAPE = false;
        }

        public string BW_A4_PAPER_SIZE { get; set; }

        public string BW_A4_PAPER_SOURCE { get; set; }

        public bool BW_A4_PRINT { get; set; }

        public string COLOR_A4_PAPER_SIZE { get; set; }

        public string COLOR_A4_PAPER_SOURCE { get; set; }

        public bool COLOR_A4_PRINT { get; set; }

        public string DEFAULT_PRINTER { get; set; }

        public string MACHINE_CODE { get; set; }

        public bool PHOTO_7INCH_LANDSCAPE { get; set; }

        public string PHOTO_7INCH_PAPER_SIZE { get; set; }

        public string PHOTO_7INCH_PAPER_SOURCE { get; set; }

        public bool PHOTO_7INCH_PRINT { get; set; }
    }
}

