using System;
using System.Reflection;
using System.Windows;
using GalaSoft.MvvmLight.Messaging;
using WebClient.ICS.Client.Messages;
using WebClient.ICS.Client.ServiceModel;
using WebClient.ICS.Client.ViewModel;

namespace WebClient.ICS.Client.Views
{
    /// <summary>
    /// Shell.
    /// </summary>
    public partial class Shell
    {
        public const string TreeViewStateName = "TreeViewState";
        public const string UserPanelStateName = "UserPanelState";

        public Shell()
        {
            InitializeComponent();
            CurrentState = TreeViewStateName;
            Messenger.Default.Register<ChangeSidePanelMessage>(this, OnMessageReceived);
        }

        /// <summary>
        /// Текущая боковая панель приложения.
        /// </summary>
        public string CurrentState { get; private set; }

        /// <summary>
        /// Переключает на первую боковую панель.
        /// </summary>
        public void FirstPanel()
        {
            CurrentState = TreeViewStateName;
            VisualStateManager.GoToState(this, TreeViewStateName, true);
        }

        /// <summary>
        /// Переключает на последнюю боковую панель.
        /// </summary>
        public void LastPanel()
        {
            CurrentState = UserPanelStateName;
            VisualStateManager.GoToState(this, UserPanelStateName, true);
            ViewModelLocator.UserPermissionViewModelStatic.Update();
        }

        /// <summary>
        /// Сменить панели.
        /// </summary>
        public void SwapPanels()
        {
            if (CurrentState == TreeViewStateName)
            {
                LastPanel();
            }
            else
            {
                FirstPanel();
            }
        }

        private void OnMessageReceived(ChangeSidePanelMessage message)
        {
            SwapPanels();
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (Service.Proxy == null) return;

            var versionNumber = ParseVersionNumber(Assembly.GetExecutingAssembly());
            VersionBox.Text = "Version: " + versionNumber;
            Service.Proxy.GetVersion(UpdateVersion);
            VisualStateManager.GoToState(this, TreeViewStateName, true);
        }

        private void UpdateVersion(string version)
        {
            VersionBox.Text += string.Format(" (Server: {0})", version);
        }

        private static Version ParseVersionNumber(Assembly assembly)
        {
            var assemblyName = new AssemblyName(assembly.FullName);
            return assemblyName.Version;
        } 

        private void VersionboxMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Clipboard.SetText(VersionBox.Text);
        }

        private void OnShowSettingsClick(object sender, RoutedEventArgs e)
        {
            FirstPanel();
        }

        private void OnShowUsersClick(object sender, RoutedEventArgs e)
        {
            LastPanel();
        }

        private void ReconnectServer(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(ServiceAddress.Text)) return;
            Service.Reconnect(ServiceAddress.Text);

            var versionNumber = ParseVersionNumber(Assembly.GetExecutingAssembly());
            VersionBox.Text = "Version: " + versionNumber;
            Service.Proxy.GetVersion(UpdateVersion);
        }
    }
}