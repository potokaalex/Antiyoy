using System;
using System.Collections.Generic;
using ClientCode.Services.SaveLoader.Progress.Actors;
using Zenject;

namespace ClientCode.Services.SaveLoader.Progress
{
    public class ProgressRegister : IInitializable, IDisposable
    {
        private readonly IProgressDataSaveLoader _saveLoader;
        private readonly List<IProgressActor> _readers;

        public ProgressRegister(IProgressDataSaveLoader saveLoader, List<IProgressActor> readers)
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