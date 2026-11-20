using System.Collections.Generic;
using Client.Project;
using Client.Project.Borders;
using Client.Project.Infrastructure;
using Client.Project.Tile;
using DG.Tweening;
using UnityEngine;

namespace Client.Gameplay
{
  public class TilesSelectionView : MonoBehaviour
  {
    [SerializeField] private SpriteRenderer _hidePanel;
    private BordersService _bordersService;
    private TilemapController _tilemapController;

    private void Awake()
    {
      _bordersService = Locator.Get<BordersService>();
      _tilemapController = Locator.Get<TilemapController>();
    }

    public void ViewTiles(List<CellController> cells)
    {
      _tilemapController.TilemapMaskCreateTiles(cells);
      DOTween.Kill(this);
      _hidePanel.enabled = true;
      _hidePanel.DOFade(0.4f, 0.2f).SetEase(Ease.OutQuad);
      _bordersService.ViewTilesSelectionBorders(cells);
    }

    public void ClearView()
    {
      _hidePanel.DOFade(0, 0.2f).SetEase(Ease.OutQuad).SetId(this).onComplete += () => _hidePanel.enabled = false;
      _tilemapController.TilemapMaskClearTiles();
      _bordersService.ClearTilesSelectionBorders();
    }
  }
}