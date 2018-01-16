﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimplexNoise;
using NoiseTest;

public class TerrainGen : ScriptableObject
{
    [SerializeField] float stoneBaseHeight = -24;
    [SerializeField] float stoneBaseNoise = 0.05f;
    [SerializeField] float stoneBaseNoiseHeight = 4;

    [SerializeField] float stoneMountainHeight = 48;
    [SerializeField] float stoneMountainFrequency = 0.008f;
    [SerializeField] float stoneMinHeight = -12;

    [SerializeField] float dirtBaseHeight = 1;
    [SerializeField] float dirtNoise = 0.04f;
    [SerializeField] float dirtNoiseHeight = 3;

    [SerializeField] float caveFrequency = 0.025f;
    [SerializeField] float caveSize = 7;

    [SerializeField] float treeFrequency = 0.2f;
    [SerializeField] int treeDensity = 3;

    [SerializeField] int Seed = 0;

    OpenSimplexNoise noise;


    private void OnEnable()
    {
        noise = new OpenSimplexNoise(Seed);
    }



    public Chunk ChunkGen(Chunk chunk)
    {
        for (int x = chunk.pos.x - 3; x < chunk.pos.x + Chunk.chunkSize + 3; x++)
        {
            for (int z = chunk.pos.z - 3; z < chunk.pos.z + Chunk.chunkSize + 3; z++)
            {
                chunk = ChunkColumnGen(chunk, x, z);
            }
        }

        return chunk;
    }

    public Chunk ChunkColumnGen(Chunk chunk, int x, int z)
    {
        int stoneHeight = Mathf.FloorToInt(stoneBaseHeight);
        stoneHeight += GetNoise(x, 0, z, stoneMountainFrequency, Mathf.FloorToInt(stoneMountainHeight));

        if (stoneHeight < stoneMinHeight)
        {
            stoneHeight = Mathf.FloorToInt(stoneMinHeight);
        }

        stoneHeight += GetNoise(x, 0, z, stoneBaseNoise, Mathf.FloorToInt(stoneBaseNoiseHeight));

        int dirtHeight = stoneHeight + Mathf.FloorToInt(dirtBaseHeight);
        dirtHeight += GetNoise(x, 100, z, dirtNoise, Mathf.FloorToInt(dirtNoiseHeight));

        for (int y = chunk.pos.y - 8; y < chunk.pos.y + Chunk.chunkSize; y++)
        {
            int caveChance = GetNoise(x, y, z, caveFrequency, 100);
            if (y <= stoneHeight && caveSize < caveChance)
            {
                SetBlock(x, y, z, new Block(), chunk);
            }
            else if (y <= dirtHeight && caveSize < caveChance)
            {
                SetBlock(x, y, z, new BlockGrass(), chunk);
                if (y == dirtHeight && GetNoise(x, 0, z, treeFrequency, 100) < treeDensity)
                {
                    CreateTree(x, y + 1, z, chunk);
                }
            }
            else
            {
                SetBlock(x, y, z, new BlockAir(), chunk);
            }
        }

        return chunk;
    }

    public int GetNoise(int x, int y, int z, float scale, int max)
    {
        //OpenSimplexNoise Penis = new OpenSimplexNoise();
        return Mathf.FloorToInt((noise.Evaluate(x * scale, y * scale, z * scale) + 1f) * (max / 2f));

        //return Mathf.FloorToInt((Noise.Generate(x * scale, y * scale, z * scale) + 1f) * (max / 2f));

       

    }


    public static void SetBlock(int x, int y, int z, Block block, Chunk chunk, bool replaceBlock = false)
    {
        x -= chunk.pos.x;
        y -= chunk.pos.y;
        z -= chunk.pos.z;

        if (Chunk.InRange(x) && Chunk.InRange(y) && Chunk.InRange(z))
        {
            if (replaceBlock || chunk.blocks[x, y, z] == null)
            {
                chunk.SetBlock(x, y, z, block);
            }
        }
    }

    void CreateTree(int x, int y, int z, Chunk chunk)
    {
        // leaves
        for (int xi = -2; xi <= 2; xi++)
        {
            for (int yi = 4; yi <= 8; yi++)
            {
                for (int zi = -2; zi <= 2; zi++)
                {
                    SetBlock(x + xi, y + yi, z + zi, new BlockLeaves(), chunk, true);
                }
            }
        }

        // create trunk
        for (int yt = 0; yt < 6; yt++)
        {
            SetBlock(x, y + yt, z, new BlockWood(), chunk, true);
        }
    }
}
