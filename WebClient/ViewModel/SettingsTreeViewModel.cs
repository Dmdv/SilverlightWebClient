using GalaSoft.MvvmLight;
using WebClient.ICS.Client.ServiceReference;

namespace WebClient.ICS.Client.ViewModel
{
    /// <summary>
    /// <para>
    /// ViewModel of tree Root.
    /// </para>
    /// <para>
    /// This class contains properties that a View can data bind to.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// </summary>
    public class SettingsTreeViewModel : ViewModelBase
    {
        /// <summary>
        /// The <see cref="Root" /> property's name.
        /// </summary>
        private const string RootPropertyName = "Root";

        private SettingNodeViewModel _root;

        /// <summary>
        /// Initializes a new instance of the SettingsTreeViewModel class.
        /// </summary>
        public SettingsTreeViewModel()
        {
            // Создание рута.
            // var root = new Node {Parent = null, Id = NodeParams.RootId, 
            // IdSpecified = true, Name = NodeParams.RootName};
            var root = new Node { Parent = null, Id = NodeParams.RootId, Name = NodeParams.RootName };
            Root = new SettingNodeViewModel(root, null);

            // CreateRootChildCommand = new DelegateCommand<string>(CreateRootNode, CanCreateRootNode);
        }

        /// <summary>
        /// Gets the Root property.
        /// Root of the tree.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// This property's value is broadcasted by the Messenger's default instance when it changes.
        /// </summary>
        public SettingNodeViewModel Root
        {
            get { return _root; }
            private set
            {
                if (_root == value)
                {
                    return;
                }

                var oldValue = _root;
                _root = value;

                RaisePropertyChanged(RootPropertyName, oldValue, value, true);
            }
        }

        ///// <summary>
        ///// Команда для создания нового узла.
        ///// </summary>
        // public DelegateCommand<string> CreateRootChildCommand { get; private set; }

        // private static bool CanCreateRootNode(string arg)
        // {
        //    return !string.IsNullOrEmpty(arg) && !string.IsNullOrWhiteSpace(arg);
        // }

        // private void CreateRootNode(string obj)
        // {
        //    var node = new Node
        //                      {
        //                          Name = obj,
        //                          Parent = (Node) Root.Element,
        //                          LastChange = DateTime.Now,
        //                          Description = string.Empty
        //                      };

        // Service.Proxy.CreateElement(null, node, null, OnCreateElementComplete);
        // }

        // private void OnCreateElementComplete(Element element, CommandNodeParameter commandNodeParameter)
        // {
        //    Root.Children.Add(new SettingNodeViewModel(element, Root));
        // }
    }
}