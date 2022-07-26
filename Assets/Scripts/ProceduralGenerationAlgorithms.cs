using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

public static class PorceduralGenerationAlg
{
 
 public static HashSet<Vector2Int> SimpleRandomWalk(Vector2Int startPos, int walkLength){

    HashSet<Vector2Int> path = new HashSet<Vector2Int>();

    path.Add(startPos);
    var previousPos = startPos;

    for (int i = 0; i < walkLength; i++)
    {
        var newPos = previousPos + Direction2D.GetRandomCardinalDirection();
        path.Add(newPos);
        previousPos = newPos;
    }

    return path;
 }

 public static List<Vector2Int> RandomWalkCorridor(Vector2Int startPos, int corridorLength){

    List<Vector2Int> corridor = new List<Vector2Int>();
    var direction = Direction2D.GetRandomCardinalDirection();
    var currentPos = startPos;
    corridor.Add(currentPos);

    for (int i = 0; i < corridorLength; i++)
    {
        currentPos += direction;
        corridor.Add(currentPos);
    }
    
    return corridor;
 }

 public static List<BoundsInt> BinarySpacePartitioning(BoundsInt spaceToSplit, int minWidth, int minHeigth){
    Queue<BoundsInt> roomsQue = new Queue<BoundsInt>();
    List<BoundsInt> roomsList = new List<BoundsInt>(); //FIFO lista
    roomsQue.Enqueue(spaceToSplit);
    while (roomsQue.Count > 0)
    {
        var room = roomsQue.Dequeue();
        if(room.size.y >= minHeigth && room.size.x >= minWidth){

            if(UnityEngine.Random.value < 0.5f){
                if(room.size.y >= minHeigth*2){
                    SplitHorizantlly(minWidth,roomsQue,room);
                } else if(room.size.x >= minWidth*2){
                    SplitVertically(minHeigth,roomsQue,room);
                } else if(room.size.x >= minWidth && room.size.y >= minHeigth){ // már csekkoltam csak egy else is elég lenne
                    roomsList.Add(room);
                }
            }else{
                 if(room.size.x >= minWidth*2){
                    SplitVertically(minHeigth,roomsQue,room);
                }else if(room.size.y >= minHeigth*2){
                    SplitHorizantlly(minWidth,roomsQue,room);
                }
                 else if(room.size.x >= minWidth && room.size.y >= minHeigth){ // már csekkoltam csak egy else is elég lenne
                    roomsList.Add(room);
                }

            }
        }
    }
    return roomsList;
 }

 private static void SplitHorizantlly(int minWidth, Queue<BoundsInt> roomsQue, BoundsInt room){
    var ySplit = Random.Range(1, room.size.y); // (minHeight, room.size.y - minHeight) ha rendezetteb nem random szobákat akarok
    BoundsInt room1 = new BoundsInt(room.min, new Vector3Int(room.size.x, ySplit, room.size.z));
    BoundsInt room2 = new BoundsInt(new Vector3Int(room.min.x, room.min.y + ySplit, room.min.z),
        new Vector3Int(room.size.x, room.size.y - ySplit, room.size.z));
    roomsQue.Enqueue(room1);
    roomsQue.Enqueue(room2);

 }
 private static void SplitVertically(int minHeigth, Queue<BoundsInt> roomsQue, BoundsInt room){
    var xSplit = Random.Range(1, room.size.x);
    BoundsInt room1 = new BoundsInt(room.min, new Vector3Int(xSplit, room.size.y, room.size.z));
    BoundsInt room2 = new BoundsInt(new Vector3Int(room.min.x + xSplit, room.min.y, room.min.z),
        new Vector3Int(room.size.x - xSplit, room.size.y, room.size.z));
    roomsQue.Enqueue(room1);
    roomsQue.Enqueue(room2);
 }

}

public static class Direction2D
{

public static List<Vector2Int> cardinalDirectionList = new List<Vector2Int>{
    new Vector2Int(0,1), //UP
    new Vector2Int(1,0), //RIGHT
    new Vector2Int(0,-1), //DOWN
    new Vector2Int(-1,0) //LEFT
};

public static List<Vector2Int> diagonalDirectionList = new List<Vector2Int>{
    new Vector2Int(1,1), //UP_RIGHT
    new Vector2Int(1,-1), //RIGHT_DOWN
    new Vector2Int(-1,-1), //DOWN_LEFT
    new Vector2Int(-1,1) //LEFT_UP
};

public static List<Vector2Int> eightDirectionList = new List<Vector2Int>{
    new Vector2Int(0,1), //UP
    new Vector2Int(1,1), //UP_RIGHT
    new Vector2Int(1,0), //RIGHT
    new Vector2Int(1,-1), //RIGHT_DOWN
    new Vector2Int(0,-1), //DOWN
    new Vector2Int(-1,-1), //DOWN_LEFT
    new Vector2Int(-1,0), //LEFT
    new Vector2Int(-1,1) //LEFT_UP
    // Oramutato iranyba megyünk ezért fontos a sorrend
};

public static Vector2Int GetRandomCardinalDirection(){
    return cardinalDirectionList[Random.Range(0, cardinalDirectionList.Count)];
}

}
