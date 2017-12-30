using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshData
{
    public List<Vector3> vertices = new List<Vector3>();
    public List<int> triangles = new List<int>();
    public List<Vector2> uv = new List<Vector2>();
    public List<Vector3> colliderVertices = new List<Vector3>();
    public List<int> colliderTriangles = new List<int>();
    public bool useRenderDataForCollision;

    public void AddQuadTriangles()
    {
        triangles.Add(vertices.Count - 4);
        triangles.Add(vertices.Count - 3);
        triangles.Add(vertices.Count - 2);
        triangles.Add(vertices.Count - 4);
        triangles.Add(vertices.Count - 2);
        triangles.Add(vertices.Count - 1);
        if (useRenderDataForCollision)
        {
            colliderTriangles.Add(colliderVertices.Count - 4);
            colliderTriangles.Add(colliderVertices.Count - 3);
            colliderTriangles.Add(colliderVertices.Count - 2);
            colliderTriangles.Add(colliderVertices.Count - 4);
            colliderTriangles.Add(colliderVertices.Count - 2);
            colliderTriangles.Add(colliderVertices.Count - 1);
        }
    }

    // add to triangle list if(true) add to collider list
    public void AddTriagnle(int tri)
    {
        triangles.Add(tri);
        if (useRenderDataForCollision)
        {
            colliderTriangles.Add(tri - (vertices.Count - colliderVertices.Count));
        }
    }

    public void AddVertex(Vector3 vertex)
    {
        vertices.Add(vertex);
        if (useRenderDataForCollision)
        {
            colliderVertices.Add(vertex);
        }
    }

    public MeshData() {}
}
