namespace MicroPrinter.App.Views
{
    using log4net;
    using MicroPrinter.App;
    using MicroPrinter.App.Api;
    using MicroPrinter.App.Model;
    using MicroPrinter.App.Utils;
    using Newtonsoft.Json.Linq;
    using System;
    using System.CodeDom.Compiler;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Markup;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using System.Windows.Threading;

    public class Advertisement : UserControl, IComponentConnector
    {
        private ILog log;
        private readonly TaskScheduler _syncContextTaskScheduler = TaskScheduler.FromCurrentSynchronizationContext();
        private Dictionary<string, WybAdvGroup> commonGroups;
        private WybAdvGroup temporaryGroup;
        private bool isDefaultAdvHandled;
        private TaskFactory taskFactory = new TaskFactory();
        private CancellationTokenSource cancellationToken = new CancellationTokenSource();
        private DispatcherTimer timer = new DispatcherTimer();
        private bool isTemporary;
        private string advertismentType = "DEFAULT";
        private List<string> downloadedFiles;
        private List<WybAdvGroupItem> commAdvItems;
        private int commonPlayIndex;
        private int commonOrderIndex;
        private WybAdvGroupItem tempAdvItem;
        private HttpUtils httpUtils;
        private MediaElement mediaElement;
        private int durTime = 15;
        internal Grid AdvertisementContainer;
        private bool _contentLoaded;

        public Advertisement()
        {
            this.InitializeComponent();
            this.commonGroups = new Dictionary<string, WybAdvGroup>();
            this.timer.Tick += new EventHandler(this.Timer_Tick);
            this.timer.Start();
            this.downloadedFiles = new List<string>();
            this.log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
            this.httpUtils = new HttpUtils();
            this.mediaElement = new MediaElement();
            this.mediaElement.MediaEnded += new RoutedEventHandler(this.AdvertisementMediaContainer_MediaEnded);
        }

