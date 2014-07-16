using System.Collections.Generic;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;
using WebClient.ICS.Client.Commands;
using WebClient.ICS.Client.Contracts;
using WebClient.ICS.Client.Controls;
using WebClient.ICS.Client.ServiceModel;
using WebClient.ICS.Client.ServiceReference;

namespace WebClient.ICS.Client.ViewModel
{
	/// <summary>
	/// ViewModel элемента подписки, отображаемого в списке подписок.
	/// </summary>
	public class SubscriptionItemViewModel : ViewModelBase
	{
		private static readonly char[] _delimiter =
		{
			';',
			','
		};

		/// <summary>
		/// Initializes a new instance of the SubscriptionItemViewModel class.
		/// </summary>
		public SubscriptionItemViewModel(
			Subscription subscription,
			ObservableCollection<SubscriptionItemViewModel> subscriptions)
		{
			Guard.NotNull(subscription);
			Guard.NotNull(subscriptions);

			UpdateSubscriptionCommand = new DelegateCommand<object>(UpdateSubscription, CanUpdateSubscription);
			DeleteSubscriptionCommand = new DelegateCommand<object>(DeleteSubscription, CanDeleteSubscription);

			_subscription = subscription;
			_subscriptions = subscriptions;

			_operationsList = new OperationListViewModel();
			_operationsList.Select(_subscription.Operations, _delimiter);
			_operationsList.PropertyChanged += (o, args) =>
			{
				HasChanges = true;
				RaisePropertyChanged("SelectedOperations");
			};
		}

		public string Address
		{
			get { return _subscription.Address; }
			set
			{
				if (_subscription.Address == value)
				{
					return;
				}
				_subscription.Address = value;
				RaisePropertyChanged("Address");
				HasChanges = true;
			}
		}

		public IEnumerable<char> Creator
		{
			get { return _subscription.Creator; }
		}

		public DelegateCommand<object> DeleteSubscriptionCommand { get; private set; }

		public IEnumerable<char> ElementPath
		{
			get { return _subscription.ElementPath; }
		}

		public IEnumerable<OperationItem> Operations
		{
			get { return _operationsList.Operations; }
		}

		/// <summary>
		/// Список выбранных операций.
		/// </summary>
		public string SelectedOperations
		{
			get { return _operationsList.SelectedOperations; }
		}

		public DelegateCommand<object> UpdateSubscriptionCommand { get; private set; }

		public string WatchedUsers
		{
			get { return _subscription.WatchedUsers; }
			set
			{
				if (_subscription.WatchedUsers == value)
				{
					return;
				}
				_subscription.WatchedUsers = value;
				RaisePropertyChanged("WatchedUsers");
				HasChanges = true;
			}
		}

		private bool HasChanges
		{
			get { return _hasChanges; }
			set
			{
				if (_hasChanges == value)
				{
					return;
				}
				_hasChanges = value;
				UpdateSubscriptionCommand.RaiseCanExecuteChanged();
			}
		}

		private Subscription Subscription
		{
			set
			{
				Guard.NotNull(value);

				_subscription = value;
				HasChanges = false;
				_operationsList.Select(_subscription.Operations, _delimiter);

				RaisePropertyChanged("WatchedUsers");
				RaisePropertyChanged("Address");
				RaisePropertyChanged("SelectedOperations");
				RaisePropertyChanged("Creator");
				RaisePropertyChanged("ElementPath");
			}
		}

		private static bool CanDeleteSubscription(object arg)
		{
			return true;
		}

		private bool CanUpdateSubscription(object arg)
		{
			return HasChanges;
		}

		private void DeleteSubscription(object obj)
		{
			Service.Proxy.DeleteSubscription(_subscription, OnDeleteSubscriptionComplete);
		}

		private void OnDeleteSubscriptionComplete(string message)
		{
			if (!string.IsNullOrEmpty(message))
			{
				MessageWindow.Show(message, "Failed to delete subscription");
				return;
			}

			_subscriptions.Remove(this);
		}

		private void OnGetSubscriptionCompleted(Subscription subscription, string message)
		{
			if (!string.IsNullOrEmpty(message))
			{
				MessageWindow.Show(message, "Failed to get 'Subscription'");
				return;
			}

			Subscription = subscription;
		}

		private void OnUpdateSubscriptionComplete(Subscription subscription, string message)
		{
			if (!string.IsNullOrEmpty(message))
			{
				MessageWindow.Show(message, "Failed to update subscription");
				Service.Proxy.GetSubscription(_subscription, OnGetSubscriptionCompleted);
				return;
			}

			Subscription = subscription;
		}

		private void UpdateSubscription(object obj)
		{
			_subscription.Operations = _operationsList.SelectedOperations;
			Service.Proxy.UpdateSubscription(_subscription, OnUpdateSubscriptionComplete);
		}

		private readonly OperationListViewModel _operationsList;
		private readonly ObservableCollection<SubscriptionItemViewModel> _subscriptions;
		private bool _hasChanges;
		private Subscription _subscription;
	}
}