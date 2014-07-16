using System;
using System.Collections.Generic;
using System.Linq;
using GalaSoft.MvvmLight;
using WebClient.ICS.Client.Contracts;

namespace WebClient.ICS.Client.ViewModel
{
	/// <summary>
	/// Filter.
	/// </summary>
	public class FilterViewModel : ViewModelBase
	{
		private static bool _hasChanges;

		private static readonly IEnumerable<string> _successFilter = new[]
		{
			"Any",
			"Success",
			"Fail"
		};

		/// <summary>
		/// Fires when filter changes.
		/// </summary>
		public event EventHandler FilterChanged;

		public FilterViewModel(OperationListViewModel operationListViewModel)
		{
			Guard.NotNull(operationListViewModel);

			Initialize();

			OperationListViewModel = operationListViewModel;
			PropertyChanged += (o, args) => HasChanges = true;
			OperationListViewModel.PropertyChanged += (sender, args) =>
			{
				RaisePropertyChanged("Operations");
				HasChanges = true;
			};
		}

		public static IEnumerable<string> SuccessFilter
		{
			get { return _successFilter; }
		}

		public DateTime From
		{
			get { return _from; }
			set
			{
				if (_from == value)
				{
					return;
				}
				_from = value;
				RaisePropertyChanged("From");
			}
		}

		public bool HasChanges
		{
			get { return _hasChanges; }
			set
			{
				if (_hasChanges == value)
				{
					return;
				}
				_hasChanges = value;
				if (FilterChanged != null)
				{
					FilterChanged(null, null);
				}
			}
		}

		public int Max
		{
			get { return _max; }
			set
			{
				if (_max == value)
				{
					return;
				}
				_max = value;
				RaisePropertyChanged("Max");
			}
		}

		public OperationListViewModel OperationListViewModel { get; private set; }

		public string Operations
		{
			get { return OperationListViewModel.SelectedOperations; }
		}

		public string Successfull
		{
			get { return _successfull; }
			set
			{
				if (value == null || _successfull == value)
				{
					return;
				}

				_successfull = value;
				RaisePropertyChanged("Successfull");
			}
		}

		public DateTime To
		{
			get { return _to; }
			set
			{
				if (_to == value)
				{
					return;
				}
				_to = value;
				RaisePropertyChanged("To");
			}
		}

		public string UserFilter
		{
			get { return _userFilter; }
			set
			{
				if (_userFilter == value)
				{
					return;
				}
				_userFilter = value;
				RaisePropertyChanged("UserFilter");
			}
		}

		private void Initialize()
		{
			_successfull = _successFilter.ElementAt(0);
			_userFilter = string.Empty;
			_max = 100;
			_from = DateTime.Now.AddMonths(-1);
			_to = DateTime.Now;
		}

		private DateTime _from;
		private int _max;
		private string _successfull;
		private DateTime _to;
		private string _userFilter;
	}
}