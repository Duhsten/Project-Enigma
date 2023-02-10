using ProjectEnigma.Util;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DevMapView : MonoBehaviour
{
    private Tilemap map;
    // Start is called before the first frame update
    void Start()
    {
        map = GetComponent<Tilemap>();
    }
    public void RenderMap()
    {
        map.ClearAllTiles();
        List<ProjectEnigma.Rooms.Room> rooms = GameManager.GetInstance().GetRooms();
        foreach(var room in rooms)
        {
            Color roomColor = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0, 1), .5f);
            foreach(TileVector p in room.FloorPoints)
            {
                map.SetTile(new Vector3Int(p.x, p.y), Tiles.DEBUG_TILE(roomColor));
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
