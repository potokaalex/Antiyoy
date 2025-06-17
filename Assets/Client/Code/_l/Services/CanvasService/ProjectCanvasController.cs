using ClientCode.Services.StaticDataProvider;
using ClientCode.UI.Factory;
using UnityEngine;

namespace ClientCode.Services.CanvasService
{
    public class ProjectCanvasController
    {
        private readonly UIFactory _factory;
        private readonly IStaticDataProvider _staticData;
        private ProjectCanvasObject _canvas;

        public ProjectCanvasController(UIFactory factory, IStaticDataProvider staticData)
        {
            _factory = factory;
            _staticData = staticData;
        }

        public void Initialize() => _canvas = _factory.Create(_staticData.Prefabs.ProjectCanvasObject);

        public Transform DefaultElementsRoot => _canvas.DefaultElementsRoot;

        public Transform TopElementsRoot => _canvas.TopElementsRoot;
    }
}