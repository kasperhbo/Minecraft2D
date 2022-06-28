using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class NewWorldGenerator : MonoBehaviour
{
    public static int ChunkSize = 1000;
    
    public Tilemap tileMap;
    public Tile waterTile;
    public List<Blocks> blocks = new List<Blocks>();

    private Blocks[,] blockIndexes = new Blocks[ChunkSize, ChunkSize];

    int North = 1;
    int West = 2;
    int East = 4;
    int South = 8;
    
    private void Start()
    {
        CreateWorld();
        CheckNeighbours();
        FillWorld();
    }


    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Vector3Int clickedTilePos = tileMap.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            blockIndexes[clickedTilePos.x, clickedTilePos.y] = blocks[0];
            CheckNeighbours();
        }
    }

    private void CreateWorld()
    {
        for (int y = 0; y < ChunkSize; y++)
        {
            for (int x = 0; x < ChunkSize; x++)
            {
                float sample = Noise.Get2DPerlin(new Vector2(x, y), 0, 1, ChunkSize);
                
                if(sample > 0.3)    
                    blockIndexes[x, y] = blocks[0];
                else
                    blockIndexes[x, y] = blocks[16];
            }
        }
    }

    private void CheckNeighbours()
    {
        for (int y = 1; y < ChunkSize - 1; y++)
        {
            for (int x = 1; x < ChunkSize - 1; x++)
            {
                if(!blockIndexes[x,y].isWater)
                {
                    int blockN = blockIndexes[x, y + 1].blockIndex;                    
                    int blockE = blockIndexes[x + 1, y].blockIndex;
                    int blockS = blockIndexes[x, y - 1].blockIndex;
                    int blockW = blockIndexes[x - 1, y].blockIndex;

                    int number = 1 * blockN + 4 * blockE + 8 * blockS + 2 * blockW;
                    //int number =1 * blockNW + 2 * blockN + 4 * blockNE + 8 * blockW + 16 * blockE + 32 * blockSW + 64 * blockS +
                                //128 * blockEW;
                    blockIndexes[x, y] = blocks[number];
                }
            }
        }
    }
    
    private void FillWorld()
    {
        for (int y = 0; y < ChunkSize; y++)
        {
            for (int x = 0; x < ChunkSize; x++)
            {
                Tile blockTile = null;
                
                foreach (var block in blocks)
                {
                    if(block == blockIndexes[x,y])
                    {
                        blockTile = block.blockTile;
                        break;
                    }
                }

                if (blockIndexes[x, y].isWater)
                {
                    blockTile = waterTile;
                }
                
                tileMap.SetTile(new Vector3Int(x,y,0), blockTile);
            }
        }
    }
}

[Serializable]
public class Blocks
{
    public Tile blockTile;
    public int blockIndex;
    public int bitmaskValue;
    public bool isWater;
}

