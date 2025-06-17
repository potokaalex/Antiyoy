using System;
using System.Collections.Generic;
using ClientCode.Services.Progress.Base;
using Zenject;

namespace ClientCode.Services.Progress.Actors
{
    public class ProgressActorsRegister : IInitializable, IDisposable
    {
        private readonly List<IProgressSaveLoader> _saveLoaders;
        private readonly List<IProgressActor> _readers;

        public ProgressActorsRegister(List<IProgressSaveLoader> saveLoaders, List<IProgressActor> readers)
        {
            _saveLoaders = saveLoaders;
            _readers = readers;
        }

        public void Initialize()
        {
            foreach (var saveLoader in _saveLoaders)
            foreach (var reader in _readers)
                saveLoader.RegisterActor(reader);
        }

        public void Dispose()
        {
            foreach (var saveLoader in _saveLoaders)
            foreach (var reader in _readers)
                saveLoader.UnRegisterActor(reader);
        }
    }
}