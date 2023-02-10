using ProjectEnigma.Rooms;
using ProjectEnigma.Util;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public class GameManager : MonoBehaviour
{
    public Tilemap map;
    public List<ProjectEnigma.Rooms.Room> rooms;

    public void Start()
    {

            map = GetComponent<Tilemap>();
        if (ProjectEnigma.Data.SaveManager.CurrentSave.Data is null)
        {
            Debug.Log("New Game Detected");
            var room = GetBiome().NewRoom(map);

            rooms.Add(room);
            var player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
            player.TeleportPlayer(TileVector.ToVector2Int(room.FloorPoints[Random.Range(0, room.FloorPoints.Count - 1)]));
            ProjectEnigma.Data.SaveManager.CurrentSave.Data = new ProjectEnigma.Data.SaveData();
            ProjectEnigma.Data.SaveManager.CurrentSave.Data.rooms = rooms;
            ProjectEnigma.Data.SaveManager.SaveGame();
        }
        else
        {
            LoadSavedGame();
        }
        
        
    }
    private void LoadSavedGame()
    {
        foreach(var room in ProjectEnigma.Data.SaveManager.CurrentSave.Data.rooms)
        {
            foreach(var floorPoint in room.FloorPoints)
            {
                map.SetTile(TileVector.ToVector3Int(floorPoint), Tiles.FLOOR_TILE);
            }
            foreach (var wallPoint in room.WallPoints)
            {
                map.SetTile(TileVector.ToVector3Int(wallPoint), Tiles.WALL_TILE);
            }
            foreach (var doorPoint in room.DoorPoints)
            {
                map.SetTile(TileVector.ToVector3Int(doorPoint.Key), Tiles.DOOR_TILE(TileVector.ToVector2Int(doorPoint.Key), TileVector.ToVector2Int(doorPoint.Value), room));
            }
        }
    }
    public static GameManager GetInstance()
    {
        return GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }
    public List<ProjectEnigma.Rooms.Room> GetRooms()
    {
        return rooms;
    }
    public RoomBiome GetBiome()
    { 
        float totalProbability = 0;
        GameRegistry.RegisterRoomBiomes();
        foreach (RoomBiome biome in GameRegistry.RoomBiomes)
        {
            totalProbability += biome.Probability;
        }

        float randomValue = UnityEngine.Random.Range(0f, totalProbability);

        float currentProbability = 0;
        foreach (RoomBiome biome in GameRegistry.RoomBiomes)
        {
            currentProbability += biome.Probability;
            if (randomValue < currentProbability)
            {
                return biome;
            }
        }

        return GameRegistry.RoomBiomes[GameRegistry.RoomBiomes.Count - 1];
    }
    public void CreateNewRoom(Vector2Int doorPoint, Vector2Int doorDirection, ProjectEnigma.Rooms.Room room)
    {
        //room.AddConnectedRoom(doorPoint, room);
        rooms.Add(GetBiome().NewRoom(map, doorPoint, doorDirection));
    }
}
