using ClientCode.Services.StaticDataProvider;
using ClientCode.UI;
using ClientCode.UI.Factory;
using UnityEngine;

namespace ClientCode.Services.CanvasService
{
    public class ProjectCanvasController
    {
        private readonly UIFactory _factory;
        private readonly IStaticDataProvider _staticDataProvider;
        private ProjectCanvasObject _canvas;

        public ProjectCanvasController(UIFactory factory, IStaticDataProvider staticDataProvider)
        {
            _factory = factory;
            _staticDataProvider = staticDataProvider;
        }

        public void Initialize() => _canvas = _factory.Create(_staticDataProvider.Prefabs.ProjectCanvasObject);

        public Transform DefaultElementsRoot => _canvas.DefaultElementsRoot;

        public Transform TopElementsRoot => _canvas.TopElementsRoot;
    }
}