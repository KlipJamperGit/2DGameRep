using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public abstract class AbstractDungeonGenerator : MonoBehaviour
{
    [SerializeField] protected Tilemap Tilemap = null;
    [SerializeField] protected TilemapVisualizer[] tilemapVisualizer = null;
    [SerializeField] protected SpawnSpriteFon map = null;
    [SerializeField] protected Vector2Int startPosition = Vector2Int.zero;
    [SerializeField] protected Vector2Int worldSize = Vector2Int.zero;
    [SerializeField] protected Vector2Int boardWidth = Vector2Int.zero;
    [SerializeField] protected int numberObjects = 0;

    public void GenerateDungeon()
    {
        map.Clear();
        for (int i = 0; i < tilemapVisualizer.Length; i++)
        {
            tilemapVisualizer[i].Clear();
        }
        for (int i = 0; i < numberObjects; i++)
        {
            RunProceduralGeneration();
        }
        map.GenerateCaves(worldSize.x * 2, worldSize.y * 2);
    }

    protected abstract void RunProceduralGeneration();
}
