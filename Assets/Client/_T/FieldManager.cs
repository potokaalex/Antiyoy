using System.Collections.Generic;

namespace Antiyoy.Gameplay
{
  public class FieldManager
  {
    public List<Hex> activeHexes = new(); 
    public List<Hex> solidObjects = new();

    public int fWidth;
    public int fHeight;
    public Hex nullHex;
    public Hex[][] field;

    public float hexSize;//??
    public GameController gameController;

    public void Initialize(int fWidth, int fHeight, GameController gameController)
    {
      this.fWidth = fWidth;
      this.fHeight = fHeight;
      this.gameController = gameController;

      field = new Hex[fWidth][];
      for (int i = 0; i < fWidth; i++) 
        field[i] = new Hex[fHeight];
      
      nullHex = new Hex(-1, -1, gameController);
      nullHex.active = false;
    }

    public void clearActiveHexesList() => activeHexes.Clear();

    public Hex getHexByPos(double x, double y)
    {
      //слабо понимаю, как работает позиция, ну, т.е. это пространство в мире?
      throw new System.NotImplementedException();
    }

    public void spawnTree(Hex hex)
    {
      if (!hex.active) return;

      if (hex.isNearWater()) addSolidObject(hex, Obj.PALM);
      else addSolidObject(hex, Obj.PINE);
    }

    public void createFieldMatrix()
    {
      for (int i = 0; i < fWidth; i++)
      {
        field[i] = new Hex[fHeight];
        for (int j = 0; j < fHeight; j++) 
          field[i][j] = new Hex(i, j, gameController);
      }
    }

    public Hex adjacentHex(Hex hex, int direction) => adjacentHex(hex.index1, hex.index2, direction);

    public Hex adjacentHex(int i, int j, int direction)
    {
      switch (direction)
      {
        case 0:
          if (i >= fWidth - 1) return nullHex;
          return field[i + 1][j];
        case 1:
          if (j >= fHeight - 1) return nullHex;
          return field[i][j + 1];
        case 2:
          if (i <= 0 || j >= fHeight - 1) return nullHex;
          return field[i - 1][j + 1];
        case 3:
          if (i <= 0) return nullHex;
          return field[i - 1][j];
        case 4:
          if (j <= 0) return nullHex;
          return field[i][j - 1];
        case 5:
          if (i >= fWidth - 1 || j <= 0) return nullHex;
          return field[i + 1][j - 1];
        default:
          return nullHex;
      }
    }

    private void addSolidObject(Hex hex, int type)
    {
      if (hex == null || !hex.active) return;
      if (hex.objectInside == type) return;
      if (!hex.canContainObjects) return;

      if (solidObjects.Contains(hex))
      {
        cleanOutHex(hex);
      }

      hex.setObjectInside(type);
      solidObjects.Add(hex);
    }

    public void cleanOutHex(Hex hex)
    {
      hex.setObjectInside(0);
      solidObjects.Remove(hex);
    }
  }
}