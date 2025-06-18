using System;
using ClientCode.Infrastructure.Installers;
using ClientCode.UI.Windows.Writing;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;
using Zenject;

namespace ClientCode.Infrastructure.States.MapEditor.MainMenu
{
    public class MainMenuWindow : MonoBehaviour, IInitializable, IDisposable
    {
        public GameObject Body;
        public ButtonView Battle;
        public ButtonView Editor;
        public ButtonView Exit;
        public MainMenuMapEditorPanel EditorPanel;
        private readonly CompositeDisposable _disposables = new();
        private ProjectManager _projectManager;

        [Inject]
        public void Construct(ProjectManager projectManager) => _projectManager = projectManager;

        public void Initialize()
        {
            Battle.OnClickAsObservable().Subscribe(_ => _projectManager.LoadBattle()).AddTo(_disposables);
            Editor.OnClickAsObservable().Subscribe(_ => ShowEditorPreloadPanel()).AddTo(_disposables);
            Exit.OnClickAsObservable().Subscribe(_ => _projectManager.Exit()).AddTo(_disposables);
            EditorPanel.AddTo(_disposables).Initialize();
            EditorPanel.Hide();
        }

        public void Dispose() => _disposables.Dispose();

        private void ShowEditorPreloadPanel()
        {
            Hide();
            EditorPanel.Show();
            EditorPanel.BackButton.OnClickAsObservable().First().Subscribe(_ =>
            {
                Show();
                EditorPanel.Hide();
            }).AddTo(_disposables);
        }

        private void Show() => Body.SetActive(true);
        
        private void Hide() => Body.SetActive(false);
    }
}