namespace MicroPrinter.App.Api
{
    using MicroPrinter.App;
    using MicroPrinter.App.Views;
    using Newtonsoft.Json.Linq;
    using System;
    using System.Runtime.InteropServices;

    internal class Common
    {
        public static bool DownLoadStatusUpdate(string type, string status)
        {
            bool flag;
            string json = HttpUtils.Post(MicroPrinter.App.AppContext.Instance.BaseURL + "/update/notice/?type=" + type, "status=" + status, "");
            try
            {
                JObject obj2 = JObject.Parse(json);
                if ((obj2 == null) || (obj2["data"].ToString() == "[]"))
                {
                    flag = false;
                }
                else
                {
                    flag = obj2["data"]["needUpdate"].ToString() == "1";
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine("DownLoadStatusUpdate:" + exception.Message);
                flag = false;
            }
            return flag;
        }

        public static string GetMachineCode()
        {
            string str;
            string json = HttpUtils.Get(MicroPrinter.App.AppContext.Instance.BaseURL + "/init/?1=1", "-1");
            try
            {
                JObject obj2 = JObject.Parse(json);
                if ((obj2 == null) || (obj2["data"].ToString() == "[]"))
                {
                    str = string.Empty;
                }
                else
                {
                    str = obj2["data"]["machine_code"].ToString();
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine("GetMachineCode:" + exception.Message);
                str = string.Empty;
            }
            return str;
        }

        public static bool Report(string type, string report_id, string report_abstract_content = "", string report_abstract_id = "", string report_content = "", string order_id = "")
        {
            bool flag;
            string str = "&report_id=" + report_id;
            if (type.ToString() == "1")
            {
                str = str + "&order_id=" + order_id;
            }
            string json = HttpUtils.Post(MicroPrinter.App.AppContext.Instance.BaseURL + "/report/index/?type=" + type, ((str + "&report_abstract_content=" + report_abstract_content) + "&report_abstract_id=" + report_abstract_id) + "&report_content=" + report_content, "");
            try
            {
                JObject obj2 = JObject.Parse(json);
                if ((obj2 == null) || (obj2["data"].ToString() == "[]"))
                {
                    flag = false;
                }
                else
                {
                    MsgDisplay.ContentUpdate("抱歉：当前遇到问题，请换一台设备取件！");
                    flag = obj2["status"].ToString() == "1";
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine("Report:" + exception.Message);
                flag = false;
            }
            return flag;
        }

        public static bool WhetherNeedUpdate(string type)
        {
            bool flag;
            string json = HttpUtils.Get(MicroPrinter.App.AppContext.Instance.BaseURL + "/update/?type=" + type, "1");
            try
            {
                JObject obj2 = JObject.Parse(json);
                if ((obj2 == null) || (obj2["data"].ToString() == "[]"))
                {
                    flag = false;
                }
                else
                {
                    flag = obj2["data"]["needUpdate"].ToString() == "1";
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine("WhetherNeedUpdate: " + exception.Message);
                flag = false;
            }
            return flag;
        }
    }
}

