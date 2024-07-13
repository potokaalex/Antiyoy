namespace ClientCode.UI.Buttons
{
    public class ProjectExitButton : ButtonBase
    {
        private protected override void OnClick()
#if UNITY_EDITOR
            => UnityEditor.EditorApplication.isPlaying = false;
#else
            => UnityEngine.Application.Quit();
#endif
    }
}