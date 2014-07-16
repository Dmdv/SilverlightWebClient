using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using WebClient.ICS.Client.Model;
using WebClient.ICS.Client.Controls;
using WebClient.ICS.Client.ViewModel;

namespace WebClient.ICS.Client.Views
{
    /// <summary>
    /// Description for SettingsTreeView.
    /// </summary>
    public partial class SettingsTreeView
    {
        private const string TabItemsPropertyName = "TabItems";
        private const string SelectedNodePropertyName = "SelectedNode";

        /// <summary>
        /// Identifies the <see cref="TabItems" /> dependency property.
        /// </summary>
        public static readonly DependencyProperty TabItemsProperty = DependencyProperty.Register(
            TabItemsPropertyName,
            typeof (ObservableCollection<TabItem>),
            typeof (SettingsTreeView),
            null);

        /// <summary>
        /// Identifies the <see cref="SelectedNode" /> dependency property.
        /// </summary>
        public static readonly DependencyProperty SelectedNodeProperty = DependencyProperty.Register(
            SelectedNodePropertyName,
            typeof (SettingNodeViewModel),
            typeof (SettingsTreeView),
            new PropertyMetadata(SelectedNodePropertyChanged));

        /// <summary>
        /// Initializes a new instance of the SettingsTreeView class.
        /// </summary>
        public SettingsTreeView()
        {
            InitializeComponent();
            TreeSettings.SelectedItemChanged +=
                (o, args) => SelectedNode = (SettingNodeViewModel) TreeSettings.SelectedItem;

            Loaded += (o, args) => SelectedNode = null;
        }

        /// <summary>
        /// Gets or sets the value of the <see cref="TabItems" />
        /// property. This is a dependency property.
        /// </summary>
        public ObservableCollection<TabItem> TabItems
        {
            get { return (ObservableCollection<TabItem>) GetValue(TabItemsProperty); }
            set { SetValue(TabItemsProperty, value); }
        }

        /// <summary>
        /// Gets or sets the value of the <see cref="SelectedNode" />
        /// property. This is a dependency property.
        /// </summary>
        public SettingNodeViewModel SelectedNode
        {
            get { return (SettingNodeViewModel) GetValue(SelectedNodeProperty); }
            set { SetValue(SelectedNodeProperty, value); }
        }

        public static TabInfo EditorTab
        {
            get { return new TabInfo {HeaderInfo = "Setting editor", Visibility = Visibility.Visible}; }
        }

        public static TabInfo RightsTab
        {
            get { return new TabInfo {HeaderInfo = "Settings rights", Visibility = Visibility.Visible}; }
        }

        public static TabInfo SubscriptionTab
        {
            get { return new TabInfo {HeaderInfo = "Subscriptions", Visibility = Visibility.Visible}; }
        }

        public static TabInfo ScriptsTab
        {
            get { return new TabInfo {HeaderInfo = "Scripts", Visibility = Visibility.Visible}; }
        }

        public static TabInfo HistoryTab
        {
            get { return new TabInfo {HeaderInfo = "History", Visibility = Visibility.Visible}; }
        }

        private static void SelectedNodePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            const string Msg = "Element has changed. \r\nWould you like to save changes?";

            // Предыдущий узел.

            if (e.OldValue != null)
            {
                var oldNodeViewModel = e.OldValue.Cast<SettingNodeViewModel>();
                UpdateNode(oldNodeViewModel, false);

                var modelUpdaters = oldNodeViewModel.HasChanges.ToList();

                if (modelUpdaters.Count != 0)
                {
                    var win = new MessageWindow 
                    { 
                        Message = Msg, Title = "Save changes confirmation", AcceptText = "Yes", CancelText = "No" 
                    };

                    win.Closed += (o, args) =>
                    {
                        if (args.Result.HasValue && args.Result.Value)
                        {
                            modelUpdaters.ForEach(x => x.SaveChanges(null));
                        }
                        else
                        {
                            modelUpdaters.ForEach(x => x.Reset());
                        }
                    };

                    win.Show();
                }
            }

            // Новый текущий узел.
            var activeNodeViewModel = e.NewValue.Cast<SettingNodeViewModel>();
            UpdateNode(activeNodeViewModel, true);
        }

        private static void UpdateNode(SettingNodeViewModel settingNodeViewModel, bool value)
        {
            settingNodeViewModel.NodeEditor.IsActive = value;
            settingNodeViewModel.NodeHistory.IsActive = value;
            settingNodeViewModel.PermissionsEditor.IsActive = value;
            settingNodeViewModel.Subscriptions.IsActive = value;
        }
    }
}