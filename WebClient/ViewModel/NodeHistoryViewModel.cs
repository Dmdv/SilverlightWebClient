using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using GalaSoft.MvvmLight;
using WebClient.ICS.Client.Commands;
using WebClient.ICS.Client.Contracts;
using WebClient.ICS.Client.Controls;
using WebClient.ICS.Client.ServiceModel;
using WebClient.ICS.Client.ServiceReference;

namespace WebClient.ICS.Client.ViewModel
{
	/// <summary>
	/// This class contains properties that a View can data bind to.
	/// </summary>
	public class NodeHistoryViewModel : ViewModelBase, IModelUpdater
	{
		private const string ErrorMessagePropertyName = "ErrorMessage";

		/// <summary>
		/// Initializes a new instance of the NodeHistoryViewModel class.
		/// </summary>
		/// <param name="element"></param>
		public NodeHistoryViewModel(Element element)
		{
			_element = element;
			RefreshCommand = new DelegateCommand<object>(Refresh, CanRefresh);
			Filter = ViewModelLocator.FilterViewModelStatic;
		}

		public HistoryInfo CurrentInfo
		{
			get { return _currentInfo; }
			set
			{
				_currentInfo = value;

				if (IsActive)
				{
					Service.Proxy.GetHistoryRecords(_currentInfo, OnGetHitoryRecordsCompleted);
				}
			}
		}

		/// <summary>
		/// Сообщение об ошибке.
		/// </summary>
		public string ErrorMessage
		{
			get { return _errorMessageProperty; }

			set
			{
				if (_errorMessageProperty == value)
				{
					return;
				}
				_errorMessageProperty = value;
				RaisePropertyChanged(ErrorMessagePropertyName);
			}
		}

		public bool HasChanges
		{
			get { return false; }
		}

		/// <summary>
		/// HistoryInfo.
		/// </summary>
		public IEnumerable<HistoryInfo> HistoryInfo
		{
			get
			{
				if (IsInDesignMode)
				{
					return ViewModelLocator.HistoryInfoDesignTime;
				}

				Guard.NotNull(Filter);

				_historyInfo = new ObservableCollection<HistoryInfo>();

				var operationFilter = Filter.Operations.Split(';');
				var operations = string.Join(" ", operationFilter).ToLower();

				if (IsActive)
				{
					Service.Proxy.GetElementHistory(_element,
						Filter.Successfull,
						Filter.From.Date,
						Filter.To.Date.AddDays(1),
						Filter.UserFilter,
						operations,
						Filter.Max,
						OnGetHistoryCompleted);
				}

				return _historyInfo;
			}
		}

		public ObservableCollection<HistoryRecord> HistoryRecords
		{
			get { return _historyRecords; }
			set
			{
				_historyRecords = value;
				RaisePropertyChanged("HistoryRecords");
			}
		}

		public bool IsActive { get; set; }

		public DelegateCommand<object> RefreshCommand { get; private set; }

		public void Reset()
		{
			Refresh(this);
		}

		public void SaveChanges(object obj)
		{
		}

		/// <summary>
		/// Unregisters this instance from the Messenger class.
		/// <para>
		/// </para>
		/// </summary>
		public override void Cleanup()
		{
			_filter.FilterChanged -= OnFilterOnFilterChanged;
			base.Cleanup();
		}

		private FilterViewModel Filter
		{
			get { return _filter; }
			set
			{
				Guard.NotNull(value);

				_filter = value;
				_filter.FilterChanged -= OnFilterOnFilterChanged;
				_filter.FilterChanged += OnFilterOnFilterChanged;
			}
		}

		private bool CanRefresh(object arg)
		{
			Guard.NotNull(Filter);

			return Filter.HasChanges;
		}

		private void OnFilterOnFilterChanged(object o, EventArgs args)
		{
			if (!IsActive)
			{
				return;
			}

			if (_element != null)
			{
				Debug.WriteLine(">>> Changed [Filter] узла '{0}'", _element.Name);
			}

			RefreshCommand.RaiseCanExecuteChanged();
		}

		private void OnGetHistoryCompleted(ObservableCollection<HistoryInfo> list, string message)
		{
			Guard.NotNull(Filter);

			if (!string.IsNullOrEmpty(message))
			{
				ErrorMessage = message;
				return;
			}

			foreach (var historyInfo in list)
			{
				_historyInfo.Add(historyInfo);
			}

			Filter.HasChanges = false;
		}

		private void OnGetHitoryRecordsCompleted(ObservableCollection<HistoryRecord> records, string message)
		{
			if (!string.IsNullOrEmpty(message))
			{
				MessageWindow.Show(message, "Failed to get 'History Record'");
				return;
			}

			HistoryRecords = records;
		}

		/// <summary>
		/// Обновление данных.
		/// </summary>
		private void Refresh(object o)
		{
			if (_element != null)
			{
				Debug.WriteLine(">>> Refresh [HISTORY] узла '{0}'", _element.Name);
			}

			RaisePropertyChanged("HistoryInfo");
		}

		private readonly Element _element;
		private HistoryInfo _currentInfo;
		private string _errorMessageProperty = string.Empty;
		private FilterViewModel _filter;
		private ObservableCollection<HistoryInfo> _historyInfo;
		private ObservableCollection<HistoryRecord> _historyRecords;
	}
}