using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using GalaSoft.MvvmLight;
using WebClient.ICS.Client.Commands;
using WebClient.ICS.Client.Controls;
using WebClient.ICS.Client.Model;
using WebClient.ICS.Client.ServiceModel;
using WebClient.ICS.Client.ServiceReference;

namespace WebClient.ICS.Client.ViewModel
{
    /// <summary>
    /// Редактирование списка прав на узел.
    /// </summary>
    public class NodePermissionsViewModel : ViewModelBase, IModelUpdater
    {
        private const string PermissionsPropertyName = "Permissions";
        private const string HeaderInfoPropertyName = "HeaderInfo";
        private const string LastChangePropertyName = "LastChange";
        private const string ErrorMessagePropertyName = "ErrorMessage";

        private string _errorMessageProperty = string.Empty;

        /// <summary>
        /// Список разрешений.
        /// </summary>
        private ObservableCollection<Permission> _permissionList;

        /// <summary>
        /// Список изменений.
        /// </summary>
        private readonly List<Permission> _changes = new List<Permission>();

        /// <summary>
        /// Вызывается, когда изменились разрешения и необходимо обновить историю, редактор и просмотр для
        /// элемента, если разрешения изменились с понижением.
        /// </summary>
        public event EventHandler PermissionNeedUpdate;

        /// <summary>
        /// Initializes a new instance of the NodePermissionsViewModel class.
        /// </summary>
        public NodePermissionsViewModel(Element element)
        {
            Element = element;
            SaveChangesCommand = new DelegateCommand<object>(SaveChanges, CanSaveChanges);
            AddPermissionCommand = new DelegateCommand<string>(AddPermission, CanAddPermission);
            RemovePermissionCommand = new DelegateCommand<Permission>(RemovePermission, CanRemovePermission);

            if (!IsInDesignMode) return;
            CreateDesignTimeData();
        }

        /// <summary>
        /// Save changes command.
        /// </summary>
        public DelegateCommand<object> SaveChangesCommand { get; private set; }

        /// <summary>
        /// Adds permission
        /// </summary>
        public DelegateCommand<string> AddPermissionCommand { get; private set; }

        /// <summary>
        /// Removes permission.
        /// </summary>
        public DelegateCommand<Permission> RemovePermissionCommand { get; private set; }

        /// <summary>
        /// Gets the Permissions property.
        /// </summary>
        public ObservableCollection<Permission> Permissions
        {
            get
            {
                ClearPermissions();

                _permissionList = new ObservableCollection<Permission>();

                if (IsActive)
                {
                    Service.Proxy.GetElementPermissions(Element, OnGetPermissionsComplete);
                }
                return _permissionList;
            }

            set
            {
                if (_permissionList == value) return;
                var oldValue = _permissionList;
                _permissionList = value;

                RaisePropertyChanged(PermissionsPropertyName, oldValue, value, true);
            }
        }

        /// <summary>
        /// HeaderInfo.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// This property's value is broadcasted by the Messenger's default instance when it changes.
        /// </summary>
        public IEnumerable<char> HeaderInfo
        {
            get { return Element.Name; }
        }

        /// <summary>
        /// Gets the LastChange property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// This property's value is broadcasted by the Messenger's default instance when it changes.
        /// </summary>
        public DateTime LastChange
        {
            get { return Element.LastChange; }
        }

        /// <summary>
        /// Has changes.
        /// </summary>
        public bool HasChanges
        {
            get { return CanSaveChanges(null); }
        }

        public bool IsActive { get; set; }

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
        /// Сохранить изменения.
        /// </summary>
        /// <param name="obj"></param>
        public void SaveChanges(object obj)
        {
            foreach (var permission in _changes)
            {
                Service.Proxy.UpdatePermission(permission, OnUpdatePermissionComplete);
            }
            
            _changes.Clear();
            SaveChangesCommand.RaiseCanExecuteChanged();
        }

