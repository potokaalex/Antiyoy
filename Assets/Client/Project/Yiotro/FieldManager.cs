using System.Collections.Generic;
using Client.Project.Hex;
using Client.Project.Utilities;
using UnityEngine;

namespace Client.Project.Yiotro
{
  public class FieldManager
  {
    public readonly List<Hex> ActiveHexes = new();
    private readonly List<Hex> _solidObjects = new();
    public readonly Hex NullHex;
    public readonly Hex[][] Field;
    public int FWidth;
    public int FHeight;

    public FieldManager()
    {
      var maxFieldSize = GameUtilities.MaxFieldSize;
      Field = new Hex[maxFieldSize.x][];
      for (var x = 0; x < maxFieldSize.x; x++)
      {
        Field[x] = new Hex[maxFieldSize.y];
        for (var y = 0; y < maxFieldSize.y; y++)
          Field[x][y] = new Hex(x, y, this);
      }

      NullHex = new Hex(-1, -1, this);
      NullHex.Active = false;
    }

    public void Setup(Vector2Int size)
    {
      FWidth = size.x;
      FHeight = size.y;
    }

    public void SpawnTree(Hex hex)
    {
      if (!hex.Active)
        return;
      AddSolidObject(hex, hex.IsNearWater() ? YiotroUtilities.OBJ_PALM : YiotroUtilities.OBJ_PINE);
    }

    public void CreateFieldMatrix()
    {
      ActiveHexes.Clear();
      _solidObjects.Clear();

      for (var i = 0; i < FWidth; i++)
      for (var j = 0; j < FHeight; j++)
        Field[i][j].Setup();
    }

    public Hex AdjacentHex(Hex hex, int direction)
    {
      if (direction < 0 || direction >= HexUtilities.Directions.Length)
        return NullHex;

      var offset = HexUtilities.Directions[direction];
      var index = HexCoordinates.AddOffsetToArray2Index(hex.Index1, hex.Index2, offset);

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