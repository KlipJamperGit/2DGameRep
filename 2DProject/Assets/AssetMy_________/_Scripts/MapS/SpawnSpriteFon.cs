using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;

public class SpawnSpriteFon : MonoBehaviour
{
    public List<GameObject> objects;

    [Range(1, 100)]
    public float scale = 20f;

    [Range(0, 1)]
    public float threshold = 0.5f;

    [Range(0, 1)]
    public float density = 0.5f; // Щільність спавну об'єктів

    public int minRegionSize = 20;

    public Tilemap tilemap;
    public Tilemap tilemapWalls;
    public Tile blackTile;
    public Tile whiteTile;
    [SerializeField] TilemapVisualizer tilemapVisualizer;

    private bool[,] noiseMap;
    private int width;
    private int height;

    private float randomOffsetX;
    private float randomOffsetY;

    private List<GameObject> spawnedObjects = new List<GameObject>(); // Список для збереження створених об'єктів
    public GameObject objectsContainer; // Контейнер для заспавнених об'єктів
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
        ClearSpawnedObjects(); // Видаляємо попередні об'єкти

        Vector3Int offset = new Vector3Int(-width / 2, -height / 2, 0);

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Tile tileToPlace = noiseMap[x, y] ? blackTile : whiteTile;
                tilemap.SetTile(new Vector3Int(x, y, 0) + offset, tileToPlace);

                if (noiseMap[x, y])
                {
                    Tile wallTile = GetWallTile(x, y);
                    tilemapWalls.SetTile(new Vector3Int(x, y, 0) + offset, wallTile);

                    // Спавн об'єкта за умови, що клітинки зверху і з боків вільні
                    if (CanSpawnObject(x, y))
                    {
                        if (Random.value <= density)
                        {
                            SpawnRandomObject(new Vector3Int(x, y, 0) + offset);
                        }
                    }
                }
            }
        }
    }

    bool CanSpawnObject(int x, int y)
    {
        // Перевіряємо клітинки зверху
        for (int i = 1; i <= 3; i++)
        {
            if (!IsCellEmpty(x, y + i)) return false;
        }

        // Перевіряємо клітинки зліва та справа на трьох рівнях зверху
        //for (int i = 0; i < 3; i++)
        //{
        //    int checkY = y + i;
        //    if (!IsCellEmpty(x - 1, checkY) || !IsCellEmpty(x + 1, checkY)) return false;
        //}

        // Перевіряємо клітинки знизу
        if (!IsCellEmpty(x, y - 1)) return false;

        return true;
    }
    bool IsCellEmpty(int x, int y)
    {
        return IsInMapRange(x, y) && !noiseMap[x, y];
    }
    

    void SpawnRandomObject(Vector3Int cellPosition)
    {
        if (objects != null && objects.Count > 0)
        {
            GameObject randomObject = objects[Random.Range(0, objects.Count)];

            Vector3 worldPosition = tilemap.CellToWorld(cellPosition);
            worldPosition += new Vector3(0.5f, 0.5f, 0); // Вирівнювання об'єкта по центру клітинки

            // Створення об'єкта
            GameObject spawnedObject = Instantiate(randomObject, worldPosition, Quaternion.identity);

            // Додавання до контейнера
            if (objectsContainer == null)
            {
                objectsContainer = new GameObject("SpawnedObjectsContainer"); // Створення контейнера, якщо його немає
            }

            spawnedObject.transform.parent = objectsContainer.transform;

            // Додавання до списку заспавнених об'єктів
            spawnedObjects.Add(spawnedObject);
        }
    }

    public void ClearSpawnedObjects()
    {
        if (objectsContainer != null)
        {
            DestroyImmediate(objectsContainer); // Видаляємо контейнер і всі дочірні об'єкти
            objectsContainer = null;   // Скидаємо посилання на контейнер
        }

        spawnedObjects.Clear(); // Очищуємо список об'єктів
    }

    Tile GetWallTile(int x, int y)
    {
        bool top = IsInMapRange(x, y + 1) && noiseMap[x, y + 1];
        bool bottom = IsInMapRange(x, y - 1) && noiseMap[x, y - 1];
        bool left = IsInMapRange(x - 1, y) && noiseMap[x - 1, y];
        bool right = IsInMapRange(x + 1, y) && noiseMap[x + 1, y];

        if (top && bottom && left && right) return (Tile)tilemapVisualizer.floorTile;
        if (top && !bottom && !left && !right) return (Tile)tilemapVisualizer.wallTop;
        if (!top && bottom && !left && !right) return (Tile)tilemapVisualizer.wallBottom;

        return (Tile)tilemapVisualizer.wallTop;
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
        ClearSpawnedObjects(); // Видаляємо об'єкти при очищенні
    }
}
