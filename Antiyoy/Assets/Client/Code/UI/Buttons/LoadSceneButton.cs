using UnityEngine;
using Zenject;

namespace ClientCode.UI.Buttons
{
    public class LoadSceneButton : ButtonBase
    {
        [SerializeField] private SceneType _sceneType;
        private ILoadSceneButtonHandler _handler;

        [Inject]
        private void Construct(ILoadSceneButtonHandler handler) => _handler = handler;

        private protected override void OnClick() => _handler?.Handle(_sceneType);
    }
}