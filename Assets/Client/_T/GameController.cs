using Client.Project.Map;

namespace Antiyoy.Gameplay
{
  public class GameController
  {
    public FieldManager fieldManager;
    public LevelSizeManager levelSizeManager;

    public GameController(int fWidth, int fHeight, LevelSize levelSize)
    {
      levelSizeManager = new(levelSize);
      fieldManager = new();
      fieldManager.Initialize(fWidth, fHeight, this);
    }
  }
}