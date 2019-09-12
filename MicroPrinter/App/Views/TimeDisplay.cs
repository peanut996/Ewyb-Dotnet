namespace MicroPrinter.App.Views
{
    using System;
    using System.CodeDom.Compiler;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Markup;
    using System.Windows.Threading;

    public class TimeDisplay : UserControl, IComponentConnector
    {
        internal Grid TimeDispalyContainer;
        internal Label lbTimeDispaly;
        private bool _contentLoaded;

        public TimeDisplay()
        {
            this.InitializeComponent();
            base.DataContext = this;
            DispatcherTimer timer1 = new DispatcherTimer();
            timer1.Interval = TimeSpan.FromMilliseconds(500.0);
            timer1.Tick += new EventHandler(this.dayTimer_Tick);
            timer1.Start();
        }

        private void dayTimer_Tick(object sender, EventArgs e)
        {
            this.CurrentDateAndTime = DateTime.Now;
            this.lbTimeDispaly.Content = "  " + this.CurrentDateAndTime;
        }

        [DebuggerNonUserCode, GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent()
        {
            if (!this._contentLoaded)
            {
                this._contentLoaded = true;
                Uri resourceLocator = new Uri("/MicroPrinter.App;component/microprinter.app/views/dispaly/timedisplay.xaml", UriKind.Relative);
                Application.LoadComponent(this, resourceLocator);
            }
        }

        [DebuggerNonUserCode, GeneratedCode("PresentationBuildTasks", "4.0.0.0"), EditorBrowsable(EditorBrowsableState.Never)]
        void IComponentConnector.Connect(int connectionId, object target)
        {
            switch (connectionId)
            {
                case 1:
                    ((TimeDisplay) target).Loaded += new RoutedEventHandler(this.UserControl_Loaded);
                    return;

                case 2:
                    this.TimeDispalyContainer = (Grid) target;
                    return;

                case 3:
                    this.lbTimeDispaly = (Label) target;
                    return;
            }
            this._contentLoaded = true;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
        }

        public DateTime CurrentDateAndTime { get; set; }
    }
}

