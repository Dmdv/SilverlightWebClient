using System.Windows;
using System.Windows.Controls;
using WebClient.ICS.Client.Model;
using WebClient.ICS.Client.ViewModel;

namespace WebClient.ICS.Client.Views
{
    /// <summary>
    /// Description for UserPermissionsView.
    /// </summary>
    public partial class UserPermissionsView
    {
        // ReSharper disable MemberCanBePrivate.Global

        /// <summary>
        /// The <see cref="SelectedItem" /> dependency property's name.
        /// </summary>
        private const string SelectedItemPropertyName = "SelectedItem";

        /// <summary>
        /// Identifies the <see cref="SelectedItem" /> dependency property.
        /// </summary>
        public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register(
            SelectedItemPropertyName,
            typeof(UserPermissionItemViewModel),
            typeof (UserPermissionsView),
            null);

        /// <summary>
        /// Initializes a new instance of the UserPermissionsView class.
        /// </summary>
        public UserPermissionsView()
        {
            InitializeComponent();
            UserListBox.SelectionChanged += OnSelectionChanged;
        }

        /// <summary>
        /// Gets or sets the value of the <see cref="SelectedItem" />
        /// property. This is a dependency property.
        /// </summary>
        public UserPermissionItemViewModel SelectedItem
        {
            get { return GetValue(SelectedItemProperty).Cast<UserPermissionItemViewModel>(); }
            set { SetValue(SelectedItemProperty, value); }
        }

        public static TabInfo HeaderInfo1
        {
            get { return new TabInfo {HeaderInfo = "Rights", Visibility = Visibility.Visible}; }
        }

        public static TabInfo HeaderInfo2
        {
            get { return new TabInfo {HeaderInfo = "Subscriptions", Visibility = Visibility.Visible}; }
        }

        private void OnSelectionChanged(object o, SelectionChangedEventArgs args)
        {
            if (DataContext == null) return;

            var userPermissionViewModel = DataContext.Cast<UserPermissionViewModel>();
            if (userPermissionViewModel.CurrentUser != null)
            {
                userPermissionViewModel.CurrentUser.SubscriptionViewModel.IsActive = false;
            }

            if (args.AddedItems.Count == 0)
            {
                SelectedItem = null;
                userPermissionViewModel.CurrentUser = null;
                return;
            }

            var userViewModel = args.AddedItems[0].Cast<UserPermissionItemViewModel>();

            userPermissionViewModel.CurrentUser = userViewModel;
            userPermissionViewModel.CurrentUser.SubscriptionViewModel.IsActive = true;
            SelectedItem = userViewModel;
        }
    }
}