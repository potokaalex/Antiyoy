using UnityEngine;
using UnityEngine.UI;

namespace ClientCode.UI.Buttons.Base
{
    [RequireComponent(typeof(Image), typeof(Button))]
    public abstract class ButtonBase : MonoBehaviour, IUIElement
    {
        private Button _button;

        private void Awake()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(OnClick);
        }

        private void OnDestroy() => _button.onClick.RemoveListener(OnClick);

        private protected abstract void OnClick();
    }
}