namespace MicroPrinter.App
{
    using System;
    using System.CodeDom.Compiler;
    using System.Diagnostics;
    using System.Windows;

    public class App : Application
    {
        [DebuggerNonUserCode, GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent()
        {
            base.StartupUri = new Uri("MainWindow.xaml", UriKind.Relative);
        }

        [DebuggerNonUserCode, GeneratedCode("PresentationBuildTasks", "4.0.0.0"), STAThread]
        public static void Main()
        {
            MicroPrinter.App.App app1 = new MicroPrinter.App.App();
            app1.InitializeComponent();
            app1.Run();
        }
    }
}

