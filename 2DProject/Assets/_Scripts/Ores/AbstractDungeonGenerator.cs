using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractDungeonGenerator : MonoBehaviour
{
    [SerializeField] protected TilemapVisualizer tilemapVisualizer = null;
    [SerializeField] protected SpawnSprite map = null;
    [SerializeField] protected Vector2Int startPosition = Vector2Int.zero;
    [SerializeField] protected Vector2Int worldSize = Vector2Int.zero;
    [SerializeField] protected Vector2Int boardWidth = Vector2Int.zero;
    [SerializeField] protected int numberObjects = 0;

    public void GenerateDungeon()
    {
        map.Clear();
        tilemapVisualizer.Clear();
       
        RunProceduralGeneration();
        map.GenerateCaves(worldSize.x * 2, worldSize.y * 2);
    }

    protected abstract void RunProceduralGeneration();
}
