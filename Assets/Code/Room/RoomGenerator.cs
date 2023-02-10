using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RoomGenerator : MonoBehaviour
{
    private static int roomCount;
    private static void RenderRoom(Tilemap map, RoomData roomData, int offsetX = 0, int offsetY = 0)
    {
        roomData.doors = new List<DoorTileHandle>();
        roomData.floorWorldPositions = new List<Vector2>();
        roomData.floorPositions = new List<Vector3Int>();
        for (int x = 0; x < roomData.data.GetLength(0); x++)
        {
            for (int y = 0; y < roomData.data.GetLength(1); y++)
            {
                CellData cell = roomData.data[x, y];
                Vector3Int tilePos = new Vector3Int(x + offsetX, y + offsetY, 0);

                if (cell.cellType == CellType.WALL)
                {
                    map.SetTile(tilePos, Tiles.WALL_TILE);
                }
                else if (cell.cellType == CellType.FLOOR)
                {
                    map.SetTile(tilePos, Tiles.FLOOR_TILE);
                    roomData.floorPositions.Add(tilePos);
                    roomData.floorWorldPositions.Add(map.CellToWorld(tilePos));
                }
                else if (cell.cellType == CellType.DOOR)
                {
                    //InteractableTile tile = Tiles.DOOR_TILE(new Vector2Int(tilePos.x, tilePos.y));
                    //roomData.doors.Add(tile.gameObject.GetComponent<DoorTileHandle>());
                    //map.SetTile(tilePos, tile);
                    //roomData.floorPositions.Add(tilePos);
                    //roomData.floorWorldPositions.Add(map.CellToWorld(tilePos));
                }
            }
        }
        map.RefreshAllTiles();
    }

    public static Room RenderGeneratedBaseRoom(Tilemap map, Room neighbor = null)
    {
        NormalRoom room = new NormalRoom();
        RoomData roomData = room.GenerateRoomData();
        RenderRoom(map, roomData);

        Room newRoom = new Room()
        {
            Name = "Room " + roomCount,
            roomData = roomData
        };
        return newRoom;
    }

    public static IEnumerator RenderGeneratedAttachedRoom(Tilemap map, Room neighbor, Vector2Int doorPoint)
    {
        NormalRoom room = new NormalRoom();
        RoomData roomData = room.GenerateRoomData();
        // The width and height of the neighbor room
        int neighborWidth = neighbor.roomData.data.GetLength(0);
        int neighborHeight = neighbor.roomData.data.GetLength(1);

        // The direction from the neighbor room to the new room
        Vector2 d = doorPoint - new Vector2Int(neighborWidth / 2, neighborHeight / 2);
        d = d.normalized;
        Vector2Int direction = new Vector2Int((int)d.x, (int)d.y);

        // The starting position for the new room
        Vector2Int startPos = doorPoint + direction * roomData.data.GetLength(0) / 2;

        // Check if the new room fits on the map
        bool roomFits = CheckRoomFits(map, startPos, direction, roomData);
        // If the room doesn't fit, try generating a new one
       
        // Render the new room on the map
        RenderRoom(map, roomData, doorPoint.x, doorPoint.y);

        // Create the new Room object
        Room newRoom = new Room()
        {
            Name = "Room " + roomCount,
            roomData = roomData
        };

        // Update the door in the neighbor room
        //DoorTileHandle door = neighbor.roomData.doors.Find(d => d.doorPos == doorPoint);
        //door.attachedRoom = newRoom;

        yield return newRoom;
    }

    private static bool CheckRoomFits(Tilemap map, Vector2Int startPos, Vector2Int direction, RoomData roomData)
    {
        // The width and height of the new room
        int roomWidth = roomData.data.GetLength(0);
        int roomHeight = roomData.data.GetLength(1);

        // Check if the new room fits in the x-direction
        if (direction.x == 1)
        {
            int maxX = startPos.x + roomWidth;
            if (maxX >= map.size.x)
            {
                return false;
            }
        }
        else if (direction.x == -1)
        {
            int minX = startPos.x - roomWidth;
            if (minX < 0)
            {
                return false;
            }
        }

        // Check if the new room fits in the y-direction
        if (direction.y == 1)
        {
            int maxY = startPos.y + roomHeight;
            if (maxY >= map.size.y)


            {
                return false;
            }
        }
        else if (direction.y == -1)
        {
            int minY = startPos.y - roomHeight;
            if (minY < 0)
            {
                return false;
            }
        }
        // Check if the new room overlaps any existing rooms
        for (int x = 0; x < roomWidth; x++)
        {
            for (int y = 0; y < roomHeight; y++)
            {
                int checkX = startPos.x + (x * direction.x);
                int checkY = startPos.y + (y * direction.y);
                Vector3Int checkPos = new Vector3Int(checkX, checkY, 0);
                TileBase tile = map.GetTile(checkPos);
                if (tile != null)
                {
                    return false;
                }
            }
        }

        return true;
    }

    }
