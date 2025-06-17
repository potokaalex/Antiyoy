using System;

namespace ClientCode.Services.Updater
{
    public interface IUpdater
    {
        event Action OnUpdate;
        event Action OnFixedUpdate;
        event Action OnProjectExit;

        public void ClearAllListeners();
    }
}