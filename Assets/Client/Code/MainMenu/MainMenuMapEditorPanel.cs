using System;
using System.Collections.Generic;
using Client.Code.Services;
using Client.Code.Services.Progress;
using ClientCode.Infrastructure.States.MapEditor.MainMenu;
using ClientCode.UI.Buttons.Base;
using ClientCode.UI.Buttons.Map;
using ClientCode.UI.Windows.Base;
using UniRx;
using UnityEngine;
using Zenject;

namespace ClientCode.UI.Windows.Writing
{
    public class MainMenuMapEditorPanel : MonoBehaviour, IDisposable
    {
        public ButtonView CreateButton;
        public ButtonView RemoveButton;
        public ButtonView BackButton;
        public Transform SelectButtonsRoot;
        public MapSelectButton SelectButtonPrefab;
        private readonly List<MapSelectButton> _selectButtons = new();
        private IProvider<ProgressData> _progressData;

        [Inject]
        public void Construct(IProvider<ProgressData> progressData)
        {
            _progressData = progressData;
        }

        public void Initialize()
        {
            //_progressData.Value;
            //1) подхватить прогресс по текущим картам и отобразить текущие карты. 
            //2) создавать, удалять карты.
            //3) грузить карты.
        }

        public void Dispose()
        {
        }

        public void Show() => gameObject.SetActive(true);

        public void Hide() => gameObject.SetActive(false);
    }
}