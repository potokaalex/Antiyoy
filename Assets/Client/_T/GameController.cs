namespace Antiyoy.Gameplay
{
  public class GameController
  {
    public FieldManager fieldManager;
    public LevelSizeManager levelSizeManager = new(LevelSize.Small);

    public GameController(int fWidth, int fHeight)
    {
      fieldManager = new();
      fieldManager.Initialize(fWidth, fHeight, this);
    }
  }
}