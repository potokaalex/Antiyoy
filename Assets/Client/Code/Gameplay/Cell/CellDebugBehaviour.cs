using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace ClientCode.Gameplay.Cell
{
    public class CellDebugBehaviour : MonoBehaviour
    {
        public TextMeshPro Text;
        [ReadOnly] public List<int> Neighbours;
    }
}