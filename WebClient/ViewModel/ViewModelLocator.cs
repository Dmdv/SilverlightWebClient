using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using WebClient.ICS.Client.Model;
using WebClient.ICS.Client.ServiceReference;
using EditType = WebClient.ICS.Client.Model.EditType;
using RightsAdapter = WebClient.ICS.Client.Model.RightsAdapter;

namespace WebClient.ICS.Client.ViewModel
{
    /// <summary>
    /// Контейнер для ViewModels.
    /// Все свойства "DesignTime" используются для design time, для работы в MS Expression Blend.
    /// </summary>
    public class ViewModelLocator
    {
        // ReSharper disable ClassNeverInstantiated.Global
        // ReSharper disable MemberCanBeMadeStatic.Global

        // Nodes.

        private static SettingsTreeViewModel _settingsViewModel;
        private static NodeEditorViewModel _viewModelNodeEditorDesignTime;
        private static NodePermissionsViewModel _permissions;
        private static UserPermissionViewModel _userPermissionsViewModel;
        private static UserPermissionItemViewModel _userRightsPermissionItem;
        private static NodeHistoryViewModel _nodeHistoryDesignTime;

        // History.

        private static HistoryInfo _historyInfoItem;
        private static HistoryRecord _historyRecordItem;
        private static ObservableCollection<HistoryInfo> _shortChangesInfoDesignTime;
        private static ObservableCollection<HistoryRecord> _fullChangesInfoDesignTime;

        // Filter.

        private static FilterViewModel _filterViewModel;
        private static OperationListViewModel _operationListViewModel;

        // Subscriptions.

        private static SubscriptionItemViewModel _subscriptionItemViewModelDesignTime;
        private static SubscriptionViewModel _subscriptionViewModel;

        // Validators.

        private static readonly IEnumerable<ValidatorViewModel> _contraints = new List<ValidatorViewModel>
                                {
                                    new ValidatorViewModel(new ConstraintInfo {EditType = EditType.Text, Name = "Max"}, null, null),
                                    new ValidatorViewModel(new ConstraintInfo {EditType = EditType.Text, Name = "Min"}, null, null)
                                };
        /// <summary>
        /// SubscriptionDesignTime
        /// </summary>
        public static SubscriptionItemViewModel SubscriptionItemViewModelDesignTime
        {
            get
            {
                if (_subscriptionItemViewModelDesignTime != null)
                {
                    var subscription = new Subscription
                                                  {
                                                      Address = "email1@email.com; email2@email.com",
                                                      Creator = @"KL\Bystrov",
                                                      WatchedUsers = @"KL\Dyachkov; KL\Bystrov",
                                                      Operations = "Create, Remove"
                                                  };

                    _subscriptionItemViewModelDesignTime
                        = new SubscriptionItemViewModel(subscription,
                                                        new ObservableCollection<SubscriptionItemViewModel>());
                }

                return _subscriptionItemViewModelDesignTime;
            }
        }

        /// <summary>
        /// SubscriptionDesignTime.
        /// </summary>
        public static SubscriptionViewModel SubscriptionDesignTime
        {
            get
            {
                var element = new Element {Name = "Test Element"};
                return _subscriptionViewModel ?? (_subscriptionViewModel = new SubscriptionViewModel(element));
            }
        }

        /// <summary>
        /// ViewModel настроек.
        /// </summary>
        public SettingsTreeViewModel SettingsViewModel
        {
            get { return SettingsViewModelStatic; }
        }

        /// <summary>
        /// Список прав с т.зр. пользователей.
        /// </summary>
        public UserPermissionViewModel UserPermissionViewModel
        {
            get { return UserPermissionViewModelStatic; }
        }

        /// <summary>
        /// Статический список прав с т.зр. пользователей.
        /// </summary>
        public static UserPermissionViewModel UserPermissionViewModelStatic
        {
            get
            {
                if (_userPermissionsViewModel == null) CreateUserPermissionListViewModel();
                return _userPermissionsViewModel;
            }
        }

        /// <summary>
        /// Filter for history records.
        /// </summary>
        public FilterViewModel FilterViewModel
        {
            get { return FilterViewModelStatic; }
        }

        /// <summary>
        /// Filter for history records.
        /// </summary>
        public static FilterViewModel FilterViewModelStatic
        {
            get { return _filterViewModel ?? (_filterViewModel = new FilterViewModel(OperationListViewModelStatic)); }
        }

