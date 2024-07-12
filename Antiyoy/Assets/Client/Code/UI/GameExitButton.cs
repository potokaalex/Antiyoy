namespace Code.UI
{
    public class GameExitButton : ButtonBase
    {
        private protected override void OnClick()
#if UNITY_EDITOR
            => UnityEditor.EditorApplication.isPlaying = false;
#else
            => UnityEngine.Application.Quit();
#endif
    }
}