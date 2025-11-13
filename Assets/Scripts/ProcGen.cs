using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class ProcGen : MonoBehaviour
{
    [SerializeField] private int width, height;
    [SerializeField] private float smoothness;
    [SerializeField] private float seed;
    [SerializeField] private TileBase groundTile, caveTile;
    [SerializeField] private Tilemap groundTilemap, caveTilemap;
    private int[,] map;
    
    [Header("Caves")] 
    [Range(0, 1)]
    [SerializeField] private float modifier;

    void Start()
    {
        Generation();
        GameController.OnReset += Generation;
    }
    
    public void Generation()
    {
        seed = Random.Range(-10000, 10000);
        ClearMap();
        groundTilemap.ClearAllTiles();
        
        map = GenerateArray(width, height, true);
        map = TerrainGeneration(map);
        RenderMap(map, groundTilemap, caveTilemap, groundTile, caveTile, 0 , 0);

        seed = Random.Range(-10000, 10000);
        map = TerrainGeneration(map);
        RenderMap(map, groundTilemap, caveTilemap, groundTile, caveTile, 0 , 50);
    }

    public int [,] GenerateArray(int width, int height, bool empty)
    {
        int[,] map =  new int[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                map[x, y] = empty ? 0 : 1;
            }
        }
        return map;
    }

    public int[,] TerrainGeneration(int[,] map)
    {
        int perlinHeight;
        for (int x = 0; x < width; x++)
        {
            perlinHeight = Mathf.RoundToInt(Mathf.PerlinNoise(x / smoothness, seed) * height / 2);
            perlinHeight += height / 2;

            for (int y = 0; y < perlinHeight; y++)
            {
                // map[x, y] = 1;
                int caveValue = Mathf.RoundToInt(Mathf.PerlinNoise((x * modifier) + seed, (y * modifier) + seed));
                map[x, y] = (caveValue == 1) ? 2 : 1;
            }
        }

        return map;
    }

    public void RenderMap(int[,] map, Tilemap groundTilemap, Tilemap caveTilemap, TileBase groundTile, TileBase caveTile, int originX, int originY)
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (map[x, y] == 1)
                {
                    groundTilemap.SetTile(new Vector3Int(x + originX, y + originY, 0), groundTile);
                } else if (map[x, y] == 2)
                {
                    caveTilemap.SetTile(new Vector3Int(x + originX, y + originY, 0), caveTile);
                }
            }
        }
    }

    void ClearMap()
    {
        groundTilemap.ClearAllTiles();
        caveTilemap.ClearAllTiles();
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position, new Vector3(width, height, 1));
    }
}
