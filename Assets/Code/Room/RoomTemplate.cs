using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RoomTemplate 
{
    public abstract int RoomWidth();
    public abstract int RoomHeight();
    
    public int[,] GenerateRoom(int[,] roomArray)
    {
        int arrayWidth = roomArray.GetLength(0);
        int arrayHeight = roomArray.GetLength(1);
        int halfWidth = arrayWidth / 2;
        int halfHeight = arrayHeight / 2;

        // Randomly select the shape of the room
        int shape = UnityEngine.Random.Range(0, 4);

        switch (shape)
        {
            // L-shaped room
            case 0:
                for (int i = halfWidth - 2; i <= halfWidth; i++)
                {
                    for (int j = halfHeight - 2; j <= halfHeight; j++)
                    {
                        roomArray[i, j] = 1;
                    }
                }
                roomArray[halfWidth + 1, halfHeight - 2] = 1;
                break;

            // T-shaped room
            case 1:
                for (int i = halfWidth - 1; i <= halfWidth + 1; i++)
                {
                    for (int j = halfHeight - 2; j <= halfHeight; j++)
                    {
                        roomArray[i, j] = 1;
                    }
                }
                roomArray[halfWidth, halfHeight - 3] = 1;
                break;

            // S-shaped room
            case 2:
                for (int i = halfWidth - 1; i <= halfWidth + 1; i++)
                {
                    for (int j = halfHeight - 2; j <= halfHeight - 1; j++)
                    {
                        roomArray[i, j] = 1;
                    }
                }
                roomArray[halfWidth - 2, halfHeight - 2] = 1;
                roomArray[halfWidth + 2, halfHeight - 1] = 1;
                break;

            // Rectangle-shaped room
            case 3:
                for (int i = halfWidth - 2; i <= halfWidth + 2; i++)
                {
                    for (int j = halfHeight - 2; j <= halfHeight + 2; j++)
                    {
                        roomArray[i, j] = 1;
                    }
                }
                break;
        }
        PrintRoom(roomArray);
        return roomArray;
    }


    public void PrintRoom(int[,] arr)
    {

        string output = "";
        for (int i = 0; i < arr.GetLength(0); i++)
        {
            for (int j = 0; j < arr.GetLength(1); j++)
            {
                output += arr[i, j] + " ";
            }
            output += "\n";
        }
        Debug.Log(output);
        output = "";
    }

    public RoomData GenerateRoomData()
    {
        return ScaleRoomData(GenerateRoom(new int[10, 10]), 4);
    }
    public RoomData ScaleRoomData(int[,] roomArray, int scaleFactor)
    {
        int roomWidth = roomArray.GetLength(0);
        int roomHeight = roomArray.GetLength(1);

        CellData[,] cellDataArray = new CellData[roomWidth * scaleFactor, roomHeight * scaleFactor];

        for (int i = 0; i < roomWidth; i++)
        {
            for (int j = 0; j < roomHeight; j++)
            {
                int value = roomArray[i, j];

                if (value == 1)
                {
                    for (int x = 0; x < scaleFactor; x++)
                    {
                        for (int y = 0; y < scaleFactor; y++)
                        {
                            cellDataArray[i * scaleFactor + x, j * scaleFactor + y].cellType = CellType.FLOOR;
                        }
                    }
                }
                else
                {
                    for (int x = 0; x < scaleFactor; x++)
                    {
                        for (int y = 0; y < scaleFactor; y++)
                        {
                            cellDataArray[i * scaleFactor + x, j * scaleFactor + y].cellType = CellType.EMPTY;
                        }
                    }
                }
            }
        }
        for(int i = 0; i < cellDataArray.GetLength(0); i++)
        {
            for(int j = 0;j < cellDataArray.GetLength(1); j++)
            {
                if(cellDataArray[i, j].cellType == CellType.FLOOR)
                {
                    if(cellDataArray[i - 1, j].cellType != CellType.FLOOR)
                    {
                        cellDataArray[i - 1, j].cellType = CellType.WALL;
                    }
                    if (cellDataArray[i + 1, j].cellType != CellType.FLOOR)
                    {
                        cellDataArray[i + 1, j].cellType = CellType.WALL;
                    }
                    if (cellDataArray[i, j - 1].cellType != CellType.FLOOR)
                    {
                        cellDataArray[i, j - 1].cellType = CellType.WALL;
                    }
                    if (cellDataArray[i, j + 1].cellType != CellType.FLOOR)
                    {
                        cellDataArray[i, j + 1].cellType = CellType.WALL;
                    }
                    if (cellDataArray[i + 1, j + 1].cellType != CellType.FLOOR)
                    {
                        cellDataArray[i + 1, j + 1].cellType = CellType.WALL;
                    }
                    if (cellDataArray[i + 1, j - 1].cellType != CellType.FLOOR)
                    {
                        cellDataArray[i + 1, j - 1].cellType = CellType.WALL;
                    }
                    if (cellDataArray[i - 1, j + 1].cellType != CellType.FLOOR)
                    {
                        cellDataArray[i - 1, j + 1].cellType = CellType.WALL;
                    }
                    if (cellDataArray[i - 1, j - 1].cellType != CellType.FLOOR)
                    {
                        cellDataArray[i - 1, j - 1].cellType = CellType.WALL;
                    }

                }
            }
        }
        
        RoomData roomData = new RoomData();
        roomData.data = cellDataArray;
        roomData= AddDoors(roomData);
        return roomData;
    }

    public RoomData AddDoors(RoomData data)
    {
        List<Vector2Int> possibleDoorPoints = new List<Vector2Int> ();
        for (int i = 0; i < data.data.GetLength(0); i++)
        {
            for (int j = 0; j < data.data.GetLength(1); j++)
            {
                if (data.data[i, j].cellType == CellType.WALL)
                {
                    possibleDoorPoints.Add(new Vector2Int(i, j));
                }
            }
        }
        for (int d = 0; d <= 2; d++)
        {
           Vector2Int doorPoint = possibleDoorPoints[UnityEngine.Random.Range(0,possibleDoorPoints.Count-1)];
            data.data[doorPoint.x, doorPoint.y].cellType = CellType.DOOR;
            
        }
        data.doorPositions = possibleDoorPoints;
        return data;
    }



    public void DebugPrintRoomData(RoomData data)
    {
        int width = data.data.GetLength(0);
        int height = data.data.GetLength(1);
        string output = "";

        for (int y = height - 1; y >= 0; y--)
        {
            for (int x = 0; x < width; x++)
            {
                switch (data.data[x, y].cellType)
                {
                    case CellType.WALL:
                        output += "#";
                        break;
                    case CellType.EMPTY:
                        output += " ";
                        break;
                    case CellType.FLOOR:
                        output += "@";
                        break;
                }
            }
            output += "\n";
        }
        Debug.Log(output);
    }



}
