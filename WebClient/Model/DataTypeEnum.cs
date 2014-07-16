namespace WebClient.ICS.Client.Model
{
    /// <summary>
    /// Ics setting data type
    /// </summary>
    public enum DataTypeEnum : byte
    {
        /// <summary>
        /// Bool
        /// </summary>
        Bool = 0,

        /// <summary>
        /// Long.
        /// </summary>
        Long = 1,

        /// <summary>
        /// Double.
        /// </summary>
        Double = 2,

        /// <summary>
        /// String.
        /// </summary>
        String = 3,

        /// <summary>
        /// DateTime.
        /// </summary>
        DateTime = 4,

        /// <summary>
        /// Xml
        /// </summary>
        Xml = 5,

        /// <summary>
        /// Bin.
        /// </summary>
        Bin = 6
    }
}