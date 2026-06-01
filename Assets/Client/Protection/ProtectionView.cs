using System.Collections;
using System.Collections.Generic;
using Client.Infrastructure;
using Client.Region;
using UnityEngine;
using UnityEngine.Pool;

namespace Client.Protection
{
  public class ProtectionView : MonoBehaviour
  {
    [SerializeField] private GameObject _icon;
    private readonly List<GameObject> _icons = new();
    private GridController _gridController;
    private ObjectPool<GameObject> _pool;

    private void Awake()
    {
      _gridController = Locator.Get<GridController>();
      _pool = new ObjectPool<GameObject>(() => Instantiate(_icon, transform), x => x.SetActive(true), x => x.SetActive(false));
    }

    public void ViewBuildingsProtection(RegionController region)
    {
      StopAllCoroutines();
      StartCoroutine(ViewCoroutine(region));
    }


    private IEnumerator ViewCoroutine(RegionController region)
    {
      using (ListPool<CellController>.Get(out var protectionArea))
      {
        ClearView();

        foreach (var cell in region.Cells)
        {
          if (cell.HasUnit && cell.Unit.CanViewProtection)
          {
            cell.Unit.GetProtectionArea(protectionArea);
            foreach (var areaCell in protectionArea)
            {
              if (areaCell != cell)
              {
                var icon = _pool.Get();
                icon.transform.position = _gridController.HexPositionToWorld(areaCell.Position);
                _icons.Add(icon);
              }
            }
          }
        }

        yield return new WaitForSeconds(1);
        ClearView();
      }
    }

    private void ClearView()
    {
      foreach (var icon in _icons)
        _pool.Release(icon);
      _icons.Clear();
    }
  }
}