using System.Collections.Generic;
using ClientCode.UI.Buttons.Base;
using UnityEngine;

namespace ClientCode.UI.Factory
{
    public class ButtonsFactory
    {
        private readonly Dictionary<ButtonType, List<ButtonBase>> _buttons = new();
        private readonly UIFactory _factory;

        public ButtonsFactory(UIFactory factory) => _factory = factory;

        public ButtonBase Create(ButtonType type, Transform root, IButtonsHandler handler)
        {
            if (!_buttons.TryGetValue(type, out var buttons) || buttons.Count == 0)
                return CreateButton(type, root, handler);

            var button = Enable(type, root);

            return button;
        }

        public void Destroy(ButtonBase button)
        {
            if (!_buttons.ContainsKey(button.GetBaseType()))
                _buttons.Add(button.GetBaseType(), new List<ButtonBase>());
            
            var buttons = _buttons[button.GetBaseType()];
            
            buttons.Add(button);
            button.gameObject.SetActive(false);
        }
        
        private ButtonBase Enable(ButtonType type, Transform root)
        {
            var buttons = _buttons[type];
            var button = buttons[0];
            
            buttons.Remove(button);
            button.transform.SetParent(root, false);
            button.gameObject.SetActive(true);
            
            return button;
        }
        
        private ButtonBase CreateButton(ButtonType type, Transform root, IButtonsHandler handler)
        {
            var button = _factory.CreateButton(type, root, args: handler);
            return button;
        }
    }
}