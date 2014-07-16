using System.Collections.Generic;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;
using WebClient.ICS.Client.Commands;
using WebClient.ICS.Client.Controls;
using WebClient.ICS.Client.Model;
using WebClient.ICS.Client.ServiceModel;
using WebClient.ICS.Client.ServiceReference;

namespace WebClient.ICS.Client.ViewModel
{
    // TODO: SubscriptionViewModel вызывать только по запросу!

    /// <summary>
    /// Элемент списка пользователей.
    /// </summary>
    public class UserPermissionItemViewModel : ViewModelBase
    {
        private const string PermissionsPropertyName = "Permissions";
        private const string SubscriptionsPropertyName = "SubscriptionViewModel";
        private const string ErrorMessagePropertyName = "ErrorMessage";
        private string _errorMessageProperty = string.Empty;

        private ObservableCollection<Permission> _permissions;

        public UserPermissionItemViewModel(User user)
        {
            User = user;
            SubscriptionViewModel = new SubscriptionViewModel(user);

            RemovePermissionCommand = new DelegateCommand<Permission>(RemovePermission, CanRemovePermission);

            if (IsInDesignMode)
            {
                CreateDesignTimeData();
            }
        }

        public DelegateCommand<Permission> RemovePermissionCommand { get; private set; }

        /// <summary>
        /// User
        /// </summary>
        public User User { get; private set; }

        /// <summary>
        /// Permissions
        /// </summary>
        public IEnumerable<Permission> Permissions
        {
            get
            {
                _permissions = new ObservableCollection<Permission>();
                Service.Proxy.GetUserPermissions(User, OnGetUserpermissionsCompleted);
                return _permissions;
            }
        }

        /// <summary>
        /// User subscriptions.
        /// </summary>
        public SubscriptionViewModel SubscriptionViewModel { get; private set; }

        /// <summary>
        /// Сообщение об ошибке.
        /// </summary>
        public string ErrorMessage
        {
            get { return _errorMessageProperty; }
            set
            {
                if (_errorMessageProperty == value) return;
                _errorMessageProperty = value;
                RaisePropertyChanged(ErrorMessagePropertyName);
            }
        }

        /// <summary>
        /// Update.
        /// </summary>
        public void Update()
        {
            RaisePropertyChanged(PermissionsPropertyName);
            RaisePropertyChanged(SubscriptionsPropertyName);
        }

        private void RemovePermission(Permission permission)
        {
            Service.Proxy.DeletePermission(permission, OnDeletePermissionComplete);
        }

        private void OnDeletePermissionComplete(Permission permission, string message)
        {
            if (!string.IsNullOrEmpty(message))
            {
                MessageWindow.Show(message, string.Format("Failed to delete '{0}'", permission.ElementPath));
            }

            // Полное обновление списка прав.
            _permissions.Clear();
        }

        private static bool CanRemovePermission(Permission permission)
        {
            return permission != null;
        }

        private void OnGetUserpermissionsCompleted(IEnumerable<Permission> permissions, string message)
        {
            if (!string.IsNullOrEmpty(message))
            {
                ErrorMessage = message;
                //MessageWindow.Show(message, "Failed to populate user permissions");
                return;
            }

            foreach (var permission in permissions)
            {
                permission.ElementPath += string.Format("\t\t\t[{0}]",
                                                        permission.Operations.Cast<Operation>());
                _permissions.Add(permission);
            }
        }

        private void CreateDesignTimeData()
        {
            _permissions = new ObservableCollection<Permission>
                               {
                                   new Permission {ElementPath = "Permission 1"},
                                   new Permission {ElementPath = "Permission 2"}
                               };
        }
    }
}