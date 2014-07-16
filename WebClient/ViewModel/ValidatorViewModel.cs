using System;
using GalaSoft.MvvmLight;
using WebClient.ICS.Client.Contracts;
using WebClient.ICS.Client.Model;

namespace WebClient.ICS.Client.ViewModel
{
	/// <summary>
	/// Common ValidatorViewModel.
	/// </summary>
	public class ValidatorViewModel : ViewModelBase
	{
		/// <summary>
		/// ValidatorViewModel.
		/// </summary>
		public ValidatorViewModel(ConstraintInfo constraintInfo, Action<string> setter, Func<string> getter)
		{
			_info = constraintInfo;
			_setter = setter;
			_getter = getter;
		}

		/// <summary>
		/// Тип данных валидатора.
		/// </summary>
		public EditType EditType
		{
			get { return _info.EditType; }
		}

		/// <summary>
		/// Name.
		/// </summary>
		public string Name
		{
			get { return _info.Name; }
		}

		/// <summary>
		/// Value.
		/// </summary>
		public string Value
		{
			get
			{
				Guard.NotNull(_getter);
				return _getter();
			}
			set
			{
				Guard.NotNull(_setter);
				if (value == Value)
				{
					return;
				}
				_setter(value);
			}
		}

		private readonly Func<string> _getter;
		private readonly ConstraintInfo _info;
		private readonly Action<string> _setter;
	}
}