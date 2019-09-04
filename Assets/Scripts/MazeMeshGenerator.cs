using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeMeshGenerator 
{
    public float width;
    public float height;

    public MazeMeshGenerator()
    {
        width = 3.75f;
        height = 3.5f;
    }

    public Mesh FromData(int[,] data)
    {
        Mesh maze = new Mesh();

        List<Vector3> newVertices = new List<Vector3>();
        List<Vector2> newUVs = new List<Vector2>();
        List<int> newTriangles = new List<int>();

        //corners of quad
        Vector3 vert1 = new Vector3(-.5f, -.5f, 0);
        Vector3 vert2 = new Vector3(-.5f, .5f, 0);
        Vector3 vert3 = new Vector3(.5f, .5f, 0);
        Vector3 vert4 = new Vector3(.5f, -.5f, 0);
       
        newVertices.Add(vert1);
        newVertices.Add(vert2);
        newVertices.Add(vert3);
        newVertices.Add(vert4);

        newUVs.Add(new Vector2(1, 0));
        newUVs.Add(new Vector2(1, 1));
        newUVs.Add(new Vector2(0, 1));
        newUVs.Add(new Vector2(0, 0));

        newTriangles.Add(2);
        newTriangles.Add(1);
        newTriangles.Add(0);

        newTriangles.Add(3);
        newTriangles.Add(2);
        newTriangles.Add(0);

        maze.vertices = newVertices.ToArray();
        maze.uv = newUVs.ToArray();
        maze.triangles = newTriangles.ToArray();

        return maze;
    }
}
