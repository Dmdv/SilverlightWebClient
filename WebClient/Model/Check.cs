namespace WebClient.ICS.Client.Model
{
    /// <summary>
    /// Проверка валидности аргументов без выбрасывания исключения.
    /// </summary>
    public static class Check
    {
        /// <summary>
        /// Не допускает значение выше 500.000 байтов для типов Bin.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="bytes"></param>
        public static bool Less500TBytes(DataTypeEnum? type, byte[] bytes)
        {
            if (type != DataTypeEnum.Bin) return true;
            return bytes.Length <= 500000;
        }
    }
}