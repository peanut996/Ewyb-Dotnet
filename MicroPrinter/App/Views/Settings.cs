namespace MicroPrinter.App.Views
{
    using DevExpress.Xpf.Editors;
    using DevExpress.Xpf.LayoutControl;
    using MicroPrinter.App;
    using Newtonsoft.Json;
    using System;
    using System.CodeDom.Compiler;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing.Printing;
    using System.IO;
    using System.Reflection;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Markup;

    public class Settings : UserControl, IComponentConnector
    {
        internal TextEdit MACHINECODETEXT;
        internal ComboBoxEdit DEFAULTPRINTCOMBO;
        internal CheckEdit BWA4PrintCheck;
        internal CheckEdit COLORA4PrintCheck;
        internal CheckEdit PHOTO7INCHPrintCheck;
        internal ComboBoxEdit BWA4PAPERSOURCECOMBO;
        internal ComboBoxEdit BWA4PAPERSIZECOMBO;
        internal ComboBoxEdit COLORA4PAPERSOURCECOMBO;
        internal ComboBoxEdit COLORA4PAPERSIZECOMBO;
        internal ComboBoxEdit PHOTO7INCHPAPERSOURCECOMBO;
        internal ComboBoxEdit PHOTO7INCHPAPERSIZECOMBO;
        internal CheckEdit PHOTO7INCHLANDSCAPE;
        internal LayoutGroup buttonGroup;
        internal Button CacelButton;
        internal Button SaveButton;
        private bool _contentLoaded;

        public Settings()
        {
            this.InitializeComponent();
        }

        private void CacelButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Close();
            Process.Start(Assembly.GetExecutingAssembly().Location);
        }

        [DebuggerNonUserCode, GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent()
        {
            if (!this._contentLoaded)
            {
                this._contentLoaded = true;
                Uri resourceLocator = new Uri("/MicroPrinter.App;component/microprinter.app/views/settings.xaml", UriKind.Relative);
                Application.LoadComponent(this, resourceLocator);
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            string contents = JsonConvert.SerializeObject(base.DataContext);
            string path = Environment.CurrentDirectory + @"\MachineSettings.json";
            if (!File.Exists(path))
            {
                new FileStream(path, FileMode.Create, FileAccess.ReadWrite).Close();
            }
            File.WriteAllText(path, contents);
            if (MessageBox.Show("保存成功，是否重新启动？", "提示", MessageBoxButton.YesNo, MessageBoxImage.Asterisk) == MessageBoxResult.Yes)
            {
                Application.Current.MainWindow.Close();
                Process.Start(Assembly.GetExecutingAssembly().Location);
            }
        }

        [DebuggerNonUserCode, GeneratedCode("PresentationBuildTasks", "4.0.0.0"), EditorBrowsable(EditorBrowsableState.Never)]
        void IComponentConnector.Connect(int connectionId, object target)
        {
            switch (connectionId)
            {
                case 1:
                    ((MicroPrinter.App.Views.Settings) target).Loaded += new RoutedEventHandler(this.UserControl_Loaded);
                    return;

                case 2:
                    this.MACHINECODETEXT = (TextEdit) target;
                    return;

                case 3:
                    this.DEFAULTPRINTCOMBO = (ComboBoxEdit) target;
                    return;

                case 4:
                    this.BWA4PrintCheck = (CheckEdit) target;
                    return;

                case 5:
                    this.COLORA4PrintCheck = (CheckEdit) target;
                    return;

                case 6:
                    this.PHOTO7INCHPrintCheck = (CheckEdit) target;
                    return;

                case 7:
                    this.BWA4PAPERSOURCECOMBO = (ComboBoxEdit) target;
                    return;

                case 8:
                    this.BWA4PAPERSIZECOMBO = (ComboBoxEdit) target;
                    return;

                case 9:
                    this.COLORA4PAPERSOURCECOMBO = (ComboBoxEdit) target;
                    return;

                case 10:
                    this.COLORA4PAPERSIZECOMBO = (ComboBoxEdit) target;
                    return;

                case 11:
                    this.PHOTO7INCHPAPERSOURCECOMBO = (ComboBoxEdit) target;
                    return;

                case 12:
                    this.PHOTO7INCHPAPERSIZECOMBO = (ComboBoxEdit) target;
                    return;

                case 13:
                    this.PHOTO7INCHLANDSCAPE = (CheckEdit) target;
                    return;

                case 14:
                    this.buttonGroup = (LayoutGroup) target;
                    return;

                case 15:
                    this.CacelButton = (Button) target;
                    this.CacelButton.Click += new RoutedEventHandler(this.CacelButton_Click);
                    return;

                case 0x10:
                    this.SaveButton = (Button) target;
                    this.SaveButton.Click += new RoutedEventHandler(this.SaveButton_Click);
                    return;
            }
            this._contentLoaded = true;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            base.DataContext = MicroPrinter.App.AppContext.Instance.MachineSetting;
            for (int i = 0; i < PrinterSettings.InstalledPrinters.Count; i++)
            {
                string item = PrinterSettings.InstalledPrinters[i];
                this.DEFAULTPRINTCOMBO.Items.Add(item);
            }
            PrintDocument document = new PrintDocument();
            foreach (PaperSource source in document.PrinterSettings.PaperSources)
            {
                this.BWA4PAPERSOURCECOMBO.Items.Add(source);
                this.COLORA4PAPERSOURCECOMBO.Items.Add(source);
                this.PHOTO7INCHPAPERSOURCECOMBO.Items.Add(source);
            }
            foreach (PaperSize size in document.PrinterSettings.PaperSizes)
            {
                this.BWA4PAPERSIZECOMBO.Items.Add(size);
                this.COLORA4PAPERSIZECOMBO.Items.Add(size);
                this.PHOTO7INCHPAPERSIZECOMBO.Items.Add(size);
            }
            foreach (PaperSize size2 in document.PrinterSettings.PaperSizes)
            {
                this.BWA4PAPERSIZECOMBO.Items.Add(size2);
                this.COLORA4PAPERSIZECOMBO.Items.Add(size2);
                this.PHOTO7INCHPAPERSIZECOMBO.Items.Add(size2);
            }
        }
    }
}

