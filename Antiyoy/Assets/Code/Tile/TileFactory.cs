using Code.Cell;
using UnityEngine;

namespace Code.Tile
{
    //нажимая на клетку, я создаю тайл, тайл имеет регион и сущность.
    public class TileFactory
    {
        public void Create(CellObject cell)
        {
            cell.SpriteRenderer.color = Color.white;
        }
        
        public void Destroy(CellObject cell)
        {
            cell.SpriteRenderer.color = Color.black;
        }
    }
}