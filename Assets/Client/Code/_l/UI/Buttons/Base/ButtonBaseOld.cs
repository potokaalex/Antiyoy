using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ClientCode.UI.Buttons.Base
{
    [RequireComponent(typeof(Image), typeof(Button))]
    public abstract class ButtonBaseOld : MonoBehaviour, IUIElement
    {
        private Button _button;
        private IButtonsHandler _handler;

        [Inject]
        public void BaseConstruct(IButtonsHandler handler) => _handler = handler;

        public abstract ButtonType GetBaseType();

        private void Awake()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(OnClick);
        }

        private void OnDestroy() => _button.onClick.RemoveListener(OnClick);

        private protected virtual void OnClick() => _handler.Handle(this);
    }
}