using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UniRx;

namespace Client.Code.Services.UI.Buttons.Select
{
    public class SelectionButtonsController<T> : SerializedMonoBehaviour, IDisposable
    {
        public Dictionary<SelectionButton, T> Buttons = new();
        private readonly CompositeDisposable _disposables = new();

        public bool HasSelection => Selected;

        public SelectionButton Selected { get; private set; }

        public T SelectedValue => Buttons[Selected];

        public Subject<SelectionButton> OnSelected { get; } = new();

        public virtual void Initialize()
        {
            foreach (var (button, _) in Buttons)
                Sub(button);
        }

        public virtual void Dispose() => _disposables.Dispose();

        public void Add(SelectionButton button, T value)
        {
            Buttons[button] = value;
            Sub(button);
        }

        public void Clear()
        {
            _disposables.Clear();
            Buttons.Clear();
        }

        public void TrySelect(SelectionButton button)
        {
            if (button.Equals(Selected))
            {
                UnSelect();
                return;
            }

            UnSelect();
            Selected = button;
            Selected.Select();
            OnSelected.OnNext(Selected);
        }

        public void UnSelect()
        {
            Selected?.UnSelect();
            Selected = null;
        }

        private void Sub(SelectionButton button) => button.OnClickEvent.Subscribe(_ => TrySelect(button)).AddTo(_disposables);
    }
}