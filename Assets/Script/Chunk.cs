using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]


public class Chunk : MonoBehaviour
{
    public Block[, ,] blocks = new Block[chunkSize, chunkSize, chunkSize];
    public static int chunkSize = 16;
    public bool update = true;
    public World world;
    public WorldPos pos;

    MeshFilter filter;
    MeshCollider coll;


    // Use this for initialization
    void Start()
    {
        filter = gameObject.GetComponent<MeshFilter>();
        coll = gameObject.GetComponent<MeshCollider>();

        blocks = new Block[chunkSize, chunkSize, chunkSize];

        for (int x = 0; x < chunkSize; x++)
        {
            for (int y = 0; y < chunkSize; y++)
            {
                for (int z = 0; z < chunkSize; z++)
                {
                    blocks[x, y, z] = new BlockAir();
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (update)
        {
            blocks[1, 1, 1] = new BlockGrass();
            update = false;
            UpdateChunk();
        }
    }

    public Block GetBlock(int x, int y, int z)
    {
        if (InRange(x) && InRange(y) && InRange(z))
        {
            return blocks[x, y, z];

        }

        return world.GetBlock(pos.x + x, pos.y + y, pos.z + z);
    }

    // check coordinates
    public static bool InRange(int index)
    {
        if (index < 0 || index >=chunkSize)
        {
            return false;
        }

        return true;
    }

    // Set block in position else send to world
    public void SetBlock(int x, int y, int z, Block block)
    {
        if (InRange(x) && InRange(y) && InRange(z))
        {
            blocks[x, y, z] = block;
        }

        else
        {
            world.SetBlock(pos.x + x, pos.y + y, pos.z + z, block);
        }
    }


    // Updates the Chunk based on its contests
    void UpdateChunk()
    {

        MeshData meshData = new MeshData();
        for (int x = 0; x < chunkSize; x++)
        {
            for (int y = 0; y < chunkSize; y++)
            {
                for (int z = 0; z < chunkSize; z++)
                {
                    meshData = blocks[x, y, z].BlockData(this, x, y, z, meshData);
                }
            }
        }
        RenderMesh(meshData);
    }

    // Sends the calculated mesh information
    // to the mesh and collision components
    void RenderMesh(MeshData meshData)
    {
        filter.mesh.Clear();
        filter.mesh.vertices = meshData.vertices.ToArray();
        filter.mesh.triangles = meshData.triangles.ToArray();
        filter.mesh.uv = meshData.uv.ToArray();
        filter.mesh.RecalculateNormals();


        // remove current mesh and create new
        coll.sharedMesh = null;
        Mesh mesh = new Mesh();
        mesh.vertices = meshData.colliderVertices.ToArray();
        mesh.triangles = meshData.colliderTriangles.ToArray();
        mesh.RecalculateNormals();

        coll.sharedMesh = mesh;
    }

    public void SetBlocksUnmodified()
    {
        foreach (Block block in blocks)
        {
            block.changed = false;
        }
    }
}
