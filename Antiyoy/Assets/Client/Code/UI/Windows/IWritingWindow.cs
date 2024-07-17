using System.Threading.Tasks;
using ClientCode.UI.Windows.Base;

namespace ClientCode.UI.Windows
{
    public interface IWritingWindow : IWindow
    {
        Task<string> GetString();
        void Clear();
    }
}