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

    public void Initialize(SaveLoadView saveLoadView)
    {
      _saveLoadView = saveLoadView;
      _backButton.OnClick += OnBackClick;
      _deleteButton.OnClick += OnDeleteClick;
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