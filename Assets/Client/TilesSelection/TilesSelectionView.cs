using System.Collections.Generic;
using Client.Borders;
using Client.Infrastructure;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Pool;

namespace Client.TilesSelection
{
  public class TilesSelectionView : MonoBehaviour
  {
    [SerializeField] private GameObject _tileMaskPrefab;
    [SerializeField] private SpriteRenderer _hidePanel;
    private readonly List<GameObject> _activeMasks = new();
    private GridController _gridController;
    private BordersService _bordersService;
    private ObjectPool<GameObject> _pool;

    private void Awake()
    {
      _gridController = Locator.Get<GridController>();
      _bordersService = Locator.Get<BordersService>();
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

      DOTween.Kill(this);
      _hidePanel.enabled = true;
      _hidePanel.DOFade(0.5f, 0.2f).SetEase(Ease.OutQuad);
      _bordersService.ViewTilesSelectionBorders(cells);
    }

    public void ClearView()
    {
      _hidePanel.DOFade(0, 0.2f).SetEase(Ease.OutQuad).SetId(this).onComplete += () => _hidePanel.enabled = false;
      foreach (var mask in _activeMasks)
        _pool.Release(mask);
      _activeMasks.Clear();
      _bordersService.ClearTilesSelectionBorders();
    }
  }
}