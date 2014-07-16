using System;
using System.Globalization;
using System.Threading;
using System.Windows;
using GalaSoft.MvvmLight.Threading;
using WebClient.ICS.Client.ServiceModel;
using WebClient.ICS.Client.ViewModel;
using WebClient.ICS.Client.Views;

namespace WebClient.ICS.Client
{
    /// <summary>
    /// Класс приложения.
    /// </summary>
    public partial class App
    {
        private const string ServiceUrl = "ServiceUrl";

        // ReSharper disable UnusedMember.Global
        // ReSharper restore UnusedMember.Global

        public App()
        {
            Thread.CurrentThread.CurrentUICulture = (CultureInfo) CultureInfo.InvariantCulture.Clone();
            Thread.CurrentThread.CurrentCulture = (CultureInfo) CultureInfo.InvariantCulture.Clone();

            var shortDatePattern = new CultureInfo("ru-RU").DateTimeFormat.ShortDatePattern;

            Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern = shortDatePattern;
            Thread.CurrentThread.CurrentUICulture.DateTimeFormat.ShortDatePattern = shortDatePattern;

            Startup += ApplicationStartup;
            Exit += ApplicationExit;
            UnhandledException += ApplicationUnhandledException;

            InitializeComponent();
        }

        private void ApplicationStartup(object sender, StartupEventArgs e)
        {
            if (!e.InitParams.ContainsKey(ServiceUrl))
            {
                MessageBox.Show(@"For connection a ServiceUlr is reqiured in 'initParams' section of start page:"
                                + @"<param name='initParams' value='ServiceUrl=http://...'/>",
                                "Service Initialization error", MessageBoxButton.OK);

                //Service.Configure();
            }
            else
            {
                Service.Configure(e.InitParams[ServiceUrl]);
            }

            RootVisual = new Shell();
            DispatcherHelper.Initialize();
        }

        private static void ApplicationExit(object sender, EventArgs e)
        {
            ViewModelLocator.Cleanup();
            Service.Close();
        }

        private static void ApplicationUnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            // If the app is running outside of the debugger then report the exception using
            // the browser's exception mechanism. On IE this will display it a yellow alert 
            // icon in the status bar and Firefox will display a script error.
            if (!System.Diagnostics.Debugger.IsAttached)
            {

                // NOTE: This will allow the application to continue running after an exception has been thrown
                // but not handled. 
                // For production applications this error handling should be replaced with something that will 
                // report the error to the website and stop the application.
                e.Handled = true;
                Deployment.Current.Dispatcher.BeginInvoke(() => ReportErrorToDom(e));
            }
        }

        private static void ReportErrorToDom(ApplicationUnhandledExceptionEventArgs e)
        {
            try
            {
                var errorMsg = e.ExceptionObject.Message + e.ExceptionObject.StackTrace;
                errorMsg = errorMsg.Replace('"', '\'').Replace("\r\n", @"\n");

                System.Windows.Browser.HtmlPage.Window.Eval("throw new Error(\"Unhandled Error in Silverlight 2 Application " + errorMsg + "\");");
            }
            catch (Exception)
            {
            }
        }
    }
}
