using System.Threading.Tasks;
using ClientCode.UI.Windows.Base;
using TMPro;
using UnityEngine;

namespace ClientCode.UI.Windows.Writing
{
    public class WritingWindow : WindowBase, IWritingWindow
    {
        [SerializeField] private TMP_InputField _field;

        public Task<string> GetString()
        {
            var taskSource = new TaskCompletionSource<string>();

            _field.onEndEdit.AddListener(OnEndEdit);

            return taskSource.Task;

            void OnEndEdit(string result)
            {
                _field.onEndEdit.RemoveListener(OnEndEdit);
                taskSource.SetResult(result);
            }
        }

        public void Clear() => _field.text = string.Empty;
    }
}