using System;
using Client.Code.MainMenu.MapEditor;
using Client.Code.Project;
using Client.Code.Services.UI.Buttons;
using UniRx;
using UnityEngine;
using Zenject;

namespace Client.Code.MainMenu
{
    public class MainMenuWindow : MonoBehaviour, IInitializable, IDisposable
    {
        public GameObject BodyRoot;
        public ButtonView BattleButton;
        public ButtonView EditorButton;
        public ButtonView ExitButton;
        public MapEditorPanel EditorPanel;
        private readonly CompositeDisposable _disposables = new();
        private ProjectManager _projectManager;

        [Inject]
        public void Construct(ProjectManager projectManager) => _projectManager = projectManager;

        public void Initialize()
        {
            BattleButton.OnClickEvent.Subscribe(_ => _projectManager.LoadBattle()).AddTo(_disposables);
            ExitButton.OnClickEvent.Subscribe(_ => _projectManager.Exit()).AddTo(_disposables);
            InitializeEditor();
        }

        public void Dispose() => _disposables.Dispose();

        private void InitializeEditor()
        {
            EditorButton.OnClickEvent.Subscribe(_ => EditorPanel.Open()).AddTo(_disposables);
            EditorPanel.AddTo(_disposables).Initialize();
            EditorPanel.OnOpenEvent.Subscribe(_ => Hide()).AddTo(_disposables);
            EditorPanel.OnCloseEvent.Subscribe(_ => Show()).AddTo(_disposables);
        }

        private void Show() => BodyRoot.SetActive(true);

        private void Hide() => BodyRoot.SetActive(false);
    }
}