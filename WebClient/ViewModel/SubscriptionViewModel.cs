using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using GalaSoft.MvvmLight;
using WebClient.ICS.Client.Model;
using WebClient.ICS.Client.Commands;
using WebClient.ICS.Client.Controls;
using WebClient.ICS.Client.ServiceModel;
using WebClient.ICS.Client.ServiceReference;

namespace WebClient.ICS.Client.ViewModel
{
    /// <summary>
    /// ViewModel подписок элемента настроек.
    /// </summary>
    public class SubscriptionViewModel : ViewModelBase, IModelUpdater
    {
        private const string ErrorMessagePropertyName = "ErrorMessage";
        private const string SubscriptionsPropertyName = "Subscriptions";

        private readonly object _coreObject;
        private ObservableCollection<SubscriptionItemViewModel> _subscriptions;
        private string _errorMessageProperty = string.Empty;

        // Поля, используемые для создания подписки.

        private string _watchedUser = string.Empty;
        private static readonly OperationListViewModel _operationListViewModel;
        private string _address = string.Empty;

        static SubscriptionViewModel()
        {
            _operationListViewModel = new OperationListViewModel();
        }

        /// <summary>
        /// ViewModel подписок узла.
        /// </summary>
        /// <param name="element"></param>
        public SubscriptionViewModel(Element element)
            : this()
        {
            _coreObject = element;
        }

        public SubscriptionViewModel(User user)
            : this()
        {
            _coreObject = user;
        }

        private SubscriptionViewModel()
        {
            CreateSubscriptionCommand = new DelegateCommand<object>(CreateSubscription, CanCreateSubscription);
            _operationListViewModel.PropertyChanged += OnOperationsPropertyChanged;

            if (!IsInDesignMode) return;
            DesignTimeData();
        }

        public DelegateCommand<object> CreateSubscriptionCommand { get; private set; }

        public IEnumerable<SubscriptionItemViewModel> Subscriptions
        {
            get
            {
                _subscriptions = new ObservableCollection<SubscriptionItemViewModel>();
                if (IsActive) InitialiseSubscriptions();
                return _subscriptions;
            }
        }

        public bool IsActive { private get; set; }

        ////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Используется при создании новой подписки.
        /// </summary>
        public string WatchedUser
        {
            get { return _watchedUser; }
            set
            {
                if (_watchedUser == value) return;
                _watchedUser = value;
                RaisePropertyChanged("WatchedUser");
                CreateSubscriptionCommand.RaiseCanExecuteChanged();
            }
        }

        /// <summary>
        /// Используется при создании новой подписки.
        /// </summary>
        public static OperationListViewModel OperationListViewModel
        {
            get { return _operationListViewModel; }
        }

        /// <summary>
        /// Используется при создании новой подписки.
        /// </summary>
        public string Address
        {
            get { return _address; }
            set
            {
                if (_address == value) return;
                _address = value;
                RaisePropertyChanged("Address");
                CreateSubscriptionCommand.RaiseCanExecuteChanged();
            }
        }

        public Visibility IsEditorVisible
        {
            get { return IsElement ? Visibility.Visible : Visibility.Collapsed; }
        }

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
        /// Cleanup.
        /// </summary>
        public override void Cleanup()
        {
            _operationListViewModel.PropertyChanged -= OnOperationsPropertyChanged;
            base.Cleanup();
        }

        private bool IsElement
        {
            get { return _coreObject is Element; }
        }

        private bool IsUser
        {
            get { return _coreObject is User; }
        }

        private void InitialiseSubscriptions()
        {
            if (IsElement)
            {
                Service.Proxy.GetElementSubscriptions(_coreObject.Cast<Element>(), OnGetSubscriptionsCompleted);
            }
            else if (IsUser)
            {
                Service.Proxy.GetUserSubscriptions(_coreObject.Cast<User>(), OnGetSubscriptionsCompleted);
            }
        }

        private void OnOperationsPropertyChanged(object o, PropertyChangedEventArgs args)
        {
            CreateSubscriptionCommand.RaiseCanExecuteChanged();
        }

        private void OnCreateSubscriptionCompleted(Subscription subscription, string message)
        {
            if (!string.IsNullOrEmpty(message))
            {
                var caption = string.Format("Failed to create node subscriptions for {0}",
                                            _coreObject.Cast<Element>().Name);

                MessageWindow.Show(message, caption);
                return;
            }

            _subscriptions.Add(new SubscriptionItemViewModel(subscription, _subscriptions));
        }

        private void OnGetSubscriptionsCompleted(ObservableCollection<Subscription> list, string message)
        {
            Debug.WriteLine(">> Get [Subscripions] for " + _coreObject);

            if (!string.IsNullOrEmpty(message))
            {
                ErrorMessage = message;
                return;
            }

            foreach (var subscription in list)
            {
                _subscriptions.Add(new SubscriptionItemViewModel(subscription, _subscriptions));
            }
        }

        private bool CanCreateSubscription(object arg)
        {
            return 
                _coreObject.TryCast<Element>() != null &&
                !string.IsNullOrEmpty(OperationListViewModel.SelectedOperations) &&
                !string.IsNullOrEmpty(Address);
        }

        private void CreateSubscription(object obj)
        {
            var subscription = new Subscription
                                   {
                                       Address = Address,
                                       Operations = OperationListViewModel.SelectedOperations.Replace(";", ","),
                                       WatchedUsers = WatchedUser
                                   };

            Debug.WriteLine(">>> Trying to create [Subscription] for element: " + _coreObject.Cast<Element>().Name);

            Service.Proxy.CreateSubscription(_coreObject.Cast<Element>(), subscription, OnCreateSubscriptionCompleted);
        }

        private void DesignTimeData()
        {
            _subscriptions = new ObservableCollection<SubscriptionItemViewModel>();

            var subscription1 = new Subscription
                                    {
                                        Address = "email1@email.com; email2@email.com",
                                        Creator = @"KL\Bystrov",
                                        WatchedUsers = @"KL\Dyachkov; KL\Bystrov",
                                        Operations = "Create, Remove"
                                    };

            var subscription2 = new Subscription
                                    {
                                        Address = "email2@email.com",
                                        Creator = @"KL\Dyachkov",
                                        WatchedUsers = @"KL\Dyachkov",
                                        Operations = "Create, Remove, Update"
                                    };

            _subscriptions.Add(new SubscriptionItemViewModel(subscription1, _subscriptions));
            _subscriptions.Add(new SubscriptionItemViewModel(subscription2, _subscriptions));
        }

        public bool HasChanges
        {
            get { return false; }
        }

        public void SaveChanges(object obj)
        {
        }

        public void Reset()
        {
            RaisePropertyChanged(SubscriptionsPropertyName);
        }
    }
}