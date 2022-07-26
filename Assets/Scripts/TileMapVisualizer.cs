using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;

public class TileMapVisualizer : MonoBehaviour
{

  [SerializeField]
  private Tilemap floorTilemap, wallTilemap;
  [SerializeField]
  private TileBase floorTile, wallTop, wallSideRight, wallSideLeft, wallSideBottom, wallFull,
   wallInnerCornerDownLeft, wallInnerCornerDownRight,
    wallDiagonalCornerDownRight, wallDiagonalCornerDownLeft, wallDiagonalCornerUpLeft, wallDiagonalCornerUpRight;
  public void PaintFloorTiles(IEnumerable<Vector2Int> floorPos){
    PaintTiles(floorPos, floorTilemap, floorTile);  
  }

  private void PaintTiles(IEnumerable<Vector2Int> pos, Tilemap tilemap, TileBase tile){
    foreach (var position in pos)
    {
        PaintSingleTile(tilemap, tile, position);
    }
  }

  internal void PaintSingeBasicWall(Vector2Int position, string binaryType){
    int typeAsInt = Convert.ToInt32(binaryType, 2);
    TileBase tile = null;

    if(WallTypeData.wallTop.Contains(typeAsInt)){
      tile = wallTop;
    }else if(WallTypeData.wallSideRight.Contains(typeAsInt)){
      tile = wallSideRight;
    }
    else if(WallTypeData.wallSideLeft.Contains(typeAsInt)){
      tile = wallSideLeft;
    }
    else if(WallTypeData.wallBottm.Contains(typeAsInt)){
      tile = wallSideBottom;
    }else if(WallTypeData.wallFull.Contains(typeAsInt)){
      tile = wallFull;
    }
    if(tile!=null){
      PaintSingleTile(wallTilemap, tile, position);
    }
  }

  internal void PaintSingleCornerWall(Vector2Int position, string binaryType){
    int typeAsInt = Convert.ToInt32(binaryType, 2);
    TileBase tile = null;

    if(WallTypeData.wallInnerCornerDownLeft.Contains(typeAsInt)){
      tile = wallInnerCornerDownLeft;
    } else if(WallTypeData.wallInnerCornerDownRight.Contains(typeAsInt)){
      tile = wallInnerCornerDownRight;
    }else if(WallTypeData.wallDiagonalCornerDownLeft.Contains(typeAsInt)){
      tile = wallDiagonalCornerDownLeft;
    }else if(WallTypeData.wallDiagonalCornerDownRight.Contains(typeAsInt)){
      tile = wallDiagonalCornerDownRight;
    }else if(WallTypeData.wallDiagonalCornerUpLeft.Contains(typeAsInt)){
      tile = wallDiagonalCornerUpLeft;
    }else if(WallTypeData.wallDiagonalCornerUpRight.Contains(typeAsInt)){
      tile = wallDiagonalCornerUpRight;
    }else if(WallTypeData.wallFullEightDirections.Contains(typeAsInt)){
      tile = wallFull;
    }else if(WallTypeData.wallBottmEightDirections.Contains(typeAsInt)){
      tile = wallSideBottom;
    }
    if(tile != null){
      PaintSingleTile(wallTilemap, tile, position);
    }
  }
  
  private void PaintSingleTile(Tilemap tilemap, TileBase tile, Vector2Int pos){
    var tilePos = tilemap.WorldToCell((Vector3Int)pos);
    tilemap.SetTile(tilePos, tile);
  }

  public void Clear(){
    floorTilemap.ClearAllTiles();
    wallTilemap.ClearAllTiles();
  }

}
