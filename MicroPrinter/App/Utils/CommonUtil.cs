namespace MicroPrinter.App.Utils
{
    using MicroPrinter.App.Api;
    using System;
    using System.Diagnostics;
    using System.IO;

    internal class CommonUtil
    {
        public bool CloseProcess(string name1)
        {
            bool flag;
            try
            {
                Process[] processesByName = Process.GetProcessesByName(name1);
                int index = 0;
                while (true)
                {
                    if (index >= processesByName.Length)
                    {
                        flag = true;
                        break;
                    }
                    Process process1 = processesByName[index];
                    process1.Kill();
                    process1.WaitForExit();
                    index++;
                }
            }
            catch (Exception)
            {
                flag = false;
            }
            return flag;
        }

        public static void DeleteDir(string file)
        {
            try
            {
                new DirectoryInfo(file).Attributes = 0;
                File.SetAttributes(file, FileAttributes.Normal);
                if (Directory.Exists(file))
                {
                    string[] fileSystemEntries = Directory.GetFileSystemEntries(file);
                    int index = 0;
                    while (true)
                    {
                        if (index >= fileSystemEntries.Length)
                        {
                            Directory.Delete(file);
                            break;
                        }
                        string path = fileSystemEntries[index];
                        if (!File.Exists(path))
                        {
                            DeleteDir(path);
                        }
                        else
                        {
                            File.Delete(path);
                            Console.WriteLine(path);
                        }
                        index++;
                    }
                }
            }
            catch (Exception exception1)
            {
                Console.WriteLine(exception1.Message.ToString());
            }
        }

        public static void GetMachineCode()
        {
            string path = Environment.CurrentDirectory + @"\MachineCode.ini";
            if (!File.Exists(path))
            {
                new FileStream(path, FileMode.Create, FileAccess.ReadWrite).Close();
                File.WriteAllText(path, Common.GetMachineCode());
            }
        }

        public bool startExe(string exePath, string exePathParent)
        {
            Process process1 = new Process();
            process1.StartInfo.FileName = exePath;
            process1.StartInfo.Arguments = "";
            process1.StartInfo.UseShellExecute = false;
            process1.StartInfo.RedirectStandardInput = true;
            process1.StartInfo.RedirectStandardOutput = true;
            process1.StartInfo.RedirectStandardError = true;
            process1.StartInfo.CreateNoWindow = true;
            process1.StartInfo.WorkingDirectory = exePathParent;
            process1.Start();
            return true;
        }

        public bool StartProcess(string path1)
        {
            try
            {
                Process.Start(path1);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public string UrlRelativeToAbsolute(string relative) => 
            Path.GetFullPath(relative);
    }
}

