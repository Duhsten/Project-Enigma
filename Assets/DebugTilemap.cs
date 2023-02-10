using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DebugTilemap : MonoBehaviour
{
    public Tilemap map;
    public static DebugTilemap Instance;
    public ProjectEnigma.Rooms.Room room;
    public List<ProjectEnigma.Rooms.Room> rooms;
    // Start is called before the first frame update
    void Start()
    {
    }
    public static void SetInstance(DebugTilemap d)
    {
        Instance = d; 
    }
    public static DebugTilemap GetInstance()
    {
        return Instance;
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    }
