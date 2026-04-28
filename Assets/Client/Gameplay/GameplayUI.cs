using Client.Infrastructure;
using UnityEngine;
using UnityEngine.UI;

namespace Client.Gameplay
{
  public class GameplayUI : MonoBehaviour
  {
    [SerializeField] private GameObject _regionPanel;
    [SerializeField] private Button _createUnitButton;
    private GameplayController _gameplayController;

    private void Awake()
    {
      _gameplayController = Locator.Get<GameplayController>();
      _createUnitButton.onClick.AddListener(_gameplayController.SetCreateUnitMode);
    }

    private void OnDestroy()
    {
      _createUnitButton.onClick.RemoveListener(_gameplayController.SetCreateUnitMode);
    }

    public void ActiveRegionUI(bool isActive)
    {
      _regionPanel.SetActive(isActive);
    }
  }
}