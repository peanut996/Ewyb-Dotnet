namespace MicroPrinter.App
{
    using log4net;
    using MicroPrinter.App.Views;
    using Newtonsoft.Json.Linq;
    using System;
    using System.IO;
    using System.Net;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Text;

    public class HttpUtils
    {
        private ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public static string Get(string Url, string needMachineCode = "1")
        {
            string json = "";
            try
            {
                string machineCode = MicroPrinter.App.AppContext.Instance.MachineCode;
                string[] textArray1 = new string[] { "GetStart ", Url, " MachineCode:", machineCode, "; needMachineCode: ", needMachineCode, "; GetEnd" };
                Console.WriteLine(string.Concat(textArray1));
                if (((needMachineCode != "1") || (machineCode == string.Empty)) && (needMachineCode != "-1"))
                {
                    Console.WriteLine("Get cannot send");
                    string[] textArray2 = new string[] { " Get 请求未发出 ：机器码 不存在，接口且要求传机器码！！！" };
                    Console.WriteLine(string.Concat(textArray2));
                }
                else
                {
                    Console.WriteLine("Get can send");
                    string text1 = Url + MicroPrinter.App.AppContext.Instance.UrlCommon;
                    Url = text1;
                    try
                    {
                        Console.WriteLine("try to send Get request start");
                        HttpWebRequest request = (HttpWebRequest) WebRequest.Create(Url);
                        request.Proxy = null;
                        request.KeepAlive = false;
                        request.Method = "GET";
                        request.ContentType = "application/json; charset=UTF-8";
                        request.AutomaticDecompression = DecompressionMethods.GZip;
                        HttpWebResponse response = (HttpWebResponse) request.GetResponse();
                        StreamReader reader1 = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                        string str3 = reader1.ReadToEnd();
                        reader1.Close();
                        Stream responseStream = response.GetResponseStream();
                        responseStream.Close();
                        if (response != null)
                        {
                            response.Close();
                        }
                        if (request != null)
                        {
                            request.Abort();
                        }
                        json = str3;
                        Console.WriteLine("try to send Get request end");
                    }
                    catch (Exception exception)
                    {
                        Console.WriteLine("Get 网络异常: " + exception.Message);
                        MsgDisplay.ContentUpdate("提示：网络异常,请检查网络连接");
                        json = null;
                    }
                    JObject obj2 = JObject.Parse(json);
                    Console.Write("当前时间：" + $"{DateTime.Now}");
                    Console.WriteLine(" Get 请求服务器 from : " + Url);
                    Console.Write("当前时间：" + $"{DateTime.Now}");
                    Console.WriteLine(" Get 服务器返回：" + obj2);
                    if (obj2["status"].ToString() != "1")
                    {
                        MsgDisplay.ContentUpdate("提示：" + obj2["msg"].ToString());
                    }
                }
            }
            catch (Exception exception2)
            {
                Console.WriteLine("Get Exception: " + exception2.Message);
                json = null;
            }
            return json;
        }

        public bool HttpDownload(string Url, string path)
        {
            bool flag = false;
            try
            {
                string text1 = Url + MicroPrinter.App.AppContext.Instance.UrlCommon;
                Url = text1;
                Console.Write("当前时间：" + $"{DateTime.Now}");
                Console.WriteLine(" 从服务器下载文件 : " + Url);
                string text2 = Path.GetDirectoryName(path) + @"\temp";
                Directory.CreateDirectory(text2);
                string str = text2 + @"\" + Path.GetFileName(path) + ".temp";
                if (System.IO.File.Exists(str))
                {
                    this.log.Info("HttpDownload 删除临时文件:" + path);
                    this.log.Debug("HttpDownload 删除临时文件:" + path);
                    System.IO.File.Delete(str);
                }
                if (System.IO.File.Exists(path))
                {
                    this.log.Info("HttpDownload 删除同名文件:" + path);
                    this.log.Debug("HttpDownload 删除同名文件:" + path);
                    System.IO.File.Delete(path);
                }
                Console.Write("当前时间：" + $"{DateTime.Now}");
                Console.WriteLine(" 尝试从服务器下载文件 开始: ");
                FileStream stream = new FileStream(str, FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
                HttpWebRequest request1 = (HttpWebRequest) WebRequest.Create(Url);
                request1.Proxy = null;
                Stream responseStream = request1.GetResponse().GetResponseStream();
                byte[] buffer = new byte[0x1000];
                int count = responseStream.Read(buffer, 0, buffer.Length);
                while (true)
                {
                    if (count <= 0)
                    {
                        stream.Close();
                        responseStream.Close();
                        System.IO.File.Move(str, path);
                        flag = true;
                        Console.Write("当前时间：" + $"{DateTime.Now}");
                        Console.WriteLine(" 尝试从服务器下载文件 结果 : " + flag.ToString());
                        break;
                    }
                    stream.Write(buffer, 0, count);
                    count = responseStream.Read(buffer, 0, buffer.Length);
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine("HttpDownload Exception: " + exception.Message);
                flag = false;
            }
            return flag;
        }

        public static string Post(string Url, string Data, string Referer = "")
        {
            string str;
            try
            {
                string text1 = Url + MicroPrinter.App.AppContext.Instance.UrlCommon;
                Url = text1;
                HttpWebRequest request = (HttpWebRequest) WebRequest.Create(Url);
                request.Method = "POST";
                request.Referer = Referer;
                byte[] bytes = Encoding.UTF8.GetBytes(Data);
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = bytes.Length;
                Stream requestStream = request.GetRequestStream();
                requestStream.Write(bytes, 0, bytes.Length);
                HttpWebResponse response = (HttpWebResponse) request.GetResponse();
                StreamReader reader1 = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                string str2 = reader1.ReadToEnd();
                reader1.Close();
                requestStream.Close();
                if (response != null)
                {
                    response.Close();
                }
                if (request != null)
                {
                    request.Abort();
                }
                str = str2;
                JObject obj2 = JObject.Parse(str);
                Console.Write("当前时间：" + $"{DateTime.Now}");
                Console.WriteLine(" Post 请求服务器 from : " + Url);
                Console.Write("当前时间：" + $"{DateTime.Now}");
                Console.WriteLine(" Post 服务器返回：" + obj2);
            }
            catch (Exception exception)
            {
                Console.WriteLine("POST Exception: " + exception.Message);
                str = null;
            }
            return str;
        }
    }
}

