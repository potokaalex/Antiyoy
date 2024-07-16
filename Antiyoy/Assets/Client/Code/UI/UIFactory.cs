using ClientCode.Services.StaticDataProvider;
using ClientCode.UI.Buttons.Base;
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

        public WindowBase CreateWindow(WindowType type, Transform root) =>
            _instantiator.InstantiateMonoBehaviour(_staticDataProvider.Prefabs.Windows[type], root, args: type);

        public ButtonBase CreateButton(ButtonType type, Transform root) =>
            _instantiator.InstantiateMonoBehaviour(_staticDataProvider.Prefabs.Buttons[type], root);

        public T Create<T>(T prefab) where T : MonoBehaviour, IUIElement => 
            _instantiator.InstantiateMonoBehaviour(prefab);

        public void Destroy<T>(T element) where T : MonoBehaviour, IUIElement => Object.Destroy(element.gameObject);
    }
}