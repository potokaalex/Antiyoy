using System.Collections;
using ClientCode.UI.Windows.Base;
using TMPro;
using UnityEngine;

namespace ClientCode.UI.Windows.Popup
{
    public class PopupWindow : WindowBaseOld
    {
        [SerializeField] private TextMeshProUGUI _text;

        public void Initialize(string message)
        {
            _text.text = message;
            Open();
            StartCoroutine(Hide());
        }

        private IEnumerator Hide()
        {
            yield return new WaitForSeconds(5);
            Close();
        }
    }
}