using System;
using System.Collections.Generic;
using Zenject;

namespace ClientCode.Services.Progress.Actors
{
    public class ProgressActorsRegister : IInitializable, IDisposable
    {
        private readonly IProgressDataSaveLoader _saveLoader;
        private readonly List<IProgressActor> _readers;

        public ProgressActorsRegister(IProgressDataSaveLoader saveLoader, List<IProgressActor> readers)
        {
            _saveLoader = saveLoader;
            _readers = readers;
        }

        public void Initialize()
        {
            foreach (var reader in _readers)
                _saveLoader.RegisterActor(reader);
        }

        public void Dispose()
        {
            foreach (var reader in _readers)
                _saveLoader.UnRegisterActor(reader);
        }
    }
}