namespace WebClient.ICS.Client.ViewModel
{
    public interface IModelUpdater
    {
        /// <summary>
        /// Returns true is has any changes.
        /// </summary>
        bool HasChanges { get; }

        /// <summary>
        /// SaveChanges and resets HasChanges flag.
        /// </summary>
        /// <param name="obj">Paraneter is used from inside a ICommand, should be null otherwise.</param>
        void SaveChanges(object obj);

        /// <summary>
        /// Reset any changes.
        /// </summary>
        void Reset();
    }
}