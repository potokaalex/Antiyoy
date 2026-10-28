namespace Antiyoy.Gameplay
{
  public class LevelSizeManager
  {
    public LevelSize levelSize;
    public float boundWidth;
    public float boundHeight;

    public LevelSizeManager(LevelSize levelSize)
    {
      this.levelSize = levelSize;
      updateBounds();
    }

    private void updateBounds()
    {
      switch (levelSize)
      {
        case LevelSize.SMALL:
          boundWidth = GraphicsYio.width;
          boundHeight = GraphicsYio.height;
          break;
        case LevelSize.MEDIUM:
          boundWidth = 2 * GraphicsYio.width;
          boundHeight = GraphicsYio.height;
          break;
        case LevelSize.BIG:
          boundWidth = 2 * GraphicsYio.width;
          boundHeight = 2 * GraphicsYio.height;
          break;
        case LevelSize.HUGE:
          boundWidth = 3 * GraphicsYio.width;
          boundHeight = 3 * GraphicsYio.height;
          break;
      }
    }
  }
}