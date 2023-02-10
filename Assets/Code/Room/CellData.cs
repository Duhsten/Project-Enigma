using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct CellData
{
    public Texture2D texture;
    public CellType cellType;
}
public enum CellType
{
    WALL,
    EMPTY,
    FLOOR,
    DOOR
}