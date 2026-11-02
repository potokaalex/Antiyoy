namespace Client.MapEditor.UI.Panels
{
  public interface IMapEditorHudPanelView
  {
    float BottomOffsetY { get; }
    void Show();
    void Hide();
  }
}