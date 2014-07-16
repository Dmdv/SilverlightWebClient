namespace WebClient.ICS.Client.Model
{
    /// <summary>
    /// Тип редактора.
    /// </summary>
    public enum EditType : byte
    {
        /// <summary>
        /// Текстовое поле.
        /// </summary>
        Text,

        /// <summary>
        /// Xml файл.
        /// </summary>
        XmlFileLoader,

        /// <summary>
        /// Файл любого типа.
        /// </summary>
        FileLoader,

        /// <summary>
        /// Тип 'Дата'
        /// </summary>
        Date
    }
}