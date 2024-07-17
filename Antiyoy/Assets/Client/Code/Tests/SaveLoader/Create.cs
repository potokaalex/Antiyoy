using ClientCode.Gameplay.Cell;
using ClientCode.Services.StaticDataProvider;
using NSubstitute;
using UnityEngine;

public static class Create
{
    public static IStaticDataProvider StaticDataProvider()
    {
        var staticDataProvider = Substitute.For<IStaticDataProvider>();
        staticDataProvider.Prefabs.CellObject = new GameObject().AddComponent<CellObject>();
        return staticDataProvider;
    }
}