using System;

namespace WebClient.ICS.Client.ViewModel
{
    /// <summary>
    /// Сообщение об исключении в NodeEditor.
    /// </summary>
    public class EditorInvalidOperationArgs : EventArgs
    {
        public EditorInvalidOperationArgs(string message)
        {
            Message = message;
        }

        public string Message { get; private set; }
    }
}