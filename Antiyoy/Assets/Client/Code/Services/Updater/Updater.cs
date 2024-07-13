using System;
using UnityEngine;

namespace ClientCode.Services.Updater
{
    public class Updater : MonoBehaviour, IUpdater
    {
        private void Update() => OnUpdate?.Invoke();

        private void FixedUpdate() => OnFixedUpdate?.Invoke();

        private void OnDestroy() => OnProjectExit?.Invoke();
        public event Action OnUpdate;
        public event Action OnFixedUpdate;
        public event Action OnProjectExit;
    }
}