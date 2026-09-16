using Client.Infrastructure;
using Client.Menu.MainMenu;
using Client.Utilities;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Client.Menu.Intro
{
  public class IntroView : MonoBehaviour
  {
    [SerializeField] private RectTransform _textRoot;
    [SerializeField] private Image _fade;
    private MainMenuView _mainMenuView;

    public void Play()
    {
      DOTween.Sequence()
        .Append(AnimationsUtilities.DoAnchoredMove(_textRoot, new Vector2(0, -50), Vector2.zero))
        .Join(_fade.DOFade(0, 0.3f))
        .AppendInterval(0.65f)
        .AppendCallback(() => gameObject.SetActive(false))
        .Append(_mainMenuView.PlayAppearAnimation());
    }

    private void Awake() => _mainMenuView = Locator.Get<MainMenuView>();
  }
}