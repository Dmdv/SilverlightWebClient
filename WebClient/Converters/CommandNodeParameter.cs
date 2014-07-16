using WebClient.ICS.Client.ViewModel;

namespace WebClient.ICS.Client.Converters
{
    /// <summary>
    /// Parameter.
    /// </summary>
    public class CommandNodeParameter
    {
        /// <summary>
        /// Parent ViewModel.
        /// </summary>
        public SettingNodeViewModel SettingNode { get; set; }

        /// <summary>
        /// NodeName.
        /// </summary>
        public string NodeName { get; set; }
    }
}