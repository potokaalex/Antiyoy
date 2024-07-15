using System;
using ClientCode.Gameplay;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ClientCode.Data.Scene
{
    [Serializable]
    public class MapEditorSceneData : ISceneData
    {
        public Camera Camera;
        public EventSystem EventSystem;
        public Transform UIRoot;
    }
}