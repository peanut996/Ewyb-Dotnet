namespace MicroPrinter.App.Views
{
    using System;
    using System.CodeDom.Compiler;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Markup;

    public class InTask : UserControl, IComponentConnector
    {
        internal Grid TaskContainer;
        internal Image PrinterIcon;
        internal Label PrinterStatusLabel;
        internal Image UserIcon;
        internal Label UserNameLabel;
        internal Image DocumentsIcon;
        internal Label PrintAmountLabel;
        internal Label PrintColorLabel;
        internal Label PrintPageLabel;
        internal Label PrintDualLabel;
        internal Label PrintRangeLabel;
        internal Label PrintPagesCountLabel;
        internal Image ProcessIcon;
        internal ProgressBar PrintProcessBar;
        private bool _contentLoaded;

        public InTask()
        {
            this.InitializeComponent();
        }

        [DebuggerNonUserCode, GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent()
        {
            if (!this._contentLoaded)
            {
                this._contentLoaded = true;
                Uri resourceLocator = new Uri("/MicroPrinter.App;component/microprinter.app/views/main/task/intask.xaml", UriKind.Relative);
                Application.LoadComponent(this, resourceLocator);
            }
        }

        [DebuggerNonUserCode, GeneratedCode("PresentationBuildTasks", "4.0.0.0"), EditorBrowsable(EditorBrowsableState.Never)]
        void IComponentConnector.Connect(int connectionId, object target)
        {
            switch (connectionId)
            {
                case 1:
                    this.TaskContainer = (Grid) target;
                    return;

                case 2:
                    this.PrinterIcon = (Image) target;
                    return;

                case 3:
                    this.PrinterStatusLabel = (Label) target;
                    return;

                case 4:
                    this.UserIcon = (Image) target;
                    return;

                case 5:
                    this.UserNameLabel = (Label) target;
                    return;

                case 6:
                    this.DocumentsIcon = (Image) target;
                    return;

                case 7:
                    this.PrintAmountLabel = (Label) target;
                    return;

                case 8:
                    this.PrintColorLabel = (Label) target;
                    return;

                case 9:
                    this.PrintPageLabel = (Label) target;
                    return;

                case 10:
                    this.PrintDualLabel = (Label) target;
                    return;

                case 11:
                    this.PrintRangeLabel = (Label) target;
                    return;

                case 12:
                    this.PrintPagesCountLabel = (Label) target;
                    return;

                case 13:
                    this.ProcessIcon = (Image) target;
                    return;

                case 14:
                    this.PrintProcessBar = (ProgressBar) target;
                    return;
            }
            this._contentLoaded = true;
        }
    }
}

