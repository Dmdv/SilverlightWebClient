using System;
using System.ComponentModel;

namespace WebClient.ICS.Client.ServiceModel
{
    internal class AsyncCompletedEventArgsEx : AsyncCompletedEventArgs
    {
        public AsyncCompletedEventArgsEx(Exception error, bool cancelled, object userState) 
            : base(error, cancelled, userState)
        {
        }

        public void RaiseExceptionIfError()
        {
            base.RaiseExceptionIfNecessary();
        }
    }
}