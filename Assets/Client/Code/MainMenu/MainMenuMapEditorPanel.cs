using System;
using System.Collections.Generic;
using Client.Code.Services.UI.Window;
using ClientCode.Infrastructure.States.MapEditor.MainMenu;
using ClientCode.UI.Buttons.Map;
using UniRx;
using UnityEngine;

namespace ClientCode.UI.Windows.Writing
{
    public class MainMenuMapEditorPanel : WindowView, IDisposable
    {
        public ButtonView CreateButton;
        public ButtonView RemoveButton;
        public ButtonView BackButton;
        public Transform SelectButtonsRoot;
        public MapSelectButton SelectButtonPrefab;
        private readonly List<MapSelectButton> _selectButtons = new();
        private readonly CompositeDisposable _disposables = new();

        public void Initialize()
        {
            Close();
            BackButton.OnClickEvent.Subscribe(_ => Close()).AddTo(_disposables);
        }

        public void Dispose() => _disposables.Dispose();
    }
}