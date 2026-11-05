using System;
using Client.Menu;
using Client.Project.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Client.MapEditor.UI.AgreeView
{
  public class MapEditorAgreeView : MonoBehaviour
  {
    [SerializeField] private TopAndBottomMenuAnimator _topAndBottomMenuAnimator;
    [SerializeField] private TextMeshProUGUI _header;
    [SerializeField] private CustomButton _cancelButton;
    [SerializeField] private Image _cancelButtonImage;
    [SerializeField] private TextMeshProUGUI _cancelButtonText;
    [SerializeField] private CustomButton _agreeButton;
    [SerializeField] private Image _agreeButtonImage;
    [SerializeField] private TextMeshProUGUI _agreeButtonText;
    private Action _agreeAction;

    private void Awake()
    {
      _cancelButton.OnClick += Hide;
      _agreeButton.OnClick += OnAgreeButtonClick;
    }

    private void OnDestroy()
    {
      _cancelButton.OnClick -= Hide;
      _agreeButton.OnClick -= OnAgreeButtonClick;
    }

    public void Show(MapEditorAgreeViewParameters parameters)
    {
      _agreeAction = parameters.AgreeAction;
      _header.SetText(parameters.Header);
      _cancelButtonImage.color = parameters.CancelColor;
      _cancelButtonText.SetText(parameters.CancelText);
      _agreeButtonImage.color = parameters.AgreeColor;
      _agreeButtonText.SetText(parameters.AgreeText);
      _topAndBottomMenuAnimator.PlayShow();
    }

    private void Hide() => _topAndBottomMenuAnimator.PlayHide();

    private void OnAgreeButtonClick()
    {
      _agreeAction?.Invoke();
      Hide();
    }
  }
}