        /// <summary>
        /// Сброс всех изменений.
        /// </summary>
        public void Reset()
        {
            Debug.WriteLine(string.Format(">>> Refresh [NodePermissions] узла '{0}'", Element.Name));

            RaisePropertyChanged(HeaderInfoPropertyName);
            RaisePropertyChanged(LastChangePropertyName);
            RaisePropertyChanged(PermissionsPropertyName);

            _changes.Clear();
            SaveChangesCommand.RaiseCanExecuteChanged();
        }

        /// <summary>
        /// Cleanup.
        /// </summary>
        public override void Cleanup()
        {
            ClearPermissions();
            base.Cleanup();
        }

        private Element Element { get; set; }

        private static bool CanRemovePermission(Permission arg)
        {
            return true;
        }

        private void RemovePermission(Permission permission)
        {
            permission.PropertyChanged -= OnPermissionPropertyChanged;
            Service.Proxy.DeletePermission(permission, OnDeletePermissionComplete);
        }

        private void OnDeletePermissionComplete(Permission permission, string message)
        {
            if (!string.IsNullOrEmpty(message))
            {
                MessageWindow.Show(message, "Failed to delete permission");
                return;
            }

            _permissionList.Remove(permission);
        }

        private static bool CanAddPermission(string parameter)
        {
            return !string.IsNullOrEmpty(parameter);
        }

        private void AddPermission(string parameter)
        {
            const Operation Op = Operation.Enumerate | Operation.Grant | Operation.Modify |
                                 Operation.Read | Operation.ReadWrite | Operation.Report | Operation.Subscribe;

            var permission = new Permission
                                 {
                                     User = new User {Name = parameter},
                                     Operations = Op.Cast<byte>()
                                 };

            Service.Proxy.AddPermission(Element, permission, OnAddPermissionComplete);
        }

        private void OnAddPermissionComplete(Permission permission, string message)
        {
            if (!string.IsNullOrEmpty(message))
            {
                MessageWindow.Show(message, "Failed to add permission");
                return;
            }

            RaisePropertyChanged(PermissionsPropertyName);
        }

        private bool CanSaveChanges(object arg)
        {
            return _changes.Count != 0;
        }

        /// <summary>
        /// Вызывается при изменении разрешений.
        /// </summary>
        private void OnUpdatePermissionComplete(string userName, Permission newPermission, string message)
        {
            if (!string.IsNullOrEmpty(message))
            {
                var msg = string.Format("Failed to save permission for User: '{0}'\r\nMessage:{1}", userName, message);
                ErrorMessage = msg;
            }

            ErrorMessage = string.Empty;
            InvokePermissionNeedUpdate(null);
        }

        private void ClearPermissions()
        {
            if (_permissionList != null)
            {
                foreach (var permission in _permissionList)
                {
                    permission.PropertyChanged -= OnPermissionPropertyChanged;
                }

                _permissionList.Clear();
            }
        }

        private void OnGetPermissionsComplete(IEnumerable<Permission> permissions, string message)
        {
            if (!string.IsNullOrEmpty(message))
            {
                ErrorMessage = "Failed to initialize permissions: \r\n\r\n" + message;
                return;
            }

            ErrorMessage = string.Empty;

            foreach (var permission in permissions)
            {
                permission.PropertyChanged -= OnPermissionPropertyChanged;
                permission.PropertyChanged += OnPermissionPropertyChanged;
                _permissionList.Add(permission);
            }
        }

        private void OnPermissionPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            var permission = sender.Cast<Permission>();
            if (!_changes.Contains(permission)) _changes.Add(permission);
            SaveChangesCommand.RaiseCanExecuteChanged();
        }

        private void CreateDesignTimeData()
        {
            _permissionList = new ObservableCollection<Permission>();

            var user1 = new User { Name = @"KL\Dyachkov" };
            var user2 = new User { Name = @"KL\Bystrov" };

            var pre1 = new Permission { Operations = Operation.Read.Cast<byte>(), User = user1 };
            var pre2 = new Permission { Operations = Operation.ReadWrite.Cast<byte>(), User = user2 };

            _permissionList.Add(pre1);
            _permissionList.Add(pre2);
        }

        private void InvokePermissionNeedUpdate(EventArgs e)
        {
            var handler = PermissionNeedUpdate;
            if (handler != null) handler(this, e);
        }
    }
}