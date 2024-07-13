using ClientCode.UI.Windows.Base;
using UnityEngine;

namespace ClientCode.UI.Windows
{
    public class MapEditorPreloadWindow : WindowBase
    {
        public override void Open()
        {
            IsOpen = true;
            gameObject.SetActive(true);
        }
        
        public override void Close()
        {
            IsOpen = false;
            gameObject.SetActive(false);
        }
    }
}