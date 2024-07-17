using System;
using System.Collections.Generic;
using ClientCode.Services.Progress.Base;
using Zenject;

namespace ClientCode.Services.Progress.Actors
{
    public class ProgressActorsRegister<T> : IInitializable, IDisposable where T : IProgressData
    {
        private readonly IProgressSaveLoader<T> _saveLoader;
        private readonly List<IProgressActor> _readers;

        public ProgressActorsRegister(IProgressSaveLoader<T> saveLoader, List<IProgressActor> readers)
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