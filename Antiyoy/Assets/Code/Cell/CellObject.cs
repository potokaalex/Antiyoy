using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Code.Cell
{
    public class CellObject : MonoBehaviour
    {
        public SpriteRenderer SpriteRenderer;
        public TextMeshProUGUI DebugText;
        public int Entity;
        public List<Vector2Int> Neighbours;
    }
}
