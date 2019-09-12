namespace MicroPrinter.App
{
    using log4net;
    using MicroPrinter.App.Api;
    using Spire.Pdf;
    using System;
    using System.Diagnostics;
    using System.Drawing;
    using System.Drawing.Printing;
    using System.Runtime.InteropServices;

    internal class PrintHelper
    {
        private static string strImagePath;
        public ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private static void ImagePrint_PrintPage(object sender, PrintPageEventArgs e)
        {
            int width = e.PageSettings.PaperSize.Width;
            int height = e.PageSettings.PaperSize.Height;
            if (e.PageSettings.Landscape)
            {
                width = e.PageSettings.PaperSize.Height;
                height = e.PageSettings.PaperSize.Width;
            }
            int num3 = width / height;
            Image local1 = Image.FromFile(strImagePath);
            int num4 = local1.Width;
            int num5 = local1.Height;
            int num6 = num4 / num5;
            if ((num4 * num5) <= (width * height))
            {
                if (num3 == num6)
                {
                    float num16 = Math.Min((float) ((width * 1f) / (num4 * 1f)), (float) ((height * 1f) / (num5 * 1f)));
                    float num17 = num4 * num16;
                    float num18 = num5 * num16;
                    Bitmap image = (Bitmap) Image.FromFile(strImagePath);
                    e.Graphics.DrawImage(image, (width - num17) / 2f, (height - num18) / 2f, num17, num18);
                }
                else
                {
                    float num12 = Math.Min((float) ((width * 1f) / (num5 * 1f)), (float) ((height * 1f) / (num4 * 1f)));
                    float num13 = num5 * num12;
                    float num14 = num4 * num12;
                    Bitmap image = (Bitmap) Image.FromFile(strImagePath);
                    image.RotateFlip(RotateFlipType.Rotate270FlipNone);
                    e.Graphics.DrawImage(image, (width - num13) / 2f, (height - num14) / 2f, num13, num14);
                }
            }
            else if (num3 == num6)
            {
                float num20 = Math.Min((float) ((width * 1f) / (num4 * 1f)), (float) ((height * 1f) / (num5 * 1f)));
                float num21 = num4 * num20;
                float num22 = num5 * num20;
                Bitmap image = (Bitmap) Image.FromFile(strImagePath);
                e.Graphics.DrawImage(image, (width - num21) / 2f, (height - num22) / 2f, num21, num22);
            }
            else
            {
                float num8 = Math.Min((float) ((width * 1f) / (num5 * 1f)), (float) ((height * 1f) / (num4 * 1f)));
                float num9 = num5 * num8;
                float num10 = num4 * num8;
                Bitmap image = (Bitmap) Image.FromFile(strImagePath);
                image.RotateFlip(RotateFlipType.Rotate270FlipNone);
                e.Graphics.DrawImage(image, (width - num9) / 2f, (height - num10) / 2f, num9, num10);
            }
        }

        private static void PhotoPrint_PrintPage(object sender, PrintPageEventArgs e)
        {
            int width = e.PageSettings.PaperSize.Width;
            int height = e.PageSettings.PaperSize.Height;
            if (e.PageSettings.Landscape)
            {
                width = e.PageSettings.PaperSize.Height;
                height = e.PageSettings.PaperSize.Width;
            }
            int num3 = width / height;
            Image local1 = Image.FromFile(strImagePath);
            int num4 = local1.Width;
            int num5 = local1.Height;
            int num6 = num4 / num5;
            if ((num4 * num5) <= (width * height))
            {
                if (num3 == num6)
                {
                    float num16 = Math.Min((float) ((width * 1f) / (num4 * 1f)), (float) ((height * 1f) / (num5 * 1f)));
                    float num17 = num4 * num16;
                    float num18 = num5 * num16;
                    Bitmap image = (Bitmap) Image.FromFile(strImagePath);
                    e.Graphics.DrawImage(image, (width - num17) / 2f, (height - num18) / 2f, num17, num18);
                }
                else
                {
                    float num12 = Math.Min((float) ((width * 1f) / (num5 * 1f)), (float) ((height * 1f) / (num4 * 1f)));
                    float num13 = num5 * num12;
                    float num14 = num4 * num12;
                    Bitmap image = (Bitmap) Image.FromFile(strImagePath);
                    image.RotateFlip(RotateFlipType.Rotate270FlipNone);
                    e.Graphics.DrawImage(image, (width - num13) / 2f, (height - num14) / 2f, num13, num14);
                }
            }
            else if (num3 == num6)
            {
                float num20 = Math.Min((float) ((width * 1f) / (num4 * 1f)), (float) ((height * 1f) / (num5 * 1f)));
                float num21 = num4 * num20;
                float num22 = num5 * num20;
                Bitmap image = (Bitmap) Image.FromFile(strImagePath);
                e.Graphics.DrawImage(image, (width - num21) / 2f, (height - num22) / 2f, num21, num22);
            }
            else
            {
                float num8 = Math.Min((float) ((width * 1f) / (num5 * 1f)), (float) ((height * 1f) / (num4 * 1f)));
                float num9 = num5 * num8;
                float num10 = num4 * num8;
                Bitmap image = (Bitmap) Image.FromFile(strImagePath);
                image.RotateFlip(RotateFlipType.Rotate270FlipNone);
                e.Graphics.DrawImage(image, (width - num9) / 2f, (height - num10) / 2f, num9, num10);
            }
        }

        public static bool PrintFileDirectly(string strFileName, string orderId = "")
        {
            bool flag2;
            try
            {
                Process process1 = new Process();
                process1.StartInfo.CreateNoWindow = true;
                process1.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                process1.StartInfo.UseShellExecute = true;
                process1.StartInfo.FileName = strFileName;
                process1.StartInfo.Verb = "print";
                process1.Start();
                flag2 = true;
            }
            catch (Exception exception)
            {
                string str = "打印出错：" + exception.Message;
                Console.WriteLine(str);
                new PrintHelper().log.Error("PrintFileDirectly:" + str + " orderId:" + orderId);
                return false;
            }
            return flag2;
        }

        public static bool PrintImage(string strFileName, short printCopies = 1, bool isColorful = true, string orderId = "")
        {
            bool flag;
            try
            {
                PrintDocument document = new PrintDocument();
                strImagePath = strFileName;
                document.PrinterSettings.PrinterName = MicroPrinter.App.AppContext.Instance.MachineSetting.DEFAULT_PRINTER;
                document.PrintController = new StandardPrintController();
                document.DefaultPageSettings.Color = isColorful;
                document.DefaultPageSettings.PrinterSettings.Copies = printCopies;
                if (!isColorful)
                {
                    foreach (PaperSource source in document.PrinterSettings.PaperSources)
                    {
                        if (source.SourceName.Equals(MicroPrinter.App.AppContext.Instance.MachineSetting.BW_A4_PAPER_SOURCE))
                        {
                            document.DefaultPageSettings.PaperSource = source;
                            break;
                        }
                    }
                    foreach (PaperSize size in document.PrinterSettings.PaperSizes)
                    {
                        if (size.PaperName.Equals(MicroPrinter.App.AppContext.Instance.MachineSetting.BW_A4_PAPER_SIZE))
                        {
                            document.DefaultPageSettings.PaperSize = size;
                            break;
                        }
                    }
                }
                else
                {
                    foreach (PaperSource source2 in document.PrinterSettings.PaperSources)
                    {
                        if (source2.SourceName.Equals(MicroPrinter.App.AppContext.Instance.MachineSetting.COLOR_A4_PAPER_SOURCE))
                        {
                            document.DefaultPageSettings.PaperSource = source2;
                            break;
                        }
                    }
                    foreach (PaperSize size2 in document.PrinterSettings.PaperSizes)
                    {
                        if (size2.PaperName.Equals(MicroPrinter.App.AppContext.Instance.MachineSetting.COLOR_A4_PAPER_SIZE))
                        {
                            document.DefaultPageSettings.PaperSize = size2;
                            break;
                        }
                    }
                }
                document.PrintPage += new PrintPageEventHandler(PrintHelper.ImagePrint_PrintPage);
                document.Print();
                return true;
            }
            catch (Exception exception)
            {
                string str = "打印图片出错：" + exception.Message;
                Console.WriteLine(str);
                new PrintHelper().log.Error("PrintImage:" + str + " orderId:" + orderId);
                flag = false;
            }
            return flag;
        }

        [Obsolete]
        public static bool PrintPdf(string strFileName, short printCopies = 1, bool isColorful = true, string orderId = "")
        {
            bool flag;
            try
            {
                PdfDocument document1 = new PdfDocument();
                document1.LoadFromFile(strFileName);
                PrintDocument printDocument = document1.PrintDocument;
                printDocument.PrinterSettings.PrinterName = MicroPrinter.App.AppContext.Instance.MachineSetting.DEFAULT_PRINTER;
                printDocument.DefaultPageSettings.Color = isColorful;
                printDocument.DefaultPageSettings.PrinterSettings.Copies = printCopies;
                printDocument.PrintController = new StandardPrintController();
                if (!isColorful)
                {
                    foreach (PaperSource source in printDocument.PrinterSettings.PaperSources)
                    {
                        if (source.SourceName.Equals(MicroPrinter.App.AppContext.Instance.MachineSetting.BW_A4_PAPER_SOURCE))
                        {
                            printDocument.DefaultPageSettings.PaperSource = source;
                            break;
                        }
                    }
                    foreach (PaperSize size in printDocument.PrinterSettings.PaperSizes)
                    {
                        if (size.PaperName.Equals(MicroPrinter.App.AppContext.Instance.MachineSetting.BW_A4_PAPER_SIZE))
                        {
                            printDocument.DefaultPageSettings.PaperSize = size;
                            break;
                        }
                    }
                }
                else
                {
                    foreach (PaperSource source2 in printDocument.PrinterSettings.PaperSources)
                    {
                        if (source2.SourceName.Equals(MicroPrinter.App.AppContext.Instance.MachineSetting.COLOR_A4_PAPER_SOURCE))
                        {
                            printDocument.DefaultPageSettings.PaperSource = source2;
                            break;
                        }
                    }
                    foreach (PaperSize size2 in printDocument.PrinterSettings.PaperSizes)
                    {
                        if (size2.PaperName.Equals(MicroPrinter.App.AppContext.Instance.MachineSetting.COLOR_A4_PAPER_SIZE))
                        {
                            printDocument.DefaultPageSettings.PaperSize = size2;
                            break;
                        }
                    }
                }
                printDocument.Print();
                return true;
            }
            catch (Exception exception)
            {
                string str = "打印PDF出错：" + exception.Message;
                Console.WriteLine(str);
                string str2 = "PrintPdf";
                string[] textArray1 = new string[] { str2, " :", str, " orderId:", orderId };
                string message = string.Concat(textArray1);
                new PrintHelper().log.Error(message);
                Common.Report("1", "1", str2, "1", message, orderId);
                flag = false;
            }
            return flag;
        }

        public static bool PrintPhoto(string strFileName, short printCopies = 1, string orderId = "")
        {
            bool flag2;
            try
            {
                PrintDocument document = new PrintDocument();
                strImagePath = strFileName;
                document.PrinterSettings.PrinterName = MicroPrinter.App.AppContext.Instance.MachineSetting.DEFAULT_PRINTER;
                document.PrintController = new StandardPrintController();
                document.DefaultPageSettings.Color = true;
                document.DefaultPageSettings.PrinterSettings.Copies = printCopies;
                document.DefaultPageSettings.Landscape = MicroPrinter.App.AppContext.Instance.MachineSetting.PHOTO_7INCH_LANDSCAPE;
                foreach (PaperSource source in document.PrinterSettings.PaperSources)
                {
                    if (source.SourceName.Equals(MicroPrinter.App.AppContext.Instance.MachineSetting.PHOTO_7INCH_PAPER_SOURCE))
                    {
                        document.DefaultPageSettings.PaperSource = source;
                        break;
                    }
                }
                foreach (PaperSize size in document.PrinterSettings.PaperSizes)
                {
                    if (size.PaperName.Equals(MicroPrinter.App.AppContext.Instance.MachineSetting.PHOTO_7INCH_PAPER_SIZE))
                    {
                        document.DefaultPageSettings.PaperSize = size;
                        break;
                    }
                }
                document.PrintPage += new PrintPageEventHandler(PrintHelper.PhotoPrint_PrintPage);
                document.Print();
                flag2 = true;
            }
            catch (Exception exception)
            {
                string str = "打印照片出错：" + exception.Message;
                Console.WriteLine(str);
                new PrintHelper().log.Error("PrintPhoto:" + str + " orderId:" + orderId);
                return false;
            }
            return flag2;
        }
    }
}

