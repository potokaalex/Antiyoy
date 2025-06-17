using ClientCode.Services.StaticDataProvider;
using ClientCode.UI.Buttons.Base;
using ClientCode.UI.Windows.Base;
using ClientCode.Utilities.Extensions;
using UnityEngine;
using Zenject;

namespace ClientCode.UI.Factory
{
    public class UIFactory
    {
        private readonly IStaticDataProvider _staticData;
        private readonly IInstantiator _instantiator;
        private Transform _uiRoot;

        public UIFactory(IStaticDataProvider staticData, IInstantiator instantiator, Transform uiRoot = null)
        {
            _staticData = staticData;
            _instantiator = instantiator;
            _uiRoot = uiRoot;
        }

        public void Initialize(Transform uiRoot) => _uiRoot = uiRoot;

        public ButtonBase CreateButton(ButtonType type, Transform root, params object[] args) =>
            Create(_staticData.Prefabs.Buttons[type], root, args: args);

        public WindowBase CreateWindow(WindowType type) =>
            Create(_staticData.Prefabs.Windows[type], _uiRoot, args: type);

        public T Create<T>(T prefab, Transform parent = null, bool instantiateInWorldSpace = false, params object[] args)
            where T : MonoBehaviour, IUIElement => _instantiator.InstantiateMonoBehaviour(prefab, parent, instantiateInWorldSpace, args);

        public void Destroy<T>(T element) where T : MonoBehaviour, IUIElement => Object.Destroy(element.gameObject);
    }
}