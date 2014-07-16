using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;
using WebClient.ICS.Client.Model;
using WebClient.ICS.Client.Commands;
using WebClient.ICS.Client.Controls;
using WebClient.ICS.Client.Converters;
using WebClient.ICS.Client.ServiceModel;
using WebClient.ICS.Client.ServiceReference;

namespace WebClient.ICS.Client.ViewModel
{
    /// <summary>
    /// Элемент дерева настроек.
    /// </summary>
    public class SettingNodeViewModel : ViewModelBase
    {
        private const string HeaderInfoPropertyName = "HeaderInfo";
        private const string LastChangePropertyName = "LastChange";

        /// <summary>
        /// Children elements.
        /// </summary>
        private readonly ObservableCollection<SettingNodeViewModel> _children =
            new ObservableCollection<SettingNodeViewModel>();

        private bool _isDisabled;

        /// <summary>
        /// Initializes a new instance of the SettingNode class.
        /// </summary>
        public SettingNodeViewModel(Element setting, SettingNodeViewModel parent)
        {
            PropertyChanged += (o, args) => Debug.WriteLine("SettingNode property changed: " + args.PropertyName);

            Element = setting;
            Parent = parent;

            InitializeCommands();
            InitializeViewModels();
        }

        /// <summary>
        /// Дочерние узлы.
        /// </summary>
        public IEnumerable<SettingNodeViewModel> Children
        {
            get
            {
                PopulateChildren();
                return _children;
            }
        }

        public bool HasChildren
        {
            get
            {
                var node = Element.TryCast<Node>();
                return node == null ? false : node.HasChildren;
            }
        }

        /// <summary>
        /// Наличие изменений.
        /// </summary>
        public IEnumerable<IModelUpdater> HasChanges
        {
            get
            {
                var list = new List<IModelUpdater>();
                if (NodeEditor.HasChanges) list.Add(NodeEditor);
                if (PermissionsEditor.HasChanges) list.Add(PermissionsEditor);
                return list;
            }
        }

        public bool IsNode
        {
            get { return Element is Node; }
        }

        public bool IsSetting
        {
            get { return Element is Setting; }
        }

        /// <summary>
        /// Имя настройки или узла.
        /// </summary>
        public string HeaderInfo
        {
            get { return NodeEditor.HeaderInfo; }
            private set { NodeEditor.HeaderInfo = value; }
        }

        /// <summary>
        /// Gets the LastChange property.
        /// </summary>
        public DateTime LastChange
        {
            get { return Element.LastChange; }
            set
            {
                var oldValue = Element.LastChange;
                Element.LastChange = value;

                RaisePropertyChanged(LastChangePropertyName, oldValue, value, true);

                NodeEditor.LastChange = value;
            }
        }

        /// <summary>
        /// Permissions list.
        /// </summary>
        public NodePermissionsViewModel PermissionsEditor { get; private set; }

        /// <summary>
        /// Node editor.
        /// </summary>
        public NodeEditorViewModel NodeEditor { get; private set; }

        /// <summary>
        /// Node subscription editor.
        /// </summary>
        public SubscriptionViewModel Subscriptions { get; private set; }

        /// <summary>
        /// Node history viewer.
        /// </summary>
        public NodeHistoryViewModel NodeHistory { get; private set; }

        /// <summary>
        /// Команда для создания нового узла.
        /// </summary>
        public DelegateCommand<CommandNodeParameter> CreateSettingCommand { get; private set; }

        /// <summary>
        /// Команда для создания нового узла.
        /// </summary>
        public DelegateCommand<CommandNodeParameter> CreateNodeCommand { get; private set; }

        /// <summary>
        /// Команда для переименования узла.
        /// </summary>
        public DelegateCommand<CommandNodeParameter> RenameCommand { get; private set; }

        /// <summary>
        /// Команда для удаления узла.
        /// </summary>
        public DelegateCommand<SettingNodeViewModel> DeleteNodeCommand { get; private set; }

        /// <summary>
        /// Команда для раскрытия узла.
        /// </summary>
        public DelegateCommand<SettingNodeViewModel> NodeExpandedCommand { get; private set; }

        /// <summary>
        /// Команда для раскрытия узла.
        /// </summary>
        public DelegateCommand<SettingNodeViewModel> NodeClickeCommand { get; private set; }

        /// <summary>
        /// Команда при смене таба.
        /// </summary>
        public DelegateCommand<ViewModelBase> TabChangedCommand { get; private set; }

