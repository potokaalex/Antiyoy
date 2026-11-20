using Client.Project;
using Client.Project.Infrastructure;
using Client.Project.UI;
using UnityEngine;

namespace Client.Menu.MainMenu.SaveLoad
{
  public class SaveLoadDeleteView : MonoBehaviour
  {
    [SerializeField] private CustomButton _backButton;
    [SerializeField] private CustomButton _deleteButton;
    [SerializeField] private MenuAnimator _menuAnimator;
    private SaveLoadView _saveLoadView;
    private SaveLoadSlotView _slotView;
    private InputController _inputController;

    public bool IsShowing => _menuAnimator.IsShowing;

    public void Initialize(SaveLoadView saveLoadView)
    {
      _inputController = Locator.Get<InputController>();
      _saveLoadView = saveLoadView;
      _backButton.OnClick += OnBackClick;
      _deleteButton.OnClick += OnDeleteClick;
    }

    private void Update()
    {
      if (IsShowing && _inputController.BackClicked)
        _backButton.Click();
    }

    public void Show(SaveLoadSlotView slotView)
    {
      _slotView = slotView;
      _menuAnimator.PlayShow();
    }

    private void OnBackClick() => Hide();

    private void OnDeleteClick()
    {
      _saveLoadView.DeleteSlot(_slotView);
      Hide();
    }

    private void Hide()
    {
      _menuAnimator.PlayHide();
      _saveLoadView.Show();
    }
  }
}