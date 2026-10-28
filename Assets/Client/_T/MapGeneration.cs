using System;
using Antiyoy.Gameplay;
using UnityEngine;
using Random = System.Random;

namespace Client._T
{
  public class MapGeneration : MonoBehaviour
  {
    private void Awake()
    {
      //from runtime config
      GameRules.fractionsQuantity = 2;
      var fWidth = 20;
      var fHeight = 20;

      var gameController = new GameController(fWidth, fHeight);
      var mapGeneratorGeneric = new MapGeneratorGeneric(gameController);
      //detectProvinces();
      gameController.fieldManager.createFieldMatrix();
      mapGeneratorGeneric.generateMap(new Random(Environment.TickCount), gameController.fieldManager.field);
      
      var str = string.Empty;
      foreach (var hexes in gameController.fieldManager.field)
      {
        foreach (var hex in hexes) 
          str += hex.active ? "X" : " ";
        str += "\n";
      }
      
      foreach (var hexes in gameController.fieldManager.field)
      {
        foreach (var hex in hexes) 
          str += hex.fraction.ToString();
        str += "\n";
      }
      
      UnityEngine.Debug.Log(str);
    }
  }
}