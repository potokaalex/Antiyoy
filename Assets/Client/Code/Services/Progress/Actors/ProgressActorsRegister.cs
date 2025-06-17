using System;
using System.Collections.Generic;
using Zenject;

namespace Client.Code.Services.Progress.Actors
{
    public class ProgressActorsRegister : IInitializable, IDisposable
    {
        private readonly List<IProgressActor> _actors;
        private readonly ProgressController _controller;

        public ProgressActorsRegister(List<IProgressActor> actors, ProgressController controller)
        {
            _actors = actors;
            _controller = controller;
        }

        public void Initialize()
        {
            foreach (var actor in _actors)
                _controller.RegisterActor(actor);
        }

        public void Dispose()
        {
            foreach (var actor in _actors)
                _controller.UnRegisterActor(actor);
        }
    }
}