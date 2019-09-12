namespace MicroPrinter.App
{
    using System;
    using System.Collections.Generic;
    using System.Management;
    using System.Runtime.CompilerServices;

    public class PrintJobInfo
    {
        public void Cancel()
        {
            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_PrintJob WHERE JobId=" + this.JobId))
            {
                ManagementObjectCollection.ManagementObjectEnumerator enumerator = searcher.Get().GetEnumerator();
                if (enumerator.MoveNext())
                {
                    ((ManagementObject) enumerator.Current).Delete();
                }
            }
        }

        public static PrintJobInfo Get(int jobId)
        {
            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_PrintJob WHERE JobId=" + jobId))
            {
                ManagementObjectCollection objects = searcher.Get();
                if (objects.Count > 0)
                {
                    using (ManagementObjectCollection.ManagementObjectEnumerator enumerator = objects.GetEnumerator())
                    {
                        if (enumerator.MoveNext())
                        {
                            return Parse((ManagementObject) enumerator.Current);
                        }
                    }
                }
            }
            return null;
        }

        public static IEnumerable<PrintJobInfo> GetAll()
        {
            List<PrintJobInfo> list = new List<PrintJobInfo>();
            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_PrintJob"))
            {
                ManagementObjectCollection objects = searcher.Get();
                if (objects.Count > 0)
                {
                    foreach (ManagementObject obj2 in objects)
                    {
                        list.Add(Parse(obj2));
                    }
                }
            }
            return list;
        }

        public static PrintJobInfo GetNext(uint? lastJobId)
        {
            string queryString = "SELECT * FROM Win32_PrintJob";
            if (lastJobId != null)
            {
                queryString = queryString + " WHERE JobId > " + lastJobId;
            }
            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(queryString))
            {
                ManagementObjectCollection objects = searcher.Get();
                if (objects.Count > 0)
                {
                    using (ManagementObjectCollection.ManagementObjectEnumerator enumerator = objects.GetEnumerator())
                    {
                        if (enumerator.MoveNext())
                        {
                            return Parse((ManagementObject) enumerator.Current);
                        }
                    }
                }
            }
            return null;
        }

        private static PrintJobInfo Parse(ManagementObject data)
        {
            PrintJobInfo info1 = new PrintJobInfo();
            info1.Document = (string) data.Properties["Document"].Value;
            info1.JobId = (uint) data.Properties["JobId"].Value;
            info1.JobStatus = (string) data.Properties["JobStatus"].Value;
            info1.PagesPrinted = (uint) data.Properties["PagesPrinted"].Value;
            info1.PaperSize = (string) data.Properties["PaperSize"].Value;
            info1.Priority = (uint) data.Properties["Priority"].Value;
            info1.TotalPages = (uint) data.Properties["TotalPages"].Value;
            info1.TimeSubmitted = (string) data.Properties["TimeSubmitted"].Value;
            info1.Status = (string) data.Properties["Status"].Value;
            info1.StatusMask = (uint) data.Properties["StatusMask"].Value;
            return info1;
        }

        public void Refresh()
        {
            PrintJobInfo info = Get((int) this.JobId);
            if (info == null)
            {
                this.JobStatus = string.Empty;
                this.TotalPages = 0;
                this.Status = string.Empty;
                this.StatusMask = 0;
            }
            else
            {
                this.JobStatus = info.JobStatus;
                this.TotalPages = info.TotalPages;
                this.Status = info.Status;
                this.StatusMask = info.StatusMask;
            }
        }

        public override string ToString() => 
            $"JobId: {this.JobId}, Document:{this.Document}, Status：{this.Status}, JobStatus:{this.JobStatus}, Pages: {this.TotalPages}";

        public string Document { get; set; }

        public bool IsBlocked =>
            ((this.StatusMask & 0x200) != 0);

        public bool IsCompleted =>
            ((this.StatusMask & 0x1000) != 0);

        public bool IsDeleted =>
            ((this.StatusMask & 0x100) != 0);

        public bool IsDeleting =>
            ((this.StatusMask & 4) != 0);

        public bool IsInError =>
            ((this.StatusMask & 2) != 0);

        public bool IsOffline =>
            ((this.StatusMask & 0x20) != 0);

        public bool IsPaperOut =>
            ((this.StatusMask & 0x40) != 0);

        public bool IsPaused =>
            ((this.StatusMask & 1) != 0);

        public bool IsPrinted =>
            ((this.StatusMask & 0x80) != 0);

        public bool IsPrinting =>
            ((this.StatusMask & 0x10) != 0);

        public bool IsRestarted =>
            ((this.StatusMask & 0x800) != 0);

        public bool IsRetained =>
            ((this.StatusMask & 0x2000) != 0);

        public bool IsSpooling =>
            ((this.StatusMask & 8) != 0);

        public bool IsUserInterventionRequired =>
            ((this.StatusMask & 0x400) != 0);

        public uint JobId { get; set; }

        public string JobStatus { get; set; }

        public uint PagesPrinted { get; set; }

        public string PaperSize { get; set; }

        public uint Priority { get; set; }

        public string Status { get; set; }

        public uint StatusMask { get; set; }

        public string TimeSubmitted { get; set; }

        public uint TotalPages { get; set; }
    }
}

