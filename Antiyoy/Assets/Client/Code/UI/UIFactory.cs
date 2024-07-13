using ClientCode.Services.StaticDataProvider;
using ClientCode.UI.Windows.Base;
using ClientCode.Utilities.Extensions;
using UnityEngine;
using Zenject;

namespace ClientCode.UI
{
    public class UIFactory
    {
        private readonly IStaticDataProvider _staticDataProvider;
        private readonly IInstantiator _instantiator;

        public UIFactory(IStaticDataProvider staticDataProvider, IInstantiator instantiator)
        {
            _staticDataProvider = staticDataProvider;
            _instantiator = instantiator;
        }

        public IWindow CreateWindow(WindowType type, Transform root) =>
            _instantiator.InstantiateMonoBehaviour(_staticDataProvider.Prefabs.Windows[type], root);
    }
}