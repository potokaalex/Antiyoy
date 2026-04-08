using UnityEngine;

namespace Client.New.Gameplay
{
  public class GameplayInstaller : MonoBehaviour
  {
    [SerializeField] private GridController _grid;
    [SerializeField] private GameplayTest _gameplayTest;
    [SerializeField] private CameraController _camera;
    private CellController[] _cells;
    private Vector2Int _mapSize;

    private void Awake()
    {
      var height = 10;
      var width = 10;
      _mapSize = new Vector2Int(width, height);
      _grid.Initialize(_mapSize);
      _gameplayTest.Initialize(_camera, _grid);
    }
  }
}