        public static IEnumerable<HistoryRecord> HistoryRecordsDesignTime
        {
            get
            {
                if (_fullChangesInfoDesignTime == null)
                {
                    _fullChangesInfoDesignTime = new ObservableCollection<HistoryRecord>();
                    var record1 = new HistoryRecord
                                      {
                                          Title = "Change 1",
                                          Content = "Content XYXYXYXYXYXYXYXYXYXYXYXYXXYXYXYXYXYXYXYXYXYXYYXXY"
                                      };
                    var record2 = new HistoryRecord
                                      {
                                          Title = "Change 2",
                                          Content = "Content DDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDD"
                                      };

                    _fullChangesInfoDesignTime.Add(record1);
                    _fullChangesInfoDesignTime.Add(record2);
                }

                return _fullChangesInfoDesignTime;
            }
        }

        public static IEnumerable<HistoryInfo> HistoryInfoDesignTime
        {
            get
            {
                if (_shortChangesInfoDesignTime == null)
                {
                    _shortChangesInfoDesignTime = new ObservableCollection<HistoryInfo>();

                    var shortChangesInfoDesignTime = new HistoryInfo
                                                         {
                                                             Time = DateTime.Now,
                                                             Info = "Additional info about changes",
                                                             Operation = Model.Operation.Modify.ToString().ToLower(),
                                                             Path = "Root/Settings",
                                                             User = @"KL\Dyachkov"
                                                         };

                    var shortChangesInfoDesignTime2 = new HistoryInfo
                                                          {
                                                              Time = DateTime.Now,
                                                              Info = "Additional info about changes2",
                                                              Operation = Model.Operation.Modify.ToString().ToLower(),
                                                              Path = "Root/Settings2",
                                                              User = @"KL\Dyachkov"
                                                          };

                    _shortChangesInfoDesignTime.Add(shortChangesInfoDesignTime);
                    _shortChangesInfoDesignTime.Add(shortChangesInfoDesignTime2);
                }

                return _shortChangesInfoDesignTime;
            }
        }

        public static HistoryRecord HistoryRecordItemDesignTime
        {
            get
            {
                return _historyRecordItem ??
                       (_historyRecordItem = new HistoryRecord {Content = "Content", Title = "Title"});
            }
        }

        /// <summary>
        /// HistoryInfoItemDesignTime.
        /// </summary>
        public static HistoryInfo HistoryInfoItemDesignTime
        {
            get
            {
                return _historyInfoItem ?? (_historyInfoItem = new HistoryInfo
                                            {
                                                Time = DateTime.Now,
                                                Info = "Additional info about changes",
                                                Operation = Model.Operation.Modify.ToString().ToLower(),
                                                Path = "Root/Settings",
                                                User = @"KL\Dyachkov"
                                            });
            }
        }

        /// <summary>
        /// Gets the UserPermissionItemViewModel property.
        /// </summary>
        public UserPermissionItemViewModel UserPermissionItemViewModelDesignTime
        {
            get { return UserPermissionItemStatic; }
        }

        /// <summary>
        /// NodeHistoryDesignTimeStatic.
        /// </summary>
        public static NodeHistoryViewModel NodeHistoryDesignTimeStatic
        {
            get { return _nodeHistoryDesignTime ?? (_nodeHistoryDesignTime = new NodeHistoryViewModel(null)); }
        }

        /// <summary>
        /// NodeHistoryDesignTime.
        /// </summary>
        public NodeHistoryViewModel NodeHistoryDesignTime
        {
            get { return NodeHistoryDesignTimeStatic; }
        }

        /// <summary>
        /// Gets the ViewModelPropertyName property.
        /// </summary>
        public NodeEditorViewModel NodeEditorViewModelDesignTime
        {
            get { return NodeEditorViewModelDesignTimeStatic; }
        }

        /// <summary>
        /// Constraint for design time.
        /// Used only to design UI.
        /// </summary>
        public IEnumerable<ValidatorViewModel> ValidatorsDesignTime
        {
            get { return _contraints; }
        }

        public NodePermissionsViewModel NodePermissionsViewModelDesignTime
        {
            get { return NodePermissionsViewModelDesignTimeStatic; }
        }

        public static RightsAdapter RightsAdapterStaticDesignTime
        {
            get
            {
                const Model.Operation Operation = Model.Operation.Read | Model.Operation.ReadWrite | Operation.Enumerate;
                var perm = new Permission {Operations = Operation.Cast<byte>()};
                return new RightsAdapter(perm);
            }
        }

