using System;
using ClientCode.Gameplay;
using UnityEngine.EventSystems;

namespace ClientCode.Data.Scene
{
    [Serializable]
    public class MapEditorSceneData : ISceneData
    {
        public CameraObject Camera;
        public EventSystem EventSystem;
    }
}