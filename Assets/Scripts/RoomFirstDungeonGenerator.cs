using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using System;

public class RoomFirstDungeonGenerator : SimpleRandomWalkDungeonGenerator
{
  [SerializeField]
  private int minRoomWidth = 4, minRoomHeigth = 4;
  [SerializeField]
  private int dungeonWidth = 20, dungeonHeight = 20;
  [SerializeField]
  [Range(0,10)]
  private int offset = 1;
  [SerializeField]
  private bool randomWalkRooms = false;

    protected override void RunProceduralGeneration()
    {
        CreateRooms();
    }

    private void CreateRooms(){
        var roomList = PorceduralGenerationAlg.BinarySpacePartitioning(new BoundsInt((Vector3Int)startPos,
         new Vector3Int(dungeonWidth,dungeonHeight,0)), minRoomWidth,minRoomHeigth);

         HashSet<Vector2Int> floor = new HashSet<Vector2Int>();

        if(randomWalkRooms){
            floor = CreateRoomsRandomly(roomList);
        } else{
            floor = CreateSimpleRooms(roomList);
        }

         List<Vector2Int> roomCenter = new List<Vector2Int>();
         foreach (var room in roomList)
         {
            roomCenter.Add((Vector2Int)Vector3Int.RoundToInt(room.center));
         }

         HashSet<Vector2Int> corridor = ConnectRooms(roomCenter);
         floor.UnionWith(corridor);

         tileMapVisualizer.PaintFloorTiles(floor);
         WallGenerator.CreateWalls(floor,tileMapVisualizer);
    }

    private HashSet<Vector2Int> CreateRoomsRandomly(List<BoundsInt> roomList){
        HashSet<Vector2Int> floor = new HashSet<Vector2Int>();
        for (int i = 0; i < roomList.Count; i++)
        {
            var roomBounds = roomList[i];
            var roomCenter = new Vector2Int(Mathf.RoundToInt(roomBounds.center.x), Mathf.RoundToInt(roomBounds.center.y));
            var roomFloor = RunRandomWalk(randomWalkData, roomCenter);
            foreach (var position in roomFloor)
            {
                if(position.x >= (roomBounds.xMin + offset) && position.x <= (roomBounds.xMax - offset) && position.y >= (roomBounds.yMin - offset) 
                && position.y <= (roomBounds.yMax - offset)){
                    floor.Add(position);
                }
            }
        }
        return floor;
    }

    private HashSet<Vector2Int> ConnectRooms(List<Vector2Int> roomCenter){
        HashSet<Vector2Int> corridors = new HashSet<Vector2Int>();
        var currentRoomCenter = roomCenter[Random.Range(0,roomCenter.Count)];
        roomCenter.Remove(currentRoomCenter);

        while (roomCenter.Count > 0)
        {
            Vector2Int closest = FindClosestPointTo(currentRoomCenter, roomCenter);
            roomCenter.Remove(closest);
            HashSet<Vector2Int> newCorridor = CreateCorridor(currentRoomCenter, closest);
            currentRoomCenter = closest;
            corridors.UnionWith(newCorridor);
        }
        return corridors;
    }

    private HashSet<Vector2Int> CreateCorridor(Vector2Int currentRoomCenter, Vector2Int closest){
        HashSet<Vector2Int> corridor = new HashSet<Vector2Int>();
        var position = currentRoomCenter;
        corridor.Add(position);
        while (position.y != closest.y)
        {
            if(closest.y > position.y){
                position += Vector2Int.up;
            }else if(closest.y < position.y){
                position += Vector2Int.down;
            }
            corridor.Add(position);
        }
        while (position.x != closest.x)
        {
            if(closest.x > position.x){
                position += Vector2Int.right;
            }else if(closest.x < position.x){
                position += Vector2Int.left;
            }
            corridor.Add(position);
        }
        return corridor;
    }

    private Vector2Int FindClosestPointTo(Vector2Int currentRoomCenter, List<Vector2Int> roomCenter){
        Vector2Int closest = Vector2Int.zero;
        float length = float.MaxValue;
        foreach (var position in roomCenter)
        {
            float currentDistance = Vector2.Distance(position, currentRoomCenter);
            if(currentDistance < length){
                length = currentDistance;
                closest = position;
            }
        }
        return closest;
    }

    private HashSet<Vector2Int> CreateSimpleRooms(List<BoundsInt> roomList){
        HashSet<Vector2Int> floor = new HashSet<Vector2Int>();
        foreach (var room in roomList)
        {
            for (int col = offset; col < room.size.x - offset; col++)
            {
                for (int row = offset; row < room.size.y - offset; row++)
                {
                    Vector2Int pos = (Vector2Int)room.min + new Vector2Int(col,row);
                    floor.Add(pos);
                }
            }
        }
        return floor;
    }

}
