using System.Collections.Generic;
using Client.Infrastructure;
using UnityEngine;
using UnityEngine.Pool;

namespace Client.TilesSelection
{
  public class TilesSelectionView : MonoBehaviour
  {
    [SerializeField] private GameObject _tileMaskPrefab;
    [SerializeField] private GameObject _hidePanel;
    private readonly List<GameObject> _activeMasks = new();
    private GridController _gridController;
    private ObjectPool<GameObject> _pool;

    private void Awake()
    {
      _gridController = Locator.Get<GridController>();
      _pool = new ObjectPool<GameObject>(() => Instantiate(_tileMaskPrefab, transform), x => x.SetActive(true), x => x.SetActive(false));
    }

    public void ViewTiles(List<CellController> cells)
    {
      foreach (var cell in cells)
      {
        var mask = _pool.Get();
        mask.transform.position = _gridController.HexPositionToWorld(cell.Position);
        _activeMasks.Add(mask);
      }

      _hidePanel.SetActive(true);
    }

    public void ClearView()
    {
      _hidePanel.SetActive(false);
      foreach (var mask in _activeMasks)
        _pool.Release(mask);
      _activeMasks.Clear();
    }
  }
}