        /// <summary>
        /// Cleanup.
        /// </summary>
        public override void Cleanup()
        {
            NodeEditor.PropertyChanged -= NodeEditorPropertyChanged;
            NodeEditor.ForceChildrenUpdate -= ResetChildren;
            NodeEditor.DataError -= OnEditorDataError;
            PermissionsEditor.PermissionNeedUpdate -= PermissionsEditorChanged;

            NodeHistory.Cleanup();
            NodeEditor.Cleanup();
            PermissionsEditor.Cleanup();
            Subscriptions.Cleanup();

            base.Cleanup();
        }

        private string HeaderInfoPrivate
        {
            set
            {
                if (Element.Name == value) return;
                var oldValue = Element.Name;
                Element.Name = value;

                RaisePropertyChanged(HeaderInfoPropertyName, oldValue, value, true);
            }
        }

        internal Element Element { get; private set; }

        private bool IsDisabled
        {
            get { return _isDisabled; }
            set
            {
                _isDisabled = value;
                NodeClickeCommand.RaiseCanExecuteChanged();
            }
        }

        private SettingNodeViewModel Parent { get; set; }

        private void InitializeCommands()
        {
            CreateNodeCommand = new DelegateCommand<CommandNodeParameter>(CreateNodeElement, CanCreateNodeElement);
            CreateSettingCommand = new DelegateCommand<CommandNodeParameter>(CreateSettingElement,
                                                                             CanCreateSettingElement);
            DeleteNodeCommand = new DelegateCommand<SettingNodeViewModel>(DeleteNode, CanDeleteNode);
            RenameCommand = new DelegateCommand<CommandNodeParameter>(RenameNode, CanRenameNode);
            NodeExpandedCommand = new DelegateCommand<SettingNodeViewModel>(WhenExpandedNode, CanExpandNode);
            NodeClickeCommand = new DelegateCommand<SettingNodeViewModel>(WhenClickedNode, CanNodeBeClicked);
            TabChangedCommand = new DelegateCommand<ViewModelBase>(WhenTabChanged, CanTabBeChanged);
        }

        private void InitializeViewModels()
        {
            NodeEditor = new NodeEditorViewModel(Element);
            PermissionsEditor = new NodePermissionsViewModel(Element);
            NodeHistory = new NodeHistoryViewModel(Element);
            Subscriptions = new SubscriptionViewModel(Element);

            NodeEditor.PropertyChanged += NodeEditorPropertyChanged;
            NodeEditor.ForceChildrenUpdate += ResetChildren;
            NodeEditor.DataError += OnEditorDataError;
            PermissionsEditor.PermissionNeedUpdate += PermissionsEditorChanged;
        }

        /// <summary>
        /// Вызывается при изменении разрешений.
        /// Происходит попытка перезагрузить узел с настройками.
        /// </summary>
        private void PermissionsEditorChanged(object sender, EventArgs eventArgs)
        {
            NodeEditor.Reset();
            NodeHistory.Reset();
            Subscriptions.Reset();
        }

        private void NodeEditorPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            // Вызывается при изменении имени
            if (e.PropertyName == NodeEditorViewModel.HeaderInfoPropertyName)
            {
                HeaderInfoPrivate = NodeEditor.HeaderInfo;
            }
        }

        private void PopulateChildren()
        {
            if (Service.Proxy == null) return;
            if (!(Element is Node)) return;
            var node = Element.Cast<Node>();

            Service.Proxy.GetChildElements(node.IsRoot() ? null : node, OnGetChildrenCompleted);
        }

        private void ResetChildren()
        {
            foreach (var settingNodeViewModel in _children)
            {
                settingNodeViewModel.Cleanup();
            }

            _children.Clear();
            PopulateChildren();
        }

        private void ResetChildren(object sender, EventArgs e)
        {
            ResetChildren();
        }

        private void OnEditorDataError(object sender, EditorInvalidOperationArgs e)
        {
            if (Parent == null) return;
            MessageWindow.Show(e.Message, "Error");
            Parent.ResetChildren();
        }

        private void RenameNode(CommandNodeParameter obj)
        {
            HeaderInfo = obj.NodeName;
        }

        private void OnGetChildrenCompleted(IEnumerable<Element> elements, string message)
        {
            if (!string.IsNullOrEmpty(message))
            {
                MessageWindow.Show(message, "Failed to get children");
                IsDisabled = true;
                return;
            }

            Debug.WriteLine(string.Format(">> PopulateChildren at {0}", HeaderInfo));

            foreach (var element in elements)
            {
                _children.Add(new SettingNodeViewModel(element, this));
            }
        }

