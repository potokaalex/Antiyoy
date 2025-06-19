using System;
using Client.Code.Services.UI.Window;
using ClientCode.Infrastructure.Installers;
using ClientCode.Infrastructure.States.MapEditor.MainMenu;
using UniRx;
using Zenject;

namespace ClientCode.UI.Windows.Writing
{
    public class MapEditorPanel : WindowView, IDisposable
    {
        public ButtonView BackButton;
        public ButtonView CreateButton;
        public ButtonView RemoveButton;
        public ButtonView LoadButton;
        public MapEditorPanelSelector Selector;
        private readonly CompositeDisposable _disposables = new();
        private MapsFactory _mapsFactory;
        private ProjectManager _projectManager;

        [Inject]
        public void Construct(MapsFactory mapsFactory, ProjectManager projectManager)
        {
            _projectManager = projectManager;
            _mapsFactory = mapsFactory;
        }

        public void Initialize()
        {
            Close();
            BackButton.OnClickEvent.Subscribe(_ => Close()).AddTo(_disposables);
            CreateButton.OnClickEvent.Subscribe(_ => _mapsFactory.Create()).AddTo(_disposables);
            RemoveButton.OnClickEvent.Subscribe(_ => OnRemove()).AddTo(_disposables);
            LoadButton.OnClickEvent.Subscribe(_ => LoadEditor()).AddTo(_disposables);
            Selector.AddTo(_disposables).Initialize();
        }

        public void Dispose() => _disposables.Dispose();

        protected override void OnClose(bool force) => Selector.UnSelect();

        private void LoadEditor()
        {
            if (Selector.HasSelection)
                _projectManager.LoadEditor(Selector.SelectedMap);
        }

        private void OnRemove()
        {
            if (Selector.HasSelection)
                _mapsFactory.Destroy(Selector.SelectedMap);
        }
    }
}