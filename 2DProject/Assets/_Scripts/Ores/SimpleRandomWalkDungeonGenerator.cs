﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class SimpleRandomWalkDungeonGenerator : AbstractDungeonGenerator
{

    [SerializeField]
    protected SimpleRandomWalkSO randomWalkParameters;
    protected List<Vector2Int> wallPosition = new List<Vector2Int>();
    protected override void RunProceduralGeneration()
    {
        int x = worldSize.x - boardWidth.x;
        int y = worldSize.y - boardWidth.y;
        int size = randomWalkParameters.walkLength;
        int rndX = Random.Range(-x, x);
        int rndY = Random.Range(-y, y);

        if (IsAreaEmpty(new Vector2Int(rndX, rndY), new Vector2Int(size, size)))
        {

            HashSet<Vector2Int> floorPositions = RunRandomWalk(randomWalkParameters, new Vector2Int(rndX, rndY));
            //for (int i = 0; i < tilemapVisualizer.Length; i++)
            //{
            //    tilemapVisualizer[i].Clear();
            //}
            int rnd = Random.Range(0, tilemapVisualizer.Length);
            tilemapVisualizer[rnd].PaintFloorTiles(floorPositions);
            WallGenerator.CreateWalls(floorPositions, tilemapVisualizer[0]);
            wallPosition.Add(new Vector2Int(rndX, rndY));
            
        }
    }
    public bool IsAreaEmpty(Vector2Int center, Vector2Int size)
    {
        for (int x = -size.x / 2; x <= size.x / 2; x++)
        {
            for (int y = -size.y / 2; y <= size.y / 2; y++)
            {
                Vector3Int cellPosition = new Vector3Int(center.x + x, center.y + y,0);
                if (Tilemap.GetTile(cellPosition) != null)
                {
                    return false;
                }
            }
        }
        return true; 
    }

    protected HashSet<Vector2Int> RunRandomWalk(SimpleRandomWalkSO parameters, Vector2Int position)
    {
        var currentPosition = position;
        HashSet<Vector2Int> floorPositions = new HashSet<Vector2Int>();
        for (int i = 0; i < parameters.iterations; i++)
        {
            var path = ProceduralGenerationAlgorithms.SimpleRandomWalk(currentPosition, parameters.walkLength);
            floorPositions.UnionWith(path);
            if (parameters.startRandomlyEachIteration)
                currentPosition = floorPositions.ElementAt(Random.Range(0, floorPositions.Count));
        }
        return floorPositions;
    }

}
