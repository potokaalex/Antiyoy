using ClientCode.UI.Windows.Base;
using Cysharp.Threading.Tasks;

namespace ClientCode.UI.Windows
{
    public interface IWritingWindow : IWindow
    {
        UniTask<string> GetString();
        void Clear();
    }
}