        private void _Timer_Tick(bool continuePlay = false)
        {
            this.AdvertismentDirDelete();
            this.CommonAdvertismentRemove();
            Console.WriteLine("定时器触发 Timer_Tick,当前时间：" + $"{DateTime.Now}");
            string str3 = @"\download\";
            this.timer.Interval = new TimeSpan(0, 0, 0, this.durTime);
            this.timer.Stop();
            string baseURL = MicroPrinter.App.AppContext.Instance.BaseURL;
            bool flag = Common.WhetherNeedUpdate("getDefault");
            try
            {
                if (this.isTemporary && (this.temporaryGroup != null))
                {
                    this.temporaryGroup = null;
                    this.isTemporary = false;
                }
                if (this.temporaryGroup != null)
                {
                    str3 = @"\download\adv\tempAdv\";
                    Console.WriteLine("开始处理临时广告的播放 downloadPath: " + str3);
                    WybAdvGroupItem item = this.temporaryGroup.wybAdvGroupItems.Values.First<WybAdvGroupItem>();
                    if (item.Type.Equals("10"))
                    {
                        string url = baseURL + "/file/download/?from=1&ftype=2&source=" + item.FileList.First<string>();
                        char[] separator = new char[] { '.' };
                        string[] textArray1 = item.FileList.First<string>().Split(separator);
                        string str6 = textArray1[textArray1.Length - 1].ToLower();
                        string[] textArray2 = new string[5];
                        textArray2[0] = Environment.CurrentDirectory;
                        textArray2[1] = str3;
                        Guid guid = Guid.NewGuid();
                        textArray2[2] = guid.ToString();
                        textArray2[3] = ".";
                        textArray2[4] = str6;
                        string path = string.Concat(textArray2);
                        if (this.httpUtils.HttpDownload(url, path))
                        {
                            Console.WriteLine("播放临时广告-图片");
                            this.UpdateImage(path);
                        }
                    }
                    else if (!item.Type.Equals("20"))
                    {
                        item.Type.Equals("30");
                    }
                    else
                    {
                        foreach (string local1 in item.FileList)
                        {
                            string url = baseURL + "/file/download/?from=2&ftype=2&source=" + item.FileList.First<string>();
                            char[] separator = new char[] { '.' };
                            string[] textArray3 = item.FileList.First<string>().Split(separator);
                            string str9 = textArray3[textArray3.Length - 1].ToLower();
                            string[] textArray4 = new string[5];
                            textArray4[0] = Environment.CurrentDirectory;
                            textArray4[1] = str3;
                            string[] strArray = textArray4;
                            strArray[2] = Guid.NewGuid().ToString();
                            strArray[3] = ".";
                            strArray[4] = str9;
                            string str5 = string.Concat(strArray);
                            if (this.httpUtils.HttpDownload(url, str5))
                            {
                                Console.WriteLine("播放临时广告的图片序列");
                                this.taskFactory.StartNew(() => this.UpdateImage(str5), new CancellationTokenSource().Token, TaskCreationOptions.None, this._syncContextTaskScheduler).Wait();
                            }
                        }
                    }
                    this.isTemporary = true;
                }
                else if (!this.isDefaultAdvHandled | flag)
                {
                    string path = Environment.CurrentDirectory + @"\Resources\media\default.avi";
                    Console.WriteLine("开始处理默认广告的播放  是否已经拉取过默认数据:" + this.isDefaultAdvHandled.ToString());
                    Console.Write("，  地址：" + path);
                    Console.WriteLine("，  是否需要强制更新：" + flag.ToString());
                    if (!File.Exists(path))
                    {
                        Console.WriteLine("开始处理默认广告的播放 本地默认文件不存在：" + path);
                        this.handleGetDefault(baseURL, path);
                    }
                    else
                    {
                        Console.WriteLine("开始处理默认广告的播放 本地默认文件存在：" + path);
                        if (!flag)
                        {
                            Console.Write("，不需要从服务器更新默认广告");
                            this.UpdateMedia(new FileInfo(path).FullName);
                        }
                        else
                        {
                            Console.Write("，需要从服务器更新默认广告");
                            this.log.Info("needUpdateGetDefault 删除同名文件:" + path);
                            File.Delete(path);
                            this.handleGetDefault(baseURL, path);
                        }
                    }
                    this.isDefaultAdvHandled = true;
                }
                else
                {
                    str3 = @"\download\adv\commonAdv\";
                    Console.WriteLine("开始处理常规广告的播放 downloadPath: " + str3);
                    if ((((this.commonPlayIndex == 0) && (this.commonOrderIndex == 0)) | (this.commonOrderIndex >= (this.commAdvItems[this.commonPlayIndex].PlayOrder + this.commAdvItems[this.commonPlayIndex].DurationCount))) | continuePlay)
                    {
                        if (this.commonPlayIndex == (this.commAdvItems.Count<WybAdvGroupItem>() - 1))
                        {
                            Console.WriteLine("常规轮回");
                            this.commonPlayIndex = 0;
                            this.commonOrderIndex = 0;
                        }
                        else if ((this.commonPlayIndex != 0) || (this.commonOrderIndex != 0))
                        {
                            this.commonPlayIndex++;
                        }
                        else
                        {
                            this.commonPlayIndex = 0;
                        }
                        WybAdvGroupItem item2 = this.commAdvItems[this.commonPlayIndex];
                        string fileURL = item2.FileList.First<string>();
                        if (fileURL != null)
                        {
                            if (item2.Type.Equals("10"))
                            {
                                string str;
                                string str12 = this.getFileRealName(fileURL);
                                string url = baseURL + "/file/download/?from=3&ftype=2&source=" + fileURL;
                                char[] separator = new char[] { '.' };
                                string[] textArray5 = fileURL.Split(separator);
                                textArray5[textArray5.Length - 1].ToLower();
                                if ((item2.RealFileList.Count<string>() != 0) && !string.IsNullOrEmpty(item2.RealFileList.First<string>()))
                                {
                                    str = item2.RealFileList.First<string>();
                                }
                                else
                                {
                                    str = Environment.CurrentDirectory + str3 + str12;
                                    if (!File.Exists(str) && this.httpUtils.HttpDownload(url, str))
                                    {
                                        this.downloadedFiles.Add(str);
                                    }
                                }
                                Console.WriteLine("from=3 10 播放常规广告-图片");
                                this.UpdateImage(str);
                            }
                            else if (!item2.Type.Equals("20") && item2.Type.Equals("30"))
                            {
                                string str2;
                                string str14 = this.getFileRealName(fileURL);
                                string url = baseURL + "/file/download/?from=4&ftype=2&source=" + fileURL;
                                char[] separator = new char[] { '.' };
                                string[] textArray6 = fileURL.Split(separator);
                                textArray6[textArray6.Length - 1].ToLower();
                                if ((item2.RealFileList.Count<string>() != 0) && !string.IsNullOrEmpty(item2.RealFileList.First<string>()))
                                {
                                    str2 = item2.RealFileList.First<string>();
                                }
                                else
                                {
                                    str2 = Environment.CurrentDirectory + str3 + str14;
                                    if (!File.Exists(str2) && this.httpUtils.HttpDownload(url, str2))
                                    {
                                        this.downloadedFiles.Add(str2);
                                    }
                                }
                                Console.WriteLine("from=4 30 播放常规广告-视频");
                                this.UpdateMedia(str2);
                            }
                        }
                    }
                    this.commonOrderIndex++;
                }
            }
            catch (Exception exception)
            {
                this.log.Error("Timer_Tick: " + exception.Message);
            }
            this.timer.Start();
        }

