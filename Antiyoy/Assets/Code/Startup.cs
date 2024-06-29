using UnityEngine;

namespace Code
{
    public class Startup : MonoBehaviour
    {
        [SerializeField] private GameObject _cellPrefab;
        //side = 1
        //big radius = side
        //small radius = size*sqrt(3)/2
        private void Awake()
        {
            var sideLength = 1f / 2;
            var bigRadius = sideLength;
            var smallRadius = sideLength * Mathf.Sqrt(3) / 2;
            
            for (var i = 0; i < 10; i++)
            {
                var x = i * bigRadius * 3/2; //bigRadius + bigRadius / 2
                var y = 0f;

                if (i % 2 != 0)
                    y = smallRadius;
                
                Instantiate(_cellPrefab, new Vector3(x, y), Quaternion.identity);
            }
            
        }
    }
}
