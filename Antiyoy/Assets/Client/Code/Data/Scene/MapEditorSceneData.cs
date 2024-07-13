using System;
using ClientCode.Gameplay;

namespace ClientCode.Data.Scene
{
    [Serializable]
    public class MapEditorSceneData : ISceneData
    {
        public CameraObject Camera;
    }
}