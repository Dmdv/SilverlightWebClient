using System;
using System.ServiceModel;
using System.Threading;
using WebClient.ICS.Client.Model;
using WebClient.ICS.Client.ServiceReference;

namespace WebClient.ICS.Client.ServiceModel
{
    public static class Service
    {
        private static IcsContractClient _contractClient;
        private static readonly ManualResetEvent _event = new ManualResetEvent(false);

        /// <summary>
        /// Returns singlton service proxy instance.
        /// </summary>
        public static IServiceClient Proxy { get; private set; }

        /// <summary>
        /// Configures service.  Used only for first launch.
        /// </summary>
        public static void Configure()
        {
            if (_contractClient != null)
            {
                throw new InvalidProgramException("The 'Configure' function is used only for first launch");
            }

            _contractClient = new IcsContractClient();
            _contractClient.OpenCompleted += OpenCompleted;
            _contractClient.CloseCompleted += CloseCompleted;
            _contractClient.OpenAsync();

            Proxy = new ServiceProxy(_contractClient);

            _event.WaitOne(1000);
            _event.Reset();
        }

        /// <summary>
        /// Configures with address. Used only for first launch.
        /// </summary>
        /// <param name="address"></param>
        public static void Configure(string address)
        {
            if (_contractClient != null)
            {
                throw new InvalidProgramException("The 'Configure' function is used only for first launch");
            }

            _contractClient = new IcsContractClient(BindingCreator.CreateHttpBinding(), new EndpointAddress(address));
            _contractClient.OpenCompleted += OpenCompleted;
            _contractClient.CloseCompleted += CloseCompleted;
            _contractClient.OpenAsync();

            Proxy = new ServiceProxy(_contractClient);

            _event.WaitOne(1000);
            _event.Reset();
        }

        public static void Reconnect(string address)
        {
            if (_contractClient != null && _contractClient.State != CommunicationState.Closed)
            {
                _contractClient.CloseAsync(address);
            }
        }

        public static void Close()
        {
            if (_contractClient != null)
            {
                _contractClient.CloseAsync();
            }
        }

        private static void CloseCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            if (e.UserState == null) return;

            var address = e.UserState.Cast<string>();

            if (_contractClient != null)
            {
                _contractClient.OpenCompleted -= OpenCompleted;
                _contractClient.CloseCompleted -= CloseCompleted;
                _contractClient = null;
            }

            Configure(address);
        }

        private static void OpenCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            _event.Set();
        }
    }
}