        /// <summary>
        /// Cleans up all the resources.
        /// </summary>
        public static void Cleanup()
        {
            ClearUserRightsViewModel();
            ClearUserPermissionListViewModel();
            ClearViewModelPropertyName();
            ClearSettingsViewModel();
        }

        private static OperationListViewModel OperationListViewModelStatic
        {
            get { return _operationListViewModel ?? (_operationListViewModel = new OperationListViewModel()); }
        }

        /// <summary>
        /// Gets the UserPermissionItemViewModel property.
        /// </summary>
        private static UserPermissionItemViewModel UserPermissionItemStatic
        {
            get
            {
                if (_userRightsPermissionItem == null) CreateUserRightsViewModel();
                return _userRightsPermissionItem;
            }
        }

        /// <summary>
        /// Gets the SettingsViewModel property.
        /// </summary>
        private static SettingsTreeViewModel SettingsViewModelStatic
        {
            get
            {
                if (_settingsViewModel == null) CreateSettingsViewModel();
                return _settingsViewModel;
            }
        }

        /// <summary>
        /// Gets the ViewModelPropertyName property.
        /// </summary>
        private static NodeEditorViewModel NodeEditorViewModelDesignTimeStatic
        {
            get
            {
                if (_viewModelNodeEditorDesignTime == null) CreateViewModelPropertyName();
                return _viewModelNodeEditorDesignTime;
            }
        }

        /// <summary>
        /// Gets the PermissionList at design time.
        /// </summary>
        private static NodePermissionsViewModel NodePermissionsViewModelDesignTimeStatic
        {
            get
            {
                if (_permissions == null) CreatePermissions();
                return _permissions;
            }
        }

        /// <summary>
        /// Provides a deterministic way to create the UserPermissionItemViewModel property.
        /// </summary>
        private static void CreateUserRightsViewModel()
        {
            if (_userRightsPermissionItem == null)
            {
                var user = new User();
                _userRightsPermissionItem = new UserPermissionItemViewModel(user);
            }
        }

        /// <summary>
        /// Provides a deterministic way to delete the UserPermissionItemViewModel property.
        /// </summary>
        private static void ClearUserRightsViewModel()
        {
            if (_userRightsPermissionItem != null) _userRightsPermissionItem.Cleanup();
            _userRightsPermissionItem = null;
        }

        /// <summary>
        /// Provides a deterministic way to create the UserPermissionViewModel property.
        /// </summary>
        private static void CreateUserPermissionListViewModel()
        {
            if (_userPermissionsViewModel == null) _userPermissionsViewModel = new UserPermissionViewModel();
        }

        private static void CreatePermissions()
        {
            var permissions = new ObservableCollection<Permission>();

            var user1 = new User { Name = @"KL\Dyachkov" };
            var user2 = new User { Name = @"KL\Bystrov" };

            var pre1 = new Permission { Operations = Operation.Read.Cast<byte>(), User = user1 };
            var pre2 = new Permission { Operations = Operation.ReadWrite.Cast<byte>(), User = user2 };

            permissions.Add(pre1);
            permissions.Add(pre2);

            _permissions = new NodePermissionsViewModel(null) {Permissions = permissions};
        }

        /// <summary>
        /// Provides a deterministic way to create the ViewModelPropertyName property.
        /// </summary>
        private static void CreateViewModelPropertyName()
        {
        	if (_viewModelNodeEditorDesignTime == null) _viewModelNodeEditorDesignTime = new NodeEditorViewModel(null);
        }

        /// <summary>
        /// Provides a deterministic way to create the SettingsViewModel property.
        /// </summary>
        private static void CreateSettingsViewModel()
        {
            if (_settingsViewModel == null) _settingsViewModel = new SettingsTreeViewModel();
        }

        /// <summary>
        /// Provides a deterministic way to delete the UserPermissionViewModel property.
        /// </summary>
        private static void ClearUserPermissionListViewModel()
        {
            _userPermissionsViewModel.Cleanup();
            _userPermissionsViewModel = null;
        }

        /// <summary>
        /// Provides a deterministic way to delete the SettingsViewModel property.
        /// </summary>
        private static void ClearSettingsViewModel()
        {
            if (_settingsViewModel != null) _settingsViewModel.Cleanup();
            _settingsViewModel = null;
        }

        /// <summary>
        /// Provides a deterministic way to delete the ViewModelPropertyName property.
        /// </summary>
        private static void ClearViewModelPropertyName()
        {
            if (_viewModelNodeEditorDesignTime != null) _viewModelNodeEditorDesignTime.Cleanup();
            _viewModelNodeEditorDesignTime = null;
        }
    }
}