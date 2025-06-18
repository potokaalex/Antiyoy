using ClientCode.UI.Windows.Base;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace ClientCode.UI.Windows.Writing
{
    public class WritingWindow : WindowBaseOld, IWritingWindow
    {
        [SerializeField] private TMP_InputField _field;

        public UniTask<string> GetString()
        {
            var taskSource = new UniTaskCompletionSource<string>();

            _field.onEndEdit.AddListener(OnEndEdit);

            return taskSource.Task;

            void OnEndEdit(string result)
            {
                _field.onEndEdit.RemoveListener(OnEndEdit);
                taskSource.TrySetResult(result);
            }
        }

        public void Clear() => _field.text = string.Empty;
    }
}