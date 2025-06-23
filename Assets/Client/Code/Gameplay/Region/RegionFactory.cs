using System.ComponentModel;
using Client.Code.Gameplay;
using ClientCode.Gameplay.Region;
using ClientCode.Gameplay.Region.Components;
using Zenject;

namespace ClientCode.Infrastructure.Installers
{
    public class RegionFactory : IInitializable
    {
        private readonly EcsController _ecsController;

        public RegionFactory(EcsController ecsController) => _ecsController = ecsController;

        public void Initialize()
        {
            //регион содержит множество клеток, т.е. компонент региона должен давать информацию о клетках.
            //хорошо, регион должен храниться в контейнере, а в клетках только ссылка не регион, правильно? - да.
            //_ecsController.World.GetPool<RegionLink>();
        }

        public void Create(int entity, RegionType regionType)
        {
            //тут должна происходить магия.
        }

        public void Destroy(int entity)
        {
            //тут должна происходить магия.
        }
    }
}