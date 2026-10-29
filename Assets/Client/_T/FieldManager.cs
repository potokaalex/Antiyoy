using System.Collections.Generic;
using Client.Project.Hex;
using UnityEngine;

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

    // Array coordinates are the canonical coordinates used by map generation.
    public Hex getHex(int x, int y)
    {
      if (x < 0 || y < 0 || x >= fWidth || y >= fHeight) return nullHex;
      return field[x][y];
    }

    public void spawnTree(Hex hex)
    {
      if (!hex.active) return;

      if (hex.isNearWater()) addSolidObject(hex, Obj.PALM);
      else addSolidObject(hex, Obj.PINE);
    }

    public void createFieldMatrix()
    {
      activeHexes.Clear();
      solidObjects.Clear();
      for (int i = 0; i < fWidth; i++)
      {
        field[i] = new Hex[fHeight];
        for (int j = 0; j < fHeight; j++) 
          field[i][j] = new Hex(i, j, gameController);
      }
    }

    public Hex adjacentHex(Hex hex, int direction) => adjacentHex(hex.index1, hex.index2, direction);

    private Hex adjacentHex(int i, int j, int direction)
    {
      if (direction < 0 || direction >= HexUtilities.Directions.Length)
        return nullHex;

      var offset = HexUtilities.Directions[direction];
      var root = HexCoordinates.FromArray2DIndex(new Vector2Int(i, j));
      var index = (root + offset).ToArray2DIndex();

      if (index.x < 0 || index.x >= fWidth || index.y < 0 || index.y >= fHeight)
        return nullHex;

      return field[index.x][index.y];
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