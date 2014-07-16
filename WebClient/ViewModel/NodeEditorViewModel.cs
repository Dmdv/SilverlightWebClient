using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Controls;
using GalaSoft.MvvmLight;
using WebClient.ICS.Client.Converters;
using WebClient.ICS.Client.Commands;
using WebClient.ICS.Client.Controls;
using WebClient.ICS.Client.Model;
using WebClient.ICS.Client.ServiceModel;
using WebClient.ICS.Client.ServiceReference;

namespace WebClient.ICS.Client.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// </summary>
    public class NodeEditorViewModel : ViewModelBase, IModelUpdater
    {
        // Обновляемые свойства.

        public const string HeaderInfoPropertyName = "HeaderInfo";
        private const string DataTypePropertyName = "DataType";
        private const string DataPropertyName = "Data";
        private const string DescriptionPropertyName = "Description";
        private const string LastChangePropertyName = "LastChange";
        private const string LastAccessPropertyName = "LastAccess";
        private const string ValidatorsPropertyName = "Validators";
        private const string ErrorMessagePropertyName = "ErrorMessage";
        private const string IsEnabledPropertyName = "IsEnabled";
        private const string EditorNotationPropertyName = "EditorNotation";

        // Игнорируемые свойства при обновлении узла.

        private readonly string[] _ignoreChanges = new[]
                                                       {
                                                           HeaderInfoPropertyName,
                                                           ErrorMessagePropertyName,
                                                           IsEnabledPropertyName
                                                       };

        // Персистентные поля. Остальные создаются во время выполнения.

        private Element _element;
        private bool _hasChanges;
        private bool _isEnabled = true;
        private static Stream _scriptStream;
        private string _errorMessageProperty = string.Empty;
        private IList<ValidatorViewModel> _validators;

        /// <summary>
        /// Вызывается, когда необходимо обновить детей текущего узла в случае накатывания скрипта.
        /// </summary>
        public event EventHandler ForceChildrenUpdate;

        /// <summary>
        /// Вызывается при возникновении ошибок редактора. Приводит к обновлению всех детей на данном уровне.
        /// </summary>
        public event EventHandler<EditorInvalidOperationArgs> DataError;

        /// <summary>
        /// Initializes a new instance of the NodeEditorViewModel class.
        /// </summary>
        /// <param name="element"></param>
        public NodeEditorViewModel(Element element)
        {
            if (IsInDesignMode)
            {
                DesignTimeData();
            }
            else
            {
                _element = element;
            }

            InitializeValidators();

            GenerateScriptCommand = new DelegateCommand<NodeEditorViewModel>(GenerateScript, CanGenerareScript);
            ApplyScriptCommand = new DelegateCommand<NodeEditorViewModel>(ApplyScript, CanLoadScript);
            SaveChangesCommand = new DelegateCommand<object>(SaveChanges, CanSaveChanges);
            GenerateScriptCommand.RaiseCanExecuteChanged();
            ApplyScriptCommand.RaiseCanExecuteChanged();

            PropertyChanged += OnNodePropertyChanged;
        }

        /// <summary>
        /// GenerateScript.
        /// </summary>
        public DelegateCommand<NodeEditorViewModel> GenerateScriptCommand { get; private set; }

        /// <summary>
        /// LoadScript.
        /// </summary>
        public DelegateCommand<NodeEditorViewModel> ApplyScriptCommand { get; private set; }

        /// <summary>
        /// Команда для обновления изменений узла.
        /// </summary>
        public DelegateCommand<object> SaveChangesCommand { get; private set; }

        /// <summary>
        /// IsActive.
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// True if Element is Node.
        /// </summary>
        public bool IsNode
        {
            get { return Element is Node; }
        }

        /// <summary>
        /// Используется для деактивации контрола, например,  в случае ошибки.
        /// </summary>
        public bool IsEnabled
        {
            get { return _isEnabled; }
            set
            {
                if (_isEnabled == value) return;
                _isEnabled = value;
                RaisePropertyChanged(IsEnabledPropertyName);
            }
        }

        /// <summary>
        /// HeaderInfo.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// This property's value is broadcasted by the Messenger's default instance when it changes.
        /// </summary>
        public string HeaderInfo
        {
            get { return Element.Name; }
            set
            {
                if (value == Element.Name) return;
                Element.Name = value;
                SaveChanges(null);
            }
        }

        /// <summary>
        /// Gets the Description property.
        /// </summary>
        public string Description
        {
            get { return Element.Description; }

            set
            {
                if (Element.Description == value)
                {
                    return;
                }

                var oldValue = Element.Description;
                Element.Description = value;

                RaisePropertyChanged(DescriptionPropertyName, oldValue, value, true);
            }
        }

        public string EditorNotation
        {
            get
            {
                return DataType == DataTypeEnum.DateTime
                           ? string.Format("Setting value: ({0})",
                                           Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern)
                           : "Setting value:";
            }
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
            }
        }

        public DateTime? LastAccess
        {
            get
            {
                var setting = Element.TryCast<Setting>();
                return setting == null ? null : setting.LastAccess;
            }
        }

        /// <summary>
        /// Gets the DataType property.
        /// </summary>
        public DataTypeEnum? DataType
        {
            get { return Element is Node ? null : (DataTypeEnum?) ((Setting) Element).DataType; }

            set
            {
                // Проверки.

                if (!value.HasValue) throw new NullReferenceException("DataType");
                if (Element is Node) return;
                var setting = Element.Cast<Setting>();
                if (setting.DataType == value.Value.Cast<byte>()) return;

                // Обновление DataType.

                setting.DataType = value.Value.Cast<byte>();

                // Обновление Data.

                setting.Data = DataType.Default().ToBytes();

                // Обновление Constraints.

                ResetConstraint(setting);

                RaisePropertyChanged(DataTypePropertyName);
                RaisePropertyChanged(ValidatorsPropertyName);
                RaisePropertyChanged(EditorNotationPropertyName);
            }
        }

        /// <summary>
        /// Gets the Data property.
        /// </summary>
        public object Data
        {
            get
            {
                if (!IsActive) return null;
                var o = Element is Node ? null : Element.Cast<Setting>().Data.ToObject(DataType);
                // Debug.WriteLine(">>> get_[Data]: '{0}'", o);
                return o;
            }

            set
            {
                // Set используется для установки свойства в binded контроле.
                // Оптимизация:
                // Для установки в коде необходимо установить это свойство и вызвать 
                // RaisePropertyChanged(DataPropertyName), чтобы изменения отразилсь в интерфейсе.

                if (Element is Node || !IsActive) return;

                if (value is ArgumentException)
                {
                    throw value.Cast<ArgumentException>();
                }

                var setting = Element.Cast<Setting>();

                // Если присваеваемое значение равно Data - возврат.
                if (value != null && value.ToBytes().SequenceEqual(setting.Data)) return;

                var bytes = value == null ? DataType.Default().ToBytes() : value.ToBytes();

                if (!Check.Less500TBytes(DataType, bytes))
                {
                    MessageWindow.Show("File length exceeded the maximum allowed size of 500.000", "Failed to set value");
                    Reset();
                    return;
                }

                setting.Data = bytes;

                // Debug.WriteLine(">>> set_[Data]: '{0}'", Element.Cast<Setting>().Data.ToObject(DataType));

                HasChanges = true;

                // Валидация.
                ClientValidator.ForType(DataType).Validate(Constraint1, Constraint2, value);
            }
        }

        /// <summary>
        /// Сообщение об ошибке. При присваивании значения вызывает блокирующее окно.
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
        /// Validators.
        /// </summary>
        public IEnumerable<ValidatorViewModel> Validators
        {
            get
            {
                InitializeValidators();
                return _validators;
            }
        }

        public bool HasChanges
        {
            get { return _hasChanges; }
            private set
            {
                _hasChanges = value;
                SaveChangesCommand.RaiseCanExecuteChanged();
            }
        }

        /// <summary>
        /// SaveChanges and resets HasChanges flag.
        /// </summary>
        /// <param name="obj">Paraneter is used from inside a ICommand, should be null otherwise.</param>
        public void SaveChanges(object obj)
        {
            Service.Proxy.UpdateElement(Element, OnUpdateComplete);
        }

        /// <summary>
        /// Вызывается при клике на узле, при смене таба.
        /// </summary>
        public void Reset()
        {
            Service.Proxy.GetElement(Element, OnResetComplete);
        }

        /// <summary>
        /// Свойство устанавливается при перезагрузке узла или при сохранении узла, \
        /// которое приводит к перезагрузке узла.
        /// </summary>
        private Element Element
        {
            get { return _element; }
            set
            {
                _element = value;
                RaisePropertyChanged(DataPropertyName);
                RaisePropertyChanged(DataTypePropertyName);
                RaisePropertyChanged(ValidatorsPropertyName);
                RaisePropertyChanged(HeaderInfoPropertyName);
                RaisePropertyChanged(DescriptionPropertyName);
                RaisePropertyChanged(LastChangePropertyName);
                RaisePropertyChanged(LastAccessPropertyName);
            }
        }

        /// <summary>
        /// Minimum constraint.
        /// </summary>
        private string Constraint1
        {
            get { return GetConstraint1(); }
            set { SetConstraint1(value); }
        }

        /// <summary>
        /// Maximum constraint.
        /// </summary>
        private string Constraint2
        {
            get { return GetConstraint2(); }
            set { SetConstraint2(value); }
        }

        private ConstraintInfo Constraint1Info
        {
            get { return ConstraintInfoCreator.FirstConstraint(DataType); }
        }

        private ConstraintInfo Constraint2Info
        {
            get { return ConstraintInfoCreator.SecondConstraint(DataType); }
        }

        private string GetConstraint1()
        {
            var setting = Element as Setting;
            return setting == null ? null : setting.Constraint1;
        }

        private string GetConstraint2()
        {
            var setting = Element as Setting;
            return setting == null ? null : setting.Constraint2;
        }

        private void SetConstraint1(string value)
        {
            var setting = Element as Setting;
            if (setting == null || setting.Constraint1 == value) return;
            setting.Constraint1 = value;
            HasChanges = true;
            ClientValidator.ForType(DataType).Validate(value, Constraint2, Data);
        }

        private void SetConstraint2(string value)
        {
            var setting = Element as Setting;
            if (setting == null || setting.Constraint2 == value) return;
            setting.Constraint2 = value;
            HasChanges = true;
            ClientValidator.ForType(DataType).Validate(Constraint1, value, Data);
        }

        private void ApplyScript(NodeEditorViewModel nodeEditor)
        {
            var openFileDialog = new OpenFileDialog
                                     {
                                         Filter = "XML Files (.xml)|*.xml|All Files (*.*)|*.*",
                                         FilterIndex = 1,
                                         Multiselect = false
                                     };

            try
            {
                var userClickedOk = openFileDialog.ShowDialog();
                if (userClickedOk != true) return;

                using (var streamReader = openFileDialog.File.OpenText())
                {
                    var text = streamReader.ReadToEnd();
                    Service.Proxy.ApplyScript(nodeEditor.Element.Cast<Node>(), text, OnApplyScriptComplete);
                }
            }
            catch (Exception exception)
            {
                MessageWindow.Show(exception.Message, "Failed to apply script");
            }
        }

        private static void GenerateScript(NodeEditorViewModel nodeEditor)
        {
            var saveFileDialog = new SaveFileDialog
                                     {
                                         Filter = "XML Files (.xml)|*.xml|All Files (*.*)|*.*",
                                         FilterIndex = 1,
                                     };
            try
            {
                var userClickedOk = saveFileDialog.ShowDialog();
                if (userClickedOk != true) return;

                _scriptStream = saveFileDialog.OpenFile();
                Service.Proxy.GenerateScript(nodeEditor.Element.Cast<Node>(), new StreamWriter(_scriptStream),
                                             OnGenerateScriptComplete);
            }
            catch (Exception exception)
            {
                MessageWindow.Show(exception.Message, "Failed to generate script");
            }
        }

        private void OnApplyScriptComplete(string message)
        {
            if (!string.IsNullOrEmpty(message))
            {
                MessageWindow.Show(message, "Exception on update element");
            }

            InvokeForceChildrenUpdate(new EventArgs());
        }

        private static void OnGenerateScriptComplete(string script, TextWriter stream, string message)
        {
            using (_scriptStream)
            {
                using (stream)
                {
                    if (!string.IsNullOrEmpty(message))
                    {
                        MessageWindow.Show(message, "Exception on update element");
                        return;
                    }

                    stream.Write(script);
                }
            }
        }

        private void OnResetComplete(Element element, string message)
        {
            if (!string.IsNullOrEmpty(message))
            {
                IsEnabled = false;
                InvokeDataError(new EditorInvalidOperationArgs(message));
                return;
            }

            Element = element;
            IsEnabled = true;
            HasChanges = false;
        }

        private void InvokeForceChildrenUpdate(EventArgs e)
        {
            var handler = ForceChildrenUpdate;
            if (handler != null) handler(this, e);
        }

        private void InvokeDataError(EditorInvalidOperationArgs e)
        {
            var handler = DataError;
            if (handler != null) handler(this, e);
        }

        private void OnUpdateComplete(Element element, string message)
        {
            if (!string.IsNullOrEmpty(message))
            {
                MessageWindow.Show(message, "Failed to update an element");
                return;
            }

            Element = element;
            HasChanges = false;
        }

        private void OnNodePropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            // Debug.WriteLine(">>> SettingNodeEditor >>  [PropertyChanged] = " + e.PropertyName);
            if (_ignoreChanges.Contains(e.PropertyName)) return;
            HasChanges = true;
        }

        private static bool CanLoadScript(NodeEditorViewModel arg)
        {
            return arg != null && arg.Element is Node;
        }

        private static bool CanGenerareScript(NodeEditorViewModel arg)
        {
            return arg != null && arg.Element is Node;
        }

        private bool CanSaveChanges(object arg)
        {
            return HasChanges;
        }

        /// <summary>
        /// Только выполняет обертку. Не инициализит валидаторы настроек.
        /// </summary>
        private void InitializeValidators()
        {
            if (_validators != null) _validators.Clear();

            if (Element is Node || DataType == DataTypeEnum.Bool || DataType == DataTypeEnum.DateTime)
            {
                _validators = null;
                return;
            }

            _validators = DataType == DataTypeEnum.Bin
                              ? new List<ValidatorViewModel>
                                    {
                                        new ValidatorViewModel(Constraint1Info, SetConstraint1, GetConstraint1)
                                    }
                              : new List<ValidatorViewModel>
                                    {
                                        new ValidatorViewModel(Constraint1Info, SetConstraint1, GetConstraint1),
                                        new ValidatorViewModel(Constraint2Info, SetConstraint2, GetConstraint2)
                                    };
        }

        private void ResetConstraint(Setting setting)
        {
            if (setting.DataType == DataTypeEnum.Bool.Cast<byte>() ||
                setting.DataType == DataTypeEnum.DateTime.Cast<byte>())
            {
                Constraint1 = null;
                Constraint2 = null;
            }
            else if (setting.DataType == DataTypeEnum.Bin.Cast<byte>())
            {
                Constraint1 = string.Empty;
                Constraint2 = null;
            }
            else
            {
                Constraint1 = string.Empty;
                Constraint2 = string.Empty;
            }
        }

        private void DesignTimeData()
        {
            _element = new Setting
                           {
                               Id = 11,
                               LastChange = DateTime.Now,
                               Name = "Sample node name",
                               DataType = (byte) DataTypeEnum.String,
                               Data = "Sample data".ToBytes(),
                               Description = "Sample description"
                           };
        }
    }
}