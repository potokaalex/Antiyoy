using ClientCode.UI;
using UnityEngine;

namespace ClientCode.Services.CanvasService
{
    public class ProjectCanvasObject : MonoBehaviour, IUIElement
    {
        public Transform DefaultElementsRoot;
        public Transform TopElementsRoot;
    }
}