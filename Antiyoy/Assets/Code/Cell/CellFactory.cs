namespace Code.Cell
{
    public class CellFactory
    {
        private readonly EcsProvider _ecsProvider;
        private readonly ConfigProvider _configProvider;

        public CellFactory(EcsProvider ecsProvider, ConfigProvider configProvider)
        {
            _ecsProvider = ecsProvider;
            _configProvider = configProvider;
        }

        public void Create()
        {
            var eventsBus = _ecsProvider.GetEventsBus();
            var mapWidth = _configProvider.GetMap().Width;
            
            for (var i = 0; i < 10; i++)
            for (var j = 0; j < 10; j++)
            {
                ref var createRequest = ref eventsBus.NewEvent<CellCreateRequest>();
                createRequest.Index = i * mapWidth + j;
            }
        }
    }
}