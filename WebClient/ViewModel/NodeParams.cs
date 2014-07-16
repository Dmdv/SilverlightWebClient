using WebClient.ICS.Client.ServiceReference;

namespace WebClient.ICS.Client.ViewModel
{
    /// <summary>
    /// Хранение констант.
    /// </summary>
    public static class NodeParams
    {
        /// <summary>
        /// RootId.
        /// </summary>
        public static int RootId
        {
            get { return 296124123; }
        }

        /// <summary>
        /// RootName.
        /// </summary>
        public static string RootName
        {
            get { return "VirtualRoot"; }
        }

        /// <summary>
        /// IsRoot.
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static bool IsRoot(this Element element)
        {
            return element.Id == RootId;
        }

        /// <summary>
        /// IsRoot.
        /// </summary>
        /// <param name="vmModel"></param>
        /// <returns></returns>
        public static bool IsRoot(this SettingNodeViewModel vmModel)
        {
            return vmModel.Element.IsRoot();
        }
    }
}