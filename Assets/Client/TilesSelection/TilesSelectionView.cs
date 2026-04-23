using System.Collections.Generic;
using Client.Hex;
using Client.Infrastructure;
using UnityEngine;
using UnityEngine.Pool;

namespace Client.TilesSelection
{
  public class TilesSelectionView : MonoBehaviour
  {
    [SerializeField] private GameObject _tileMaskPrefab;
    private readonly List<GameObject> _activeMasks = new();
    private GridController _gridController;
    private ObjectPool<GameObject> _pool;

    private void Awake()
    {
      _gridController = Locator.Get<GridController>();
      _pool = new ObjectPool<GameObject>(() => Instantiate(_tileMaskPrefab, transform, true), x => x.SetActive(true), x => x.SetActive(false));
    }

    public void ViewTiles(List<HexCoordinates> positions)
    {
      ClearView();
      gameObject.SetActive(true);

      foreach (var position in positions)
      {
        var mask = _pool.Get();
        mask.transform.position = _gridController.HexPositionToWorld(position);
        _activeMasks.Add(mask);
      }
    }

    public void ClearView()
    {
      gameObject.SetActive(false);
      foreach (var mask in _activeMasks)
        _pool.Release(mask);
      _activeMasks.Clear();
    }
  }
}