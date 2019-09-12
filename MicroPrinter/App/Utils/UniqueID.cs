namespace MicroPrinter.App.Utils
{
    using System;
    using System.Management;

    internal class UniqueID
    {
        public static string GetCPUID()
        {
            string str = null;
            using (ManagementClass class2 = new ManagementClass("Win32_Processor"))
            {
                foreach (ManagementObject obj1 in class2.GetInstances())
                {
                    str = obj1.Properties["ProcessorId"].Value.ToString();
                    obj1.Dispose();
                }
            }
            return str;
        }
    }
}

