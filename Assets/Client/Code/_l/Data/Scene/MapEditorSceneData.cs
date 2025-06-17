using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ClientCode.Data.Scene
{
    [Serializable]
    public class MapEditorSceneData
    {
        public Camera Camera;
        public EventSystem EventSystem;
        public Transform UIRoot;
    }
}