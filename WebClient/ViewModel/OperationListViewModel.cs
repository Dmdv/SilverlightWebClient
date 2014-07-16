using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using GalaSoft.MvvmLight;

namespace WebClient.ICS.Client.ViewModel
{
    /// <summary>
    /// Инициализирует контрол выбора операций.
    /// </summary>
    public class OperationListViewModel : ViewModelBase
    {
        private const string Separator = "; ";
        private const string Text = "Any";

        private static readonly string[] _operations = new[]
                                                           {
                                                               "Create", "Rename", "Write", 
                                                               "Update", "Delete", "Grant",
                                                               "Subscribe"
                                                           };

        private string _selectedOperations;
        private readonly OperationItem _anyItem;
        private bool _isUserClick = true;

        /// <summary>
        /// Initializes a new instance of the OperationListViewModel class.
        /// </summary>
        public OperationListViewModel()
        {
            _anyItem = new OperationItem {IsChecked = true, Text = Text};

            var operationItems = _operations
                .Select(x => new OperationItem { IsChecked = true, Text = x })
                .OrderBy(y => y.Text)
                .ToList();

            Operations = new ObservableCollection<OperationItem>(operationItems);
            Operations.Insert(0, _anyItem);

            RefreshSelectedOperations();

            foreach (var operationItem in operationItems)
            {
                operationItem.PropertyChanged += (o, args) => RefreshSelectedOperations();
            }

            _anyItem.PropertyChanged += AnyChanged;
        }

        public OperationItem AnyItem
        {
            get { return _anyItem; }
        }

        public ObservableCollection<OperationItem> Operations { get; set; }

        /// <summary>
        /// Возвращает Any, если выбраны все операции.
        /// </summary>
        public string SelectedOperations
        {
            get
            {
                var anyValue = AnyItem.Text;
                return _selectedOperations.Contains(anyValue) ? anyValue : _selectedOperations;
            }

            set
            {
                _selectedOperations = value;
                RaisePropertyChanged("SelectedOperations");
            }
        }

        /// <summary>
        /// Отметить только операции, указанные в списке, разделенные разделителем.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="delimiter"></param>
        public void Select(string input, char[] delimiter)
        {
            if (string.IsNullOrEmpty(input)) return;
            var operations = input.ToLower().Split(delimiter).Select(y => y.Trim()).ToArray();
            var operationItems = Operations.Where(x => operations.Contains(x.Text.ToLower())).ToList();
            if (operationItems.Count == 0) return;
            AnyItem.IsChecked = false;

            foreach (var operationItem in operationItems)
            {
                operationItem.IsChecked = true;
            }
        }

        private void AnyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (!_isUserClick) return;

            if (_anyItem.IsChecked.HasValue && _anyItem.IsChecked.Value)
            {
                foreach (var operationItem in Operations)
                {
                    operationItem.IsChecked = true;
                }
            }

            if (_anyItem.IsChecked.HasValue && !_anyItem.IsChecked.Value)
            {
                foreach (var operationItem in Operations)
                {
                    operationItem.IsChecked = false;
                }
            }
        }

        private void RefreshSelectedOperations()
        {
            if (!_isUserClick) return;

            _isUserClick = false;

            var operationItems = new []{_anyItem};

            if (Operations.Except(operationItems).All(z => z.IsChecked.HasValue && z.IsChecked.Value))
            {
                _anyItem.IsChecked = true;
            }
            else if (Operations.Except(operationItems).Any(z => z.IsChecked.HasValue && !z.IsChecked.Value))
            {
                _anyItem.IsChecked = false;
            }

            _isUserClick = true;

            SelectedOperations = string.Join(Separator, Operations
                                                       .Where(x => x.IsChecked.HasValue && x.IsChecked.Value)
                                                       .Select(y => y.Text));
        }
    }
}