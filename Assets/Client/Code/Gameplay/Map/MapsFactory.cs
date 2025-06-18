namespace ClientCode.UI.Windows.Writing
{
    public class MapsFactory
    {
        private readonly MapsContainer _container;
        private static int _lastCreatedNumber;

        public MapsFactory(MapsContainer container) => _container = container;

        public void Create()
        {
            var map = new MapController { Name = $"Created-{_lastCreatedNumber}" };
            _lastCreatedNumber++;
            _container.Add(map);
        }

        public void Destroy(MapController map) => _container.Remove(map);
    }
}