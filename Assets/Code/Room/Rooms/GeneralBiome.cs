using ProjectEnigma.Rooms;
using ProjectEnigma.Util;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GeneralBiome : RoomBiome
{
    public override string Biome { get => "General"; }

    public override float Probability { get => .8f; }

public override List<TileVector> GenerateFloorShape(Tilemap world, Vector2Int startPos, Vector2Int direction)
    {
        List<TileVector> points = new List<TileVector>();
        int width = 10000;
        int height = 1000;

        int[,] shape = new int[width, height];

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                shape[i, j] = 0;
            }
        }

        Vector2Int endPos = new Vector2Int(Random.Range(0, width), Random.Range(0, height));

        Queue<Vector2Int> queue = new Queue<Vector2Int>();
        queue.Enqueue(startPos);

        if (startPos.x >= 0 && startPos.x < shape.GetLength(0) &&
            startPos.y >= 0 && startPos.y < shape.GetLength(1))
        {
            shape[startPos.x, startPos.y] = 1;
        }
        else
        {
            Debug.LogError("Start position is outside the bounds of the array");
        }

        while (queue.Count > 0)
        {
            Vector2Int currPos = queue.Dequeue();
            List<Vector2Int> neighbors = GetValidNeighbors(currPos, shape, width, height, direction);

            int numNeighbors = neighbors.Count;
            if (numNeighbors > 0)
            {
                int randIndex = Random.Range(0, numNeighbors);
                Vector2Int nextPos = neighbors[randIndex];
                shape[nextPos.x, nextPos.y] = 1;
                queue.Enqueue(nextPos);
            }
        }

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (shape[i, j] == 1)
                {
                    if (world.GetTile(new Vector3Int(i, j, 0)) != Tiles.FLOOR_TILE && world.GetTile(new Vector3Int(i, j, 0)) != Tiles.WALL_TILE && world.GetTile(new Vector3Int(i, j, 0)) is not InteractableTile)
                    {
                        world.SetTile(new Vector3Int(i, j, 0), Tiles.FLOOR_TILE);
                        points.Add(new TileVector(i, j));
                    }
                }
            }
        }
        return points;
    }
}
