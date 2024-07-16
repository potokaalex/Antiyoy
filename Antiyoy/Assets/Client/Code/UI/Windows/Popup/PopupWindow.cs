using System.Collections;
using ClientCode.UI.Windows.Base;
using TMPro;
using UnityEngine;

namespace ClientCode.UI.Windows.Popup
{
    public class PopupWindow : WindowBase
    {
        [SerializeField] private TextMeshProUGUI _text;

        public void Initialize(string message)
        {
            _text.text = message;
            Open();
            //TODO: animation!
            StartCoroutine(Hide());
        }

        private IEnumerator Hide()
        {
            yield return new WaitForSeconds(5);
            Close();
        }
    }
}