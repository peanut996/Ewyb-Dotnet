namespace MicroPrinter.App
{
    using MicroPrinter.App.Utils;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Threading;

    internal class AppContext : INotifyPropertyChanged
    {
        private static MicroPrinter.App.AppContext instance;
        private string machineCode;
        private string baseURL;
        public List<PrinterInfo> printers = new List<PrinterInfo>();
        public string defaultPrinter = string.Empty;
        public string baseVersion = string.Empty;
        [CompilerGenerated]
        private PropertyChangedEventHandler PropertyChanged;

        public event PropertyChangedEventHandler PropertyChanged
        {
            [CompilerGenerated] add
            {
                PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
                while (true)
                {
                    PropertyChangedEventHandler a = propertyChanged;
                    PropertyChangedEventHandler handler3 = (PropertyChangedEventHandler) Delegate.Combine(a, value);
                    propertyChanged = Interlocked.CompareExchange<PropertyChangedEventHandler>(ref this.PropertyChanged, handler3, a);
                    if (ReferenceEquals(propertyChanged, a))
                    {
                        return;
                    }
                }
            }
            [CompilerGenerated] remove
            {
                PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
                while (true)
                {
                    PropertyChangedEventHandler source = propertyChanged;
                    PropertyChangedEventHandler handler3 = (PropertyChangedEventHandler) Delegate.Remove(source, value);
                    propertyChanged = Interlocked.CompareExchange<PropertyChangedEventHandler>(ref this.PropertyChanged, handler3, source);
                    if (ReferenceEquals(propertyChanged, source))
                    {
                        return;
                    }
                }
            }
        }

        public AppContext()
        {
            CommonUtil.GetMachineCode();
            string path = Environment.CurrentDirectory + @"\MachineSettings.json";
            string str2 = File.ReadAllText(Environment.CurrentDirectory + @"\MachineCode.ini");
            this.MachineSetting = !File.Exists(path) ? new MachineSettings() : JsonConvert.DeserializeObject<MachineSettings>(File.ReadAllText(path));
            this.MachineSetting.MACHINE_CODE = str2;
            this.MachineCode = str2;
            this.BaseURL = "https://api.ewyb.cn/control/v1";
            this.BaseVersion = "1.0.0";
        }

        private void OnPropertyChanged(string strPropertyInfo)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(strPropertyInfo));
            }
        }

        public string BaseURL
        {
            get => 
                this.baseURL;
            set
            {
                if (value != this.baseURL)
                {
                    this.baseURL = value;
                    this.OnPropertyChanged("BaseURL");
                }
            }
        }

        public string UrlCommon
        {
            get
            {
                string machineCode = this.MachineCode;
                string baseVersion = this.BaseVersion;
                return ("&machine_code=" + machineCode.ToString() + "&version=" + baseVersion.ToString());
            }
        }

        public string BaseVersion
        {
            get => 
                this.baseVersion;
            set
            {
                if (value != this.baseVersion)
                {
                    this.baseVersion = value;
                    this.OnPropertyChanged("baseVersion");
                }
            }
        }

        public static MicroPrinter.App.AppContext Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new MicroPrinter.App.AppContext();
                }
                return instance;
            }
        }

        public string MachineCode
        {
            get => 
                this.machineCode;
            set
            {
                if (value != this.machineCode)
                {
                    this.machineCode = value;
                    this.OnPropertyChanged("MachineCode");
                }
            }
        }

        public MachineSettings MachineSetting { get; set; }
    }
}

