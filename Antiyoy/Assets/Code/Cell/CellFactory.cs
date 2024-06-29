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
            var mapConfig = _configProvider.GetMap();
            
            for (var i = 0; i < mapConfig.Width; i++)
            for (var j = 0; j < mapConfig.Height; j++)
            {
                ref var createRequest = ref eventsBus.NewEvent<CellCreateRequest>();
                createRequest.Index = i * mapConfig.Width + j;
            }
        }
    }
}