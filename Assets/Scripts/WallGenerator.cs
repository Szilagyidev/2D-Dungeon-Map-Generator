using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WallGenerator
{
    public static void CreateWalls(HashSet<Vector2Int> floorPos, TileMapVisualizer tilemapVisualizer)
    {

        var basicWallPos = FindWallsInDirections(floorPos, Direction2D.cardinalDirectionList);
        var cornerWallPos = FindWallsInDirections(floorPos, Direction2D.diagonalDirectionList);

        CreateBasicWall(tilemapVisualizer, basicWallPos, floorPos);
        CreateCornerWall(tilemapVisualizer, cornerWallPos, floorPos);
    }

    private static void CreateCornerWall(TileMapVisualizer tilemapVisualizer, HashSet<Vector2Int> cornerWallPos, HashSet<Vector2Int> floorPos){
        foreach (var position in cornerWallPos)
        {
            string neighboursBinaryValue = "";
            foreach (var direction in Direction2D.eightDirectionList)
            {
                var neighbourPosition = position + direction;
                if(floorPos.Contains(neighbourPosition)){
                    neighboursBinaryValue += "1";
                }else{
                    neighboursBinaryValue += "0";
                }
            }
            tilemapVisualizer.PaintSingleCornerWall(position, neighboursBinaryValue);
        }
    }

    private static void CreateBasicWall(TileMapVisualizer tilemapVisualizer, HashSet<Vector2Int> basicWallPos, HashSet<Vector2Int> floorPos)
    {
        foreach (var position in basicWallPos)
        {
            string neighboursBinaryValue = "";
            foreach (var direction in Direction2D.cardinalDirectionList)
            {
                var neighbourPosition = position + direction;
                if(floorPos.Contains(neighbourPosition)){
                    neighboursBinaryValue += "1";
                } else {
                    neighboursBinaryValue += "0";
                }
            }
            tilemapVisualizer.PaintSingeBasicWall(position, neighboursBinaryValue);
        }
    }

    private static HashSet<Vector2Int> FindWallsInDirections(HashSet<Vector2Int> floorPos, List<Vector2Int> directionList){
        HashSet<Vector2Int> wallPos = new HashSet<Vector2Int>();
        foreach (var position in floorPos)
        {
            foreach (var direction in directionList)
            {
                var neighbourPos = position + direction;
                if(floorPos.Contains(neighbourPos) == false)
                    wallPos.Add(neighbourPos);
            }
        }
        return wallPos;
    }
}
