using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;

public class SpawnSpriteFon : MonoBehaviour
{
    [Range(1, 100)]
    public float scale = 20f;
    [Range(0, 1)]
    public float threshold = 0.5f;
    public int minRegionSize = 20;

    public Tilemap tilemap;
    public Tilemap tilemapWalls;
    public Tile blackTile;
    public Tile whiteTile;

    private bool[,] noiseMap;
    private int width;
    private int height;

    private float randomOffsetX; // Зміщення по X
    private float randomOffsetY; // Зміщення по Y

    public void GenerateCaves(int mapWidth, int mapHeight)
    {
        randomOffsetX = Random.Range(0f, 100f);
        randomOffsetY = Random.Range(0f, 100f);
        width = mapWidth;
        height = mapHeight;

        noiseMap = GenerateNoiseMap();
        FilterSmallRegions();

        tilemap.ClearAllTiles();
        tilemapWalls.ClearAllTiles();

        Vector3Int offset = new Vector3Int(-width / 2, -height / 2, 0);

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Tile tileToPlace = noiseMap[x, y] ? blackTile : whiteTile;
                tilemap.SetTile(new Vector3Int(x, y, 0) + offset, tileToPlace);

                // Якщо плитка чорна, додати її на другий Tilemap
                if (noiseMap[x, y])
                {
                    tilemapWalls.SetTile(new Vector3Int(x, y, 0) + offset, blackTile);
                }
            }
        }
    }

    bool[,] GenerateNoiseMap()
    {
        bool[,] map = new bool[width, height];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                float xCoord = (float)x / width * scale + randomOffsetX;
                float yCoord = (float)y / height * scale + randomOffsetY;

                float noiseValue = Mathf.PerlinNoise(xCoord, yCoord);
                map[x, y] = noiseValue > threshold;
            }
        }

        return map;
    }

    void FilterSmallRegions()
    {
        List<List<Vector2Int>> regions = GetRegions();

        foreach (var region in regions)
        {
            if (region.Count < minRegionSize)
            {
                foreach (var tile in region)
                {
                    noiseMap[tile.x, tile.y] = false;
                }
            }
        }
    }

    List<List<Vector2Int>> GetRegions()
    {
        bool[,] visited = new bool[width, height];
        List<List<Vector2Int>> regions = new List<List<Vector2Int>>();

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (!visited[x, y] && noiseMap[x, y])
                {
                    List<Vector2Int> newRegion = GetRegionTiles(x, y, visited);
                    regions.Add(newRegion);
                }
            }
        }

        return regions;
    }

    List<Vector2Int> GetRegionTiles(int startX, int startY, bool[,] visited)
    {
        List<Vector2Int> tiles = new List<Vector2Int>();
        Queue<Vector2Int> queue = new Queue<Vector2Int>();
        queue.Enqueue(new Vector2Int(startX, startY));
        visited[startX, startY] = true;

        Vector2Int[] directions =
        {
            new Vector2Int(0, 1),
            new Vector2Int(1, 0),
            new Vector2Int(0, -1),
            new Vector2Int(-1, 0)
        };

        while (queue.Count > 0)
        {
            Vector2Int currentTile = queue.Dequeue();
            tiles.Add(currentTile);

            foreach (var dir in directions)
            {
                int neighbourX = currentTile.x + dir.x;
                int neighbourY = currentTile.y + dir.y;

                if (IsInMapRange(neighbourX, neighbourY) && !visited[neighbourX, neighbourY] && noiseMap[neighbourX, neighbourY])
                {
                    visited[neighbourX, neighbourY] = true;
                    queue.Enqueue(new Vector2Int(neighbourX, neighbourY));
                }
            }
        }

        return tiles;
    }

    bool IsInMapRange(int x, int y)
    {
        return x >= 0 && x < width && y >= 0 && y < height;
    }

    public void Clear()
    {
        tilemap.ClearAllTiles();
        tilemapWalls.ClearAllTiles();
    }
}