using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace ClientCode.Gameplay.Cell
{
    public class CellObject1 : MonoBehaviour
    {
        public SpriteRenderer SpriteRenderer;
        public TextMeshPro DebugText;
        public int Entity;
        
        public List<int> NeighbourCellEntities;
    }
}