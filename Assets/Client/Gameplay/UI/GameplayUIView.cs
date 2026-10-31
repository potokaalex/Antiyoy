using Client.Gameplay.UI.Hud;
using Client.Gameplay.UI.IncomeWindow;
using Client.Gameplay.UI.MoneyWindow;
using Client.Project.Region;
using UnityEngine;

namespace Client.Gameplay.UI
{
  public class GameplayUIView : MonoBehaviour
  {
    [SerializeField] private HudView _hudView;
    [SerializeField] private GameTransitionView _gameTransitionView;
    [SerializeField] private PauseView _pauseView;
    [SerializeField] private IncomeWindowView _incomeWindowView;
    [SerializeField] private MoneyWindowView _moneyWindowView;
    [SerializeField] private WinView _winView;
    [SerializeField] private SaveView _saveView;

    public void PlayShow()
    {
      _hudView.PlayShow();
      _gameTransitionView.PlayToyGameTransition();
    }

    public void ShowRegionUI(RegionController region)
    {
      _hudView.Region.SetActive(true);
      _hudView.Region.ViewMoney(region.Money);
      _hudView.Region.ViewIncome(region.GetIncome());
    }

    public void HideRegionUI() => _hudView.Region.SetActive(false);

    public void ViewTurnsCount(int value) => _hudView.ViewTurnsCount(value);

    public void ShowEndScreen(RegionType winner)
    {
      _hudView.PlayHide();
      _winView.PlayShow(winner);
      _gameTransitionView.PlayOutGameTransition();
    }

    public void ClearRegionCreation() => _hudView.Region.Creation.Clear();

    public void ShowPause(bool fromGameplay)
    {
      if (fromGameplay)
      {
        _hudView.PlayHide();
        _gameTransitionView.PlayOutGameTransition();
      }

      _pauseView.PlayShow();
    }

    public void HidePause()
    {
      _pauseView.PlayHide();
      _hudView.PlayShow();
      _gameTransitionView.PlayToyGameTransition();
    }

    public void OpenIncomeWindow() => _incomeWindowView.Open();

    public void OpenMoneyWindow() => _moneyWindowView.Open();

    public void OpenSaveView() => _saveView.Show();
  }
}