using System;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Windows;
using WebClient.ICS.Client.Commands;
using WebClient.ICS.Client.Editors;
using WebClient.ICS.Client.Model;

namespace WebClient.ICS.Client.Views
{
    /// <summary>
    /// Кортрол отображает контент в зависимости от типа данных.
    /// </summary>
    public sealed partial class DataDependentControlContainer
    {
        // ReSharper disable MemberCanBePrivate.Global
        // ReSharper disable UnusedMember.Global

        /// <summary>
        /// The <see cref="DataType" /> dependency property's name.
        /// </summary>
        private const string VisibleDataTypePropertyName = "VisibleDataType";

        /// <summary>
        /// Identifies the <see cref="DataType" /> dependency property.
        /// </summary>
        public static readonly DependencyProperty VisibleDataTypeProperty = DependencyProperty.Register(
            VisibleDataTypePropertyName,
            typeof (DataTypeEnum),
            typeof (DataDependentControlContainer),
            new PropertyMetadata(VisibleDataTypePropertyChangedCallback));

        public DataDependentControlContainer()
        {
            InitializeComponent();
        }

        /// <summary>
        /// На основе этого свойства устанавливается видимость внутренних контролов.
        /// </summary>
        public DataTypeEnum VisibleDataType
        {
            get { return (DataTypeEnum) GetValue(VisibleDataTypeProperty); }
            set { SetValue(VisibleDataTypeProperty, value); }
        }

        private static void VisibleDataTypePropertyChangedCallback(DependencyObject d,
                                                                   DependencyPropertyChangedEventArgs e)
        {
            var thisObject = d.Cast<DataDependentControlContainer>();
            var datetype = e.NewValue.Cast<DataTypeEnum>();

            var uiElement = thisObject.EditorsContainer.Children.FirstOrDefault();
            if (uiElement != null)
            {
                thisObject.EditorsContainer.Children.Remove(uiElement);
                uiElement.Cast<IActiveAware>().IsActive = false;

                Debug.WriteLine(string.Format(">>> Удален контрол типа {0} Code={1}", 
                    uiElement.GetType(), uiElement.GetHashCode()));
            }

            var currentElement = CurrentElement(datetype);
            currentElement.Cast<IActiveAware>().IsActive = true;

            Debug.WriteLine(string.Format(">>> Добавлен контрол типа {0} Code={1}",
                                          currentElement.GetType(), currentElement.GetHashCode()));

            thisObject.EditorsContainer.Children.Add(currentElement);
        }

        private static UIElement CurrentElement(DataTypeEnum datetype)
        {
            switch (datetype)
            {
                case DataTypeEnum.Bool:
                    return new BooleanEditor();
                case DataTypeEnum.Long:
                case DataTypeEnum.Double:
                    return new NumberEditor();
                case DataTypeEnum.String:
                    return new TextEditor();
                case DataTypeEnum.DateTime:
                    return new DateTimeEditor();
                case DataTypeEnum.Bin:
                    return new BinaryFileLoader();
                case DataTypeEnum.Xml:
                    return new XmlFileLoader();
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}