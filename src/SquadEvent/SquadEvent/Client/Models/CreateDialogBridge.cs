using System;

namespace SquadEvent.Client.Models
{
    public class CreateDialogBridge
    {
        public event EventHandler OnOpenCreate;

        public void InvokeOpenCreate()
        {
            OnOpenCreate?.Invoke(this, EventArgs.Empty);
        }
    }
}