        private void AdvertisementMediaContainer_MediaEnded(object sender, RoutedEventArgs e)
        {
            try
            {
                MediaElement element = sender as MediaElement;
                if (element != null)
                {
                    element.Stop();
                    this._Timer_Tick(true);
                }
            }
            catch (Exception exception)
            {
                this.log.Error("AdvertisementMediaContainer_MediaEnded: " + exception.Message);
                MessageBox.Show(exception.Message);
            }
        }

        private void AdvertismentDirDelete()
        {
            string machineCode = MicroPrinter.App.AppContext.Instance.MachineCode;
            string json = HttpUtils.Get(MicroPrinter.App.AppContext.Instance.BaseURL + "/adv/dirDelete/?1=1", "1");
            try
            {
                JObject obj2 = JObject.Parse(json);
                if ((obj2 == null) || (obj2["data"].ToString() == "[]"))
                {
                    Console.WriteLine("AdvertismentDirDelete:deewe");
                    MsgDisplay.ContentUpdate("提示：" + obj2["msg"].ToString());
                }
                else
                {
                    string str2 = @"\download\adv\";
                    JArray source = obj2["data"]["items"] as JArray;
                    for (int i = 0; i < source.Count<JToken>(); i++)
                    {
                        JObject obj1 = source[i] as JObject;
                        string str3 = obj1["status"].ToString();
                        string str4 = obj1["path"].ToString();
                        Console.WriteLine("status:" + str3, ",path:" + str4);
                        string file = Environment.CurrentDirectory + str2 + str4;
                        if (str3 == "true")
                        {
                            CommonUtil.DeleteDir(file);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                this.log.Error("AdvertismentDirDelete: " + exception.Message);
            }
        }

        private void CommonAdvertismentRemove()
        {
            string machineCode = MicroPrinter.App.AppContext.Instance.MachineCode;
            string json = HttpUtils.Get(MicroPrinter.App.AppContext.Instance.BaseURL + "/adv/commonRemove/?1=1", "1");
            try
            {
                JObject obj2 = JObject.Parse(json);
                if ((obj2 == null) || (obj2["data"].ToString() == "[]"))
                {
                    Console.WriteLine("CommonAdvertismentRemove:deewe");
                }
                else
                {
                    string str2 = @"\download\adv\commonAdv\";
                    JArray source = obj2["data"]["items"] as JArray;
                    for (int i = 0; i < source.Count<JToken>(); i++)
                    {
                        string fileURL = (source[i] as JObject)["fileUrls"].ToString();
                        Console.WriteLine("source:" + fileURL);
                        string str4 = this.getFileRealName(fileURL);
                        Console.WriteLine("fileRealName1:" + str4);
                        string path = Environment.CurrentDirectory + str2 + str4;
                        Console.WriteLine("deleteFile:" + path);
                        if (File.Exists(path))
                        {
                            File.Delete(path);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                this.log.Error("CommonAdvertismentRemove: " + exception.Message);
            }
        }

        private void CommonAdvertismentUpdate()
        {
            string baseURL = MicroPrinter.App.AppContext.Instance.BaseURL;
            string machineCode = MicroPrinter.App.AppContext.Instance.MachineCode;
            Console.WriteLine("共用广告更新 MachineCode:" + machineCode + " end");
            if ((machineCode != null) && (machineCode != string.Empty))
            {
                while (!this.cancellationToken.IsCancellationRequested)
                {
                    string json = HttpUtils.Get(baseURL + "/adv/common/?1=1", "1");
                    try
                    {
                        JObject obj2 = JObject.Parse(json);
                        if ((obj2 == null) || (obj2["data"].ToString() == "[]"))
                        {
                            Console.WriteLine("CommonAdvertismentUpdate:i8d2dioedew");
                            MsgDisplay.ContentUpdate("提示：" + obj2["msg"].ToString());
                        }
                        else
                        {
                            JArray source = obj2["data"]["items"] as JArray;
                            this.commAdvItems = new List<WybAdvGroupItem>();
                            int num2 = 0;
                            while (true)
                            {
                                if (num2 >= source.Count<JToken>())
                                {
                                    this.commAdvItems.Sort((x, y) => x.Order.CompareTo(y.Order));
                                    int num = 0;
                                    for (int i = 1; i < this.commAdvItems.Count<WybAdvGroupItem>(); i++)
                                    {
                                        this.commAdvItems[i].PlayOrder = num + this.commAdvItems[i - 1].DurationCount;
                                        num += this.commAdvItems[i - 1].DurationCount;
                                    }
                                    break;
                                }
                                JObject obj3 = source[num2] as JObject;
                                WybAdvGroupItem item1 = new WybAdvGroupItem();
                                item1.Id = obj3["id"].ToString();
                                item1.Order = (int.Parse(obj3["inGroupOrder"].ToString()) * 20) + (num2 * 10);
                                item1.Type = obj3["type"].ToString();
                                item1.Duration = int.Parse(obj3["duration"].ToString());
                                WybAdvGroupItem item = item1;
                                item.DurationCount = (int) Math.Ceiling((double) ((item.Duration * 1f) / 10f));
                                item.Thumbnail = obj3["thumbnail"].ToString();
                                char[] separator = new char[] { ';' };
                                item.FileList = obj3["fileUrls"].ToString().Split(separator).ToList<string>();
                                this.commAdvItems.Add(item);
                                num2++;
                            }
                        }
                    }
                    catch (Exception exception)
                    {
                        this.log.Error("CommonAdvertismentUpdate: " + exception.Message);
                    }
                    Thread.Sleep(0x2bf20);
                }
            }
        }

        private string getFileRealName(string fileURL)
        {
            char[] separator = new char[] { '/' };
            string[] source = fileURL.Split(separator);
            if (source.Count<string>() > 1)
            {
                return source.Last<string>();
            }
            char[] chArray2 = new char[] { '\\' };
            source = fileURL.Split(chArray2);
            return ((source.Count<string>() > 1) ? source.Last<string>() : string.Empty);
        }

        private void handleGetDefault(string baseURL, string path)
        {
            string type = "getDefault";
            string url = baseURL + "/file/" + type + "/?1=1";
            Console.WriteLine("开始处理默认广告的播放 本地默认文件不存在,尝试下载：" + path);
            if (!this.httpUtils.HttpDownload(url, path))
            {
                MsgDisplay.ContentUpdate("拉取失败，请联系管理员");
                Console.WriteLine("开始处理默认广告的播放  本地默认文件不存在：尝试下载失败！" + path);
            }
            else
            {
                Console.WriteLine("拉取成功 尝试更新资源");
                MsgDisplay.ContentUpdate("拉取成功 尝试更新资源");
                Common.DownLoadStatusUpdate(type, "1");
                this.UpdateMedia(new FileInfo(path).FullName);
            }
        }

        [DebuggerNonUserCode, GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent()
        {
            if (!this._contentLoaded)
            {
                this._contentLoaded = true;
                Uri resourceLocator = new Uri("/MicroPrinter.App;component/microprinter.app/views/main/advertisement.xaml", UriKind.Relative);
                Application.LoadComponent(this, resourceLocator);
            }
        }

        private void ScheduleWorks()
        {
            this.taskFactory.StartNew(new Action(this.CommonAdvertismentUpdate), this.cancellationToken.Token);
            this.taskFactory.StartNew(new Action(this.TemporaryAdvertismentUpdate), this.cancellationToken.Token);
        }

        [DebuggerNonUserCode, GeneratedCode("PresentationBuildTasks", "4.0.0.0"), EditorBrowsable(EditorBrowsableState.Never)]
        void IComponentConnector.Connect(int connectionId, object target)
        {
            if (connectionId == 1)
            {
                ((Advertisement) target).Loaded += new RoutedEventHandler(this.UserControl_Loaded);
            }
            else if (connectionId != 2)
            {
                this._contentLoaded = true;
            }
            else
            {
                this.AdvertisementContainer = (Grid) target;
            }
        }

        private void TemporaryAdvertismentUpdate()
        {
            string baseURL = MicroPrinter.App.AppContext.Instance.BaseURL;
            while (!this.cancellationToken.IsCancellationRequested)
            {
                Thread.Sleep(0x1388);
                if (this.temporaryGroup == null)
                {
                    try
                    {
                        JObject obj2 = JObject.Parse(HttpUtils.Get(baseURL + "/adv/temporary/?1=1", "1"));
                        if (obj2 == null)
                        {
                            continue;
                        }
                        if (obj2["data"].ToString() == "[]")
                        {
                            continue;
                        }
                        JObject obj3 = obj2["data"]["group"] as JObject;
                        JObject obj4 = obj2["data"]["groupitem"] as JObject;
                        if (((this.temporaryGroup == null) || (this.temporaryGroup.Id == null)) || !this.temporaryGroup.Id.Equals(obj3["id"].ToString()))
                        {
                            WybAdvGroup group1 = new WybAdvGroup();
                            group1.Id = obj3["id"].ToString();
                            group1.Name = obj3["name"].ToString();
                            this.temporaryGroup = group1;
                            WybAdvGroupItem item2 = new WybAdvGroupItem {
                                Id = obj4["id"].ToString(),
                                Type = obj4["type"].ToString(),
                                Duration = int.Parse(obj4["duration"].ToString()),
                                Thumbnail = obj4["thumbnail"].ToString()
                            };
                            char[] separator = new char[] { ';' };
                            item2.FileList = obj4["fileUrls"].ToString().Split(separator).ToList<string>();
                            WybAdvGroupItem item = item2;
                            this.temporaryGroup.wybAdvGroupItems.Add(item.Id, item);
                            Console.WriteLine("获取临时广告 [" + this.temporaryGroup.Id + "] 成功!");
                        }
                        string[] textArray1 = new string[] { baseURL, "/adv/removeTemporary/?1=1", "&groupID=", this.temporaryGroup.Id };
                        obj2 = JObject.Parse(HttpUtils.Get(string.Concat(textArray1), "1"));
                        if ((obj2 == null) && !bool.Parse(obj2["success"].ToString()))
                        {
                            Console.WriteLine("取消临时广告 [" + this.temporaryGroup.Id + "] 失败!");
                        }
                        else if (bool.Parse(obj2["success"].ToString()))
                        {
                            Console.WriteLine("取消临时广告 [" + this.temporaryGroup.Id + "] 成功!");
                        }
                    }
                    catch (Exception exception)
                    {
                        this.log.Error("TemporaryAdvertismentUpdate: " + exception.Message);
                    }
                }
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            this._Timer_Tick(false);
        }

        private void UpdateImage(string source)
        {
            try
            {
                if (File.Exists(source))
                {
                    BitmapImage image = new BitmapImage(new Uri(source, UriKind.Absolute));
                    Image image1 = new Image();
                    image1.Source = image;
                    image1.Stretch = Stretch.Fill;
                    Image element = image1;
                    this.AdvertisementContainer.Children.Clear();
                    this.AdvertisementContainer.Children.Add(element);
                    element.HorizontalAlignment = HorizontalAlignment.Stretch;
                    element.VerticalAlignment = VerticalAlignment.Stretch;
                }
            }
            catch (Exception exception)
            {
                this.log.Error("UpdateImage: " + exception.Message + "source:" + source);
            }
        }

        private void UpdateMedia(string source)
        {
            try
            {
                if (File.Exists(source))
                {
                    this.mediaElement.LoadedBehavior = MediaState.Manual;
                    this.mediaElement.UnloadedBehavior = MediaState.Manual;
                    this.mediaElement.Close();
                    GC.Collect();
                    this.mediaElement.Source = new Uri(source);
                    this.AdvertisementContainer.Children.Clear();
                    this.AdvertisementContainer.Children.Add(this.mediaElement);
                    this.mediaElement.HorizontalAlignment = HorizontalAlignment.Stretch;
                    this.mediaElement.VerticalAlignment = VerticalAlignment.Stretch;
                    this.mediaElement.Play();
                }
                MsgDisplay.ContentUpdate("");
            }
            catch (Exception exception)
            {
                string msg = "UpdateMedia: " + exception.Message;
                MsgDisplay.ContentUpdate(msg);
                this.log.Error(msg);
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                this.taskFactory.StartNew(new Action(this.ScheduleWorks));
            }
            catch (Exception exception1)
            {
                MessageBox.Show(exception1.Message);
            }
        }

        [Serializable, CompilerGenerated]
        private sealed class <>c
        {
            public static readonly Advertisement.<>c <>9 = new Advertisement.<>c();
            public static Comparison<WybAdvGroupItem> <>9__29_0;

            internal int <CommonAdvertismentUpdate>b__29_0(WybAdvGroupItem x, WybAdvGroupItem y) => 
                x.Order.CompareTo(y.Order);
        }
    }
}

