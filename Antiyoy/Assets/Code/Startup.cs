using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace Code
{
    public class Startup : MonoBehaviour
    {
        [SerializeField] private CellObject _cellPrefab;
        [SerializeField] private CameraObject _camera;
        
        private void Awake()
        {
            CreateCells();
        }

        private void Update()
        {
            TouchCell();
        }

        private void TouchCell()
        {
            if (!Input.GetMouseButtonDown(0)) 
                return;
            
            var ray = _camera.GetRayFromCurrentMousePosition();
            var hit = Physics2D.Raycast(ray.origin, ray.direction);
            
            if (hit.transform)
            {
                if(hit.transform.TryGetComponent<CellObject>(out var cell))
                    Debug.Log(cell.name);
            }
        }

        private void CreateCells()
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
