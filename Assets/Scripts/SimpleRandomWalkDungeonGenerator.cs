using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Random = UnityEngine.Random;

public class SimpleRandomWalkDungeonGenerator : AbstractDungeonGenerator
{

    [SerializeField]
    protected SimpleRandomWalkData randomWalkData;
    protected override void RunProceduralGeneration(){

        HashSet<Vector2Int> floorPos = RunRandomWalk(randomWalkData, startPos);
        tileMapVisualizer.Clear();
        tileMapVisualizer.PaintFloorTiles(floorPos);
        WallGenerator.CreateWalls(floorPos, tileMapVisualizer);
    }

    protected HashSet<Vector2Int> RunRandomWalk(SimpleRandomWalkData randomWalkData, Vector2Int pos){

        var currentPos = pos;
        HashSet<Vector2Int> floorPos = new HashSet<Vector2Int>();

        for (int i = 0; i < randomWalkData.iterations; i++)
        {
            var path = PorceduralGenerationAlg.SimpleRandomWalk(currentPos,randomWalkData.walkLenght);
            floorPos.UnionWith(path);
            if(randomWalkData.startRandomlyEachIteration)
                currentPos = floorPos.ElementAt(Random.Range(0, floorPos.Count));
        }
        return floorPos;
    }


}
