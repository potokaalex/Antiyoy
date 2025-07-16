using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Client.Code.Core.UI.Buttons
{
    public class ButtonView : MonoBehaviour, IPointerClickHandler
    {
        public Subject<Unit> OnClickEvent { get; } = new();

        public void OnPointerClick(PointerEventData eventData)
        {
            OnClick();
            OnClickEvent.OnNext(default);
        }

        protected virtual void OnClick()
        {
        }
    }
}