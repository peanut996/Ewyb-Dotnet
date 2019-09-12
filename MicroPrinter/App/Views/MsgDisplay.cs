namespace MicroPrinter.App.Views
{
    using System;
    using System.CodeDom.Compiler;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Markup;

    public class MsgDisplay : UserControl, IComponentConnector
    {
        internal Grid MsgDispalyContainer;
        internal Label LABELCONTENT;
        private bool _contentLoaded;

        public MsgDisplay()
        {
            this.InitializeComponent();
            this.LABELCONTENT.Content = this.CurrentMsg = "拉取广告中，请耐心等待...";
        }

        public static void ContentUpdate(string msg = "")
        {
            Console.WriteLine("Hello~~~~" + msg + Owner.DataContext);
            Owner.LABELCONTENT.Content = Owner.CurrentMsg = msg;
        }

        [DebuggerNonUserCode, GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent()
        {
            if (!this._contentLoaded)
            {
                this._contentLoaded = true;
                Uri resourceLocator = new Uri("/MicroPrinter.App;component/microprinter.app/views/dispaly/msgdisplay.xaml", UriKind.Relative);
                Application.LoadComponent(this, resourceLocator);
            }
        }

        [DebuggerNonUserCode, GeneratedCode("PresentationBuildTasks", "4.0.0.0"), EditorBrowsable(EditorBrowsableState.Never)]
        void IComponentConnector.Connect(int connectionId, object target)
        {
            switch (connectionId)
            {
                case 1:
                    ((MsgDisplay) target).Loaded += new RoutedEventHandler(this.UserControl_Loaded);
                    return;

                case 2:
                    this.MsgDispalyContainer = (Grid) target;
                    return;

                case 3:
                    this.LABELCONTENT = (Label) target;
                    return;
            }
            this._contentLoaded = true;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Owner = this;
        }

        public string CurrentMsg { get; set; }

        public static MsgDisplay Owner
        {
            get => 
                <Owner>k__BackingField;
            private set => 
                (<Owner>k__BackingField = value);
        }
    }
}

