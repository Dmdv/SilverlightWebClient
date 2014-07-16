using System.Collections.Generic;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using WebClient.ICS.Client.Commands;
using WebClient.ICS.Client.Controls;
using WebClient.ICS.Client.Messages;
using WebClient.ICS.Client.ServiceModel;
using WebClient.ICS.Client.ServiceReference;

namespace WebClient.ICS.Client.ViewModel
{
    /// <summary>
    /// Список пользователей.
    /// </summary>
    public class UserPermissionViewModel : ViewModelBase
    {
        private const string UsersPropertyName = "Users";

        private ObservableCollection<UserPermissionItemViewModel> _userList;

        /// <summary>
        /// Initializes a new instance of the UserPermissionViewModel class.
        /// </summary>
        public UserPermissionViewModel()
        {
            RemoveUserCommand = new DelegateCommand<UserPermissionItemViewModel>(OnRemoveUser, CanRemoveUser);

            if (IsInDesignMode)
            {
                CreateDesignTimeData();
            }
        }

        /// <summary>
        /// CurrentUser.
        /// </summary>
        public UserPermissionItemViewModel CurrentUser { get; set; }

        /// <summary>
        /// RemoveUserCommand.
        /// </summary>
        public DelegateCommand<UserPermissionItemViewModel> RemoveUserCommand { get; private set; }

        /// <summary>
        /// Gets the Users property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// This property's value is broadcasted by the Messenger's default instance when it changes.
        /// </summary>
        public IEnumerable<UserPermissionItemViewModel> Users
        {
            get
            {
                if (Service.Proxy == null) return null;
                _userList = new ObservableCollection<UserPermissionItemViewModel>();
                Service.Proxy.GetUsers(OnGetUsersComplete);
                return _userList;
            }
        }

        /// <summary>
        /// Обновление модели.
        /// </summary>
        public void Update()
        {
            if (CurrentUser != null)
            {
                CurrentUser.Update();
            }

            RaisePropertyChanged(UsersPropertyName);
        }

        private void OnRemoveUser(UserPermissionItemViewModel userPermissionItem)
        {
            Service.Proxy.DeleteUser(userPermissionItem.User, OnDeleteUserCompleted);
        }

        private void OnDeleteUserCompleted(string message)
        {
            if (!string.IsNullOrEmpty(message))
            {
                MessageWindow.Show(message, "Failed to delete user");
                return;
            }

            RaisePropertyChanged(UsersPropertyName);
        }

        private static bool CanRemoveUser(UserPermissionItemViewModel arg)
        {
            return arg != null;
        }

        private void OnGetUsersComplete(IEnumerable<User> users, string message)
        {
            if (!string.IsNullOrEmpty(message))
            {
                MessageWindow.Show(message, "Failed to populate list of users");
                Messenger.Default.Send(new ChangeSidePanelMessage());
                return;
            }

            foreach (var user in users)
            {
                _userList.Add(new UserPermissionItemViewModel(user));
            }
        }

        private void CreateDesignTimeData()
        {
            _userList = new ObservableCollection<UserPermissionItemViewModel>();

            var user1 = new User { Name = @"KL\Dyachkov" };
            var user2 = new User { Name = @"KL\Bystrov" };

            _userList.Add(new UserPermissionItemViewModel(user1));
            _userList.Add(new UserPermissionItemViewModel(user2));
        }
    }
}