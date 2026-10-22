using Client.Menu;
using Client.Menu.Intro;
using Client.Menu.MainMenu;
using Client.Project.Borders;
using Client.Project.CameraFeatures;
using Client.Project.Infrastructure;
using Client.Project.Region;
using Client.Project.Tile;
using Client.Project.Unit.Code;
using UnityEngine;

namespace Client.Project.Project
{
  public class ProjectInstaller : MonoInstaller, ICoroutineRunner
  {
    [SerializeField] private IntroView _introView;
    [SerializeField] private MainMenuView _mainMenuView;
    [SerializeField] private MenuView _menuView;
    [SerializeField] private CameraController _cameraController;
    [SerializeField] private TilemapController _tilemapController;
    [SerializeField] private GridController _gridController;
    [SerializeField] private BordersService _bordersService;
    [SerializeField] private RegionsService _regionsService;
    [SerializeField] private UnitsService _unitsService;

    public override void Install(Context context)
    {
      context.Register(this, typeof(ICoroutineRunner));
      context.Register(new InputController());
      context.Register(_cameraController);
      context.Register(_tilemapController);
      context.Register(_gridController);
      context.Register(_bordersService);
      context.Register(new RegionsFactory());
      context.Register(_regionsService);
      context.Register(new UnitsAreaCalculator());
      context.Register(_unitsService);
      context.Register(_introView);
      context.Register(_menuView);
      context.Register(_mainMenuView);
      context.Register(new ProjectController());
    }
  }
}