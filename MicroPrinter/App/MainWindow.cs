namespace MicroPrinter.App
{
    using MicroPrinter.App.Views;
    using System;
    using System.CodeDom.Compiler;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Markup;

    public class MainWindow : Window, IComponentConnector
    {
        private MainView mainView;
        private TaskListView taskListView;
        private PrinterStatusView printerStatusView;
        private TestView testView;
        private Settings settingsView;
        internal Grid MainContainer;
        private bool _contentLoaded;

        public MainWindow()
        {
            if (!this._contentLoaded)
            {
                this._contentLoaded = true;
                Application.LoadComponent(this, new Uri("/MicroPrinter.App;component/mainwindow.xaml", UriKind.Relative));
            }
        }

        [DebuggerNonUserCode, GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent()
        {
            if (!this._contentLoaded)
            {
                this._contentLoaded = true;
                Uri resourceLocator = new Uri("/MicroPrinter.App;component/mainwindow.xaml", UriKind.Relative);
                Application.LoadComponent(this, resourceLocator);
            }
        }

        [DebuggerNonUserCode, GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
        private void SwitchMainView(UserControl view)
        {
            base.WindowState = WindowState.Maximized;
            this.MainContainer.Children.Clear();
            this.MainContainer.Children.Add(view);
        }

        [DebuggerNonUserCode, GeneratedCode("PresentationBuildTasks", "4.0.0.0"), EditorBrowsable(EditorBrowsableState.Never)]
        void IComponentConnector.Connect(int connectionId, object target)
        {
            if (connectionId == 1)
            {
                ((MainWindow) target).KeyUp += new KeyEventHandler(this.Window_KeyUp);
                ((MainWindow) target).Loaded += new RoutedEventHandler(this.Window_Loaded);
            }
            else if (connectionId != 2)
            {
                this._contentLoaded = true;
            }
            else
            {
                this.MainContainer = (Grid) target;
            }
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            Key key = e.Key;
            if ((key == Key.Home) || (key == Key.F12))
            {
                base.Topmost = true;
                base.WindowState = WindowState.Maximized;
                base.WindowStyle = WindowStyle.None;
                this.SwitchMainView(this.mainView);
            }
            else if (key == Key.Escape)
            {
                Application.Current.MainWindow.Close();
            }
            else
            {
                switch (key)
                {
                    case Key.F1:
                        this.SwitchMainView(this.settingsView);
                        return;

                    case Key.F2:
                        this.SwitchMainView(this.taskListView);
                        return;

                    case Key.F3:
                        this.SwitchMainView(this.printerStatusView);
                        return;

                    case Key.F4:
                        base.Topmost = false;
                        base.WindowState = WindowState.Normal;
                        base.WindowStyle = WindowStyle.SingleBorderWindow;
                        return;

                    case Key.F5:
                    case Key.F6:
                    case Key.F7:
                    case Key.F8:
                        return;

                    case Key.F9:
                        this.SwitchMainView(this.testView);
                        return;
                }
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.mainView = new MainView();
            this.taskListView = new TaskListView();
            this.printerStatusView = new PrinterStatusView();
            this.testView = new TestView();
            this.settingsView = new Settings();
            this.SwitchMainView(this.mainView);
        }
    }
}

