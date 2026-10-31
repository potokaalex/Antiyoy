using Client.Project.UI;
using Client.Project.Utilities;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Client.Menu.MainMenu.SaveLoad
{
  public class SaveLoadOptionsView : MonoBehaviour
  {
    [SerializeField] private CustomButton _backButton;
    [SerializeField] private CustomButton _renameButton;
    [SerializeField] private CustomButton _deleteButton;
    [SerializeField] private TMP_InputField _inputField;
    [SerializeField] private TopAndBottomMenuAnimator _bodyMenuAnimator;
    [SerializeField] private MenuAnimator _inputFieldAnimator;
    [SerializeField] private Image _fade;
    private readonly float _fadeAnimationSpeedFactor = 1.5f;
    private SaveLoadView _saveLoadView;
    private SaveLoadSlotView _slotView;

    public void Initialize(SaveLoadView saveLoadView)
    {
      _saveLoadView = saveLoadView;
      _backButton.OnClick += OnBackClick;
      _renameButton.OnClick += OnRenameClick;
      _deleteButton.OnClick += OnDeleteClick;
      _inputField.onEndEdit.AddListener(OnEndRename);
    }

    private void OnDestroy()
    {
      _backButton.OnClick -= OnBackClick;
      _renameButton.OnClick -= OnRenameClick;
      _deleteButton.OnClick -= OnDeleteClick;
      _inputField.onEndEdit.RemoveListener(OnEndRename);
      DOTween.Kill(this);
    }

    public void Show(SaveLoadSlotView slotView)
    {
      _slotView = slotView;
      gameObject.SetActive(true);
      _bodyMenuAnimator.PlayShow();
      _fade.SetAlpha(0);
      _fade.DOFade(0.2f, 0.5f / _fadeAnimationSpeedFactor).SetEase(AnimationsUtilities.MenuDefaultEase).SetId(this);
    }

    private void OnBackClick() => Hide();

    private void Hide()
    {
      _bodyMenuAnimator.PlayHide();
      _fade.DOFade(0, 0.5f / _fadeAnimationSpeedFactor).SetEase(AnimationsUtilities.MenuDefaultEase).OnComplete(() => gameObject.SetActive(false))
        .SetId(this);
      _inputFieldAnimator.PlayHide();
    }

    private void OnRenameClick()
    {
      _inputFieldAnimator.PlayShow();
      _bodyMenuAnimator.PlayHide();
      _inputField.text = _slotView.Header;
    }

    private void OnDeleteClick()
    {
      Hide();
      _saveLoadView.ShowDeleteView(_slotView);
    }

    private void OnEndRename(string value)
    {
      _saveLoadView.Rename(_slotView, value);
      Hide();
    }
  }
}