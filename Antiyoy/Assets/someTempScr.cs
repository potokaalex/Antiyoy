using System.Collections;
using System.Collections.Generic;
using ClientCode.Utilities;
using ClientCode.Utilities.Extensions;
using UnityEngine;
using UnityEngine.Tilemaps;

public class someTempScr : MonoBehaviour
{
    public Tilemap Tilemap;
    public TileBase Tile;
    
    // Start is called before the first frame update
    void Start()
    {
        foreach (var d in HexDirectionsUtilities.GetNeighbors(new Vector2Int(0,0)))
        {
           Tilemap.SetTile((new Vector2Int(0,0) + d).ToVector3Int(), Tile); 
        }
        
        
        foreach (var d in HexDirectionsUtilities.GetNeighbors(new Vector2Int(9,9)))
        {
            Tilemap.SetTile((new Vector2Int(9,9) + d).ToVector3Int(), Tile); 
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
