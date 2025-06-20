using ClientCode.Infrastructure.States.MapEditor.MainMenu;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace ClientCode.Infrastructure.Installers
{
    public class SelectionButton : MonoBehaviour
    {
        public ButtonView Button;
        public Image Image;
        public Color Default;
        public Color Selected;

        public Subject<Unit> OnClickEvent => Button.OnClickEvent;
        
        public void Select() => Image.color = Selected;

        public void UnSelect() => Image.color = Default;
    }
}