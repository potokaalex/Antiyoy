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
        public MapEditorPanelMapSelector MapSelector;
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
            MapSelector.Initialize();
            MapSelector.AddTo(_disposables);
        }

        public void Dispose() => _disposables.Dispose();

        protected override void OnClose(bool force) => MapSelector.UnSelect();

        private void LoadEditor()
        {
            if (MapSelector.HasSelection)
                _projectManager.LoadEditor(MapSelector.SelectedValue);
        }

        private void OnRemove()
        {
            if (MapSelector.HasSelection)
                _mapsFactory.Destroy(MapSelector.SelectedValue);
        }
    }
}