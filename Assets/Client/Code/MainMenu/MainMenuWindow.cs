using System;
using ClientCode.Infrastructure.Installers;
using UniRx;
using UnityEngine;
using Zenject;

namespace ClientCode.Infrastructure.States.MapEditor.MainMenu
{
    public class MainMenuWindow : MonoBehaviour, IInitializable, IDisposable
    {
        public ButtonView Battle;
        public ButtonView Editor;
        public ButtonView Exit;
        private readonly CompositeDisposable _disposables = new();
        private ProjectManager _projectManager;

        [Inject]
        public void Construct(ProjectManager projectManager) => _projectManager = projectManager;

        public void Initialize()
        {
            Battle.OnClickAsObservable().Subscribe(_ => _projectManager.LoadBattle()).AddTo(_disposables);
            Editor.OnClickAsObservable().Subscribe(_ => _projectManager.LoadEditor()).AddTo(_disposables);
            Exit.OnClickAsObservable().Subscribe(_ => _projectManager.Exit()).AddTo(_disposables);
        }

        public void Dispose() => _disposables.Dispose();
    }
}