        private static void WhenExpandedNode(SettingNodeViewModel settingNode)
        {
            settingNode.ResetChildren();
        }

        private static void WhenClickedNode(SettingNodeViewModel settingNode)
        {
            Debug.WriteLine(string.Format(">> Clicked node at {0}", settingNode.HeaderInfo));
            settingNode.NodeEditor.Reset();
        }

        private static void WhenTabChanged(ViewModelBase viewModel)
        {
            if (viewModel == null) return;
            
            if (viewModel.TryCast<NodePermissionsViewModel>() != null)
            {
                viewModel.Cast<NodePermissionsViewModel>().Reset();
            }

            if (viewModel.TryCast<NodeHistoryViewModel>() != null)
            {
                viewModel.Cast<NodeHistoryViewModel>().Reset();
            }
        }

        private static void CreateSettingElement(CommandNodeParameter arg)
        {
            Node parent = null;
            var parentViewModel = arg.SettingNode;

            if (arg.SettingNode.IsNode)
            {
                parent = arg.SettingNode.Element.Cast<Node>();
                if (parent.IsRoot())
                {
                    parent = null;
                }
            }
            else if (arg.SettingNode.IsSetting)
            {
                parent = arg.SettingNode.Parent.Element.Cast<Node>();
                parentViewModel = arg.SettingNode.Parent;
            }

            var setting = new Setting
                              {
                                  Name = arg.NodeName,
                                  Parent = parent,
                                  Data = false.ToBytes(),
                                  Description = string.Empty
                              };

            Service.Proxy.CreateElement(setting.Parent, setting, parentViewModel, OnCreateElementComplete);
        }

        private static void CreateNodeElement(CommandNodeParameter arg)
        {
            var node = new Node
                              {
                                  Name = arg.NodeName,
                                  Parent = arg.SettingNode.Element.Cast<Node>(),
                                  LastChange = DateTime.Now,
                                  Description = string.Empty
                              };

            if (node.Parent.IsRoot())
            {
                node.Parent = null;
            }

            Service.Proxy.CreateElement(node.Parent, node, arg.SettingNode, OnCreateElementComplete);
        }

        private static void OnCreateElementComplete(Element newElement, string message, SettingNodeViewModel parentViewModel)
        {
            if (!string.IsNullOrEmpty(message))
            {
                MessageWindow.Show(message, "Failed to create new element");
                return;
            }

            parentViewModel._children.Add(new SettingNodeViewModel(newElement, parentViewModel));
        }

        private static void DeleteNode(SettingNodeViewModel arg)
        {
            Service.Proxy.DeleteElement(arg.Element, arg, OnDeleteCompleted);
        }

        private static void OnDeleteCompleted(SettingNodeViewModel arg, string message)
        {
            if (!string.IsNullOrEmpty(message))
            {
                MessageWindow.Show(message, "Failed to delete the element");
                return;
            }

            arg.Cleanup();
            arg.Parent._children.Remove(arg);
        }

        private static bool CanRenameNode(CommandNodeParameter arg)
        {
            return
                arg.SettingNode != null
                && arg.NodeName != string.Empty
                && !arg.SettingNode.Parent.IsRoot() // Все элементы, у которых родитель есть Root - не переименовывать.
                && arg.NodeName != arg.SettingNode.HeaderInfo;
        }

        private static bool CanCreateSettingElement(CommandNodeParameter arg)
        {
            // Клик на узле или на настройке.
            if (arg.SettingNode == null) return false;
            if (arg.SettingNode.IsNode) return true;
            return arg.SettingNode.Parent != null && arg.SettingNode.Parent.IsNode;
        }

        private static bool CanCreateNodeElement(CommandNodeParameter arg)
        {
            return arg.SettingNode == null || !(arg.SettingNode.IsSetting);
        }

        private static bool CanDeleteNode(SettingNodeViewModel arg)
        {
            return
                arg != null
                && arg.Parent != null
                && !arg.Parent.IsRoot(); // Все элементы, у которых родитель есть Root - не удалять.
        }

        private static bool CanExpandNode(SettingNodeViewModel arg)
        {
            return true;
        }

        private static bool CanNodeBeClicked(SettingNodeViewModel settingNode)
        {
            return settingNode == null || !settingNode.IsDisabled;
        }

        private static bool CanTabBeChanged(ViewModelBase arg)
        {
            return true;
        }
    }
}