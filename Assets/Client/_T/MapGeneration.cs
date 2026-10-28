using System;
using Antiyoy.Gameplay;

namespace Client._T
{
  public class MapGeneration
  {
    public void generateMap()
    {
      //from runtime config
      GameRules.fractionsQuantity = 2;
      var fWidth = 20;
      var fHeight = 20;

      var gameController = new GameController(fWidth, fHeight);
      var mapGeneratorGeneric = new MapGeneratorGeneric(gameController);
      mapGeneratorGeneric.generateMap(new Random(), gameController.fieldManager.field);
      //detectProvinces();
    }
  }
}