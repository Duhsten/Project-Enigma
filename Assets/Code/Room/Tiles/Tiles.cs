using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Tiles : MonoBehaviour
{
    public static Tile WALL_TILE = Resources.Load("Textures/Wall") as Tile;
    public static Tile FLOOR_TILE = Resources.Load("Textures/Floor") as Tile;
    public static InteractableTile DOOR_TILE(Vector2Int doorPoint, Vector2Int doorDirection, ProjectEnigma.Rooms.Room room)
    {
        InteractableTile tile = Resources.Load("Textures/Door") as InteractableTile;
        tile.gameObject.GetComponent<DoorTileHandle>().SetDoorPoint(doorPoint);
        tile.gameObject.GetComponent<DoorTileHandle>().SetRoom(room);
        tile.gameObject.GetComponent<DoorTileHandle>().SetDoorDirection(doorDirection);
        return tile;
    }
    public static Tile DEBUG_TILE(Color color)
    {
        Tile tile = Resources.Load("Textures/Debug") as Tile;
        tile.color = color;
        return tile;
    }
    public static Tile EMPTY_TILE = new Tile()
    {
        color = Color.clear,
        sprite = Resources.Load("Textures/Base") as Sprite,
    };

}
