using System.ServiceModel;
using System.ServiceModel.Channels;

namespace WebClient.ICS.Client.ServiceModel
{
    /// <summary>
    /// Creates binding.
    /// </summary>
    public static class BindingCreator
    {
        /// <summary>
        /// BasicHttpBinding.
        /// </summary>
        /// <returns></returns>
        public static Binding CreateHttpBinding()
        {
            return new BasicHttpBinding(BasicHttpSecurityMode.TransportCredentialOnly)
                              {
                                  MaxBufferSize = 2147483647,
                                  MaxReceivedMessageSize = 2147483647
                              };
        }
    }
}
