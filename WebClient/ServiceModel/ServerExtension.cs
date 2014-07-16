using System.ComponentModel;

namespace WebClient.ICS.Client.ServiceModel
{
    /// <summary>
    /// ServerExtension.
    /// </summary>
    public static class ServerExtension
    {
        /// <summary>
        /// Raise exception if error.
        /// </summary>
        /// <param name="args"></param>
        public static void RaiseException(this AsyncCompletedEventArgs args)
        {
            new AsyncCompletedEventArgsEx(args.Error, args.Cancelled, args.UserState).RaiseExceptionIfError();
        }
    }
}
