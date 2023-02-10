using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class RoomData : UnityEngine.Object
{
    public CellData[,] data;
    public List<Vector2Int> doorPositions;
    public List<DoorTileHandle> doors;
    public List<Vector3Int> floorPositions;
    public List<Vector2> floorWorldPositions;
}
