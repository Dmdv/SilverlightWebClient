using GalaSoft.MvvmLight;

namespace WebClient.ICS.Client.ViewModel
{
    /// <summary>
    /// Операция.
    /// </summary>
    public class OperationItem : ViewModelBase
    {
        private bool? _isChecked;

        /// <summary>
        /// Имя операции
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Выбрана или нет.
        /// </summary>
        public bool? IsChecked
        {
            get { return _isChecked; }
            set
            {
                if (_isChecked == value) return;

                _isChecked = value;
                RaisePropertyChanged("IsChecked");
            }
        }
    }
}