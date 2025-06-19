using System.Collections.Generic;
using ClientCode.Gameplay;
using ClientCode.Gameplay.Cell;
using ClientCode.Services.CanvasService;
using ClientCode.UI.Buttons.Base;
using ClientCode.UI.Windows.Base;
using UnityEngine.Tilemaps;

namespace ClientCode.Data.Static
{
    public class Prefabs
    {
        public Dictionary<ButtonType, ButtonBaseOld> Buttons;
        public Dictionary<WindowType, WindowBaseOld> Windows;
        public ProjectCanvasObject ProjectCanvasObject;
        public GridObject GridObject;
        public TileBase EmptyTile;
        public TileBase Tile;
        public CellDebugBehaviour CellDebug;
    }
}