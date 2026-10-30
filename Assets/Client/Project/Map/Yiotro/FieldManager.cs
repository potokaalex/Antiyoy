using System.Collections.Generic;
using Client.Project.Hex;
using UnityEngine;

namespace Client.Project.Map.Yiotro
{
  public class FieldManager
  {
    public readonly List<Hex> ActiveHexes = new();
    public int FWidth;
    public int FHeight;
    public Hex NullHex;
    public Hex[][] Field;
    private readonly List<Hex> _solidObjects = new();

    public void Setup(Vector2Int size)
    {
      FWidth = size.x;
      FHeight = size.y;

      Field = new Hex[FWidth][];
      for (var i = 0; i < FWidth; i++)
        Field[i] = new Hex[FHeight];

      NullHex = new Hex(-1, -1, this);
      NullHex.Active = false;
    }

    public void ClearActiveHexesList() => ActiveHexes.Clear();

    public void SpawnTree(Hex hex)
    {
      if (!hex.Active)
        return;
      AddSolidObject(hex, hex.IsNearWater() ? YiotroMapGenerator.OBJ_PALM : YiotroMapGenerator.OBJ_PINE);
    }

    public void CreateFieldMatrix()
    {
      ActiveHexes.Clear();
      _solidObjects.Clear();
      for (var i = 0; i < FWidth; i++)
      {
        Field[i] = new Hex[FHeight];
        for (var j = 0; j < FHeight; j++)
          Field[i][j] = new Hex(i, j, this);
      }
    }

    public Hex AdjacentHex(Hex hex, int direction) => AdjacentHex(hex.Index1, hex.Index2, direction);

    private Hex AdjacentHex(int i, int j, int direction)
    {
      if (direction < 0 || direction >= HexUtilities.Directions.Length)
        return NullHex;

      var offset = HexUtilities.Directions[direction];
      var root = HexCoordinates.FromArray2DIndex(new Vector2Int(i, j));
      var index = (root + offset).ToArray2DIndex();

      if (index.x < 0 || index.x >= FWidth || index.y < 0 || index.y >= FHeight)
        return NullHex;

      return Field[index.x][index.y];
    }

    private void AddSolidObject(Hex hex, int type)
    {
      if (hex == null || !hex.Active || hex.ObjectInside == type)
        return;

      if (_solidObjects.Contains(hex))
        CleanOutHex(hex);

      hex.SetObjectInside(type);
      _solidObjects.Add(hex);
    }

    private void CleanOutHex(Hex hex)
    {
      hex.SetObjectInside(0);
      _solidObjects.Remove(hex);
    }
  }
}