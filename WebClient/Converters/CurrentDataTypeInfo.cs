using System.Windows;
using WebClient.ICS.Client.Model;

namespace WebClient.ICS.Client.Converters
{
    /// <summary>
    /// Предоставляет информацию о текущем типе данных контекста контрола, содержащего средства 
    /// для редактирования значений настроек.
    /// Методы вызываются всякий раз при смене текущего узла настроек, когда меняется DataContext 
    /// панели редактирования.
    /// </summary>
    public class CurrentDataTypeInfo : FrameworkElement
    {
        //TODO: static котейнер для хранения по хешу контекста разных панелей.

        // ReSharper disable UnusedMember.Global
        // ReSharper disable MemberCanBePrivate.Global

        /// <summary>
        /// The <see cref="DataType" /> dependency property's name.
        /// </summary>
        private const string DataTypePropertyName = "DataType";

        /// <summary>
        /// Gets or sets the value of the <see cref="DataType" />
        /// property. This is a dependency property.
        /// </summary>
        public DataTypeEnum DataType
        {
            get { return (DataTypeEnum) GetValue(DataTypeProperty); }
            set { SetValue(DataTypeProperty, value); }
        }

        public static DataTypeEnum DataTypeStatic { get; set; }

        /// <summary>
        /// Identifies the <see cref="DataType" /> dependency property.
        /// </summary>
        public static readonly DependencyProperty DataTypeProperty = DependencyProperty.Register(
            DataTypePropertyName,
            typeof (DataTypeEnum),
            typeof (CurrentDataTypeInfo),
            new PropertyMetadata(ValueChangedCallback));

        private static void ValueChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue != null) DataTypeStatic = (DataTypeEnum) e.NewValue;
        }
    }
}