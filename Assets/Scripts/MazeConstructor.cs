using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeConstructor : MonoBehaviour
{
    public bool showDebug;

    [SerializeField] private Material mazeMat1;
    [SerializeField] private Material mazeMat2;
    [SerializeField] private Material startMat;
    [SerializeField] private Material treasureMat;
    // script ref
    private MazeMeshGenerator meshGenerator;
    private MazeDataGenerator dataGenerator;


    public float hallWidth
    {
        get; private set;
    }
    public float hallHeight
    {
        get; private set;
    }

    public int startRow
    {
        get; private set;
    }
    public int startCol
    {
        get; private set;
    }

    public int goalRow
    {
        get; private set;
    }
    public int goalCol
    {
        get; private set;
    }


    public int[,] data
    {
        get; private set;
    }

    private void Awake()
    {
        dataGenerator = new MazeDataGenerator();
        meshGenerator = new MazeMeshGenerator();
        // default walls surrounding a single empty cell
        // empty cell is area the player can move around in, dont generate a mesh here
        data = new int[,]
        {
            {1,1,1},
            {1,0,1},
            {1,1,1}
        };
    }
    // parameters data taken form gamecontroller
    public void GenerateNewMaze(int sizeRows, int sizeCols,
    TriggerEventHandler startCallback = null, TriggerEventHandler goalCallback = null)
    {
        // if the ints are divisible by 2, log an error & reduce by one to prevent errors
        if (sizeRows % 2 == 0 )
        {          
            sizeRows--;
            Debug.Log(sizeRows + " Odd numbers work better for dungeon size.");
        }
        if (sizeCols % 2 == 0)
        {          
            sizeCols--;
            Debug.Log(sizeCols + " Odd numbers work better for dungeon size.");
        }
        // run func
        DisposeOldMaze();
        // data is equal to the fromdimensions func output with the sizerows and sizecols parameters from gamecontroller
        data = dataGenerator.FromDimensions(sizeRows, sizeCols);

        FindStartPosition();
        FindGoalPosition();

        // store values used to generate this mesh
        hallWidth = meshGenerator.width;
        hallHeight = meshGenerator.height;
        // run function that adds colliders, meshrenderers etc to new gameobjects
        DisplayMaze();

        PlaceStartTrigger(startCallback);
        PlaceGoalTrigger(goalCallback);
    }

    private void OnGUI()
    {
        // return if false
        if (!showDebug)
        {
            return;
        }

        int[,] maze = data;
        int rMax = maze.GetUpperBound(0);
        int cMax = maze.GetUpperBound(1);

        string msg = "";
        // nested for loop
        // i(rmax) is more or = to 0 --
        for (int i = rMax; i >= 0; i--)
        {
            // j is less or = cmax ++
            for (int j = 0; j <= cMax; j++)
            {
                // i and j are 0
                if (maze[i, j] == 0)
                {
                    msg += "....";
                }
                // else
                else
                {
                    msg += "==";
                }
            }
            // new line
            msg += "\n";
        }
        // create a gui message on screen at 20 20 size 500 500
        GUI.Label(new Rect(20, 20, 500, 500), msg);
    }

    private void DisplayMaze()
    {
        // new gameobject
        GameObject go = new GameObject();
        // set name and tag
        go.transform.position = Vector3.zero;
        go.name = "Procedural Maze";
        go.tag = "Generated";
        // apply mesh and meshfilter
        MeshFilter mf = go.AddComponent<MeshFilter>();
        mf.mesh = meshGenerator.FromData(data);
        // add collider
        MeshCollider mc = go.AddComponent<MeshCollider>();
        mc.sharedMesh = mf.mesh;
        // add meshrenderer and set material
        MeshRenderer mr = go.AddComponent<MeshRenderer>();
        mr.materials = new Material[2] { mazeMat1, mazeMat2 };
    }

    public void DisposeOldMaze()
    {
        // destroy al previously generated objects
        GameObject[] objects = GameObject.FindGameObjectsWithTag("Generated");
        foreach (GameObject go in objects)
        {
            Destroy(go);
        }
    }
    // start at 0,0 and search through maze data to find an open area to store the start position in
    void FindStartPosition()
    {       
        // array? = data
        int[,] maze = data;
        int rMax = maze.GetUpperBound(0);
        int cMax = maze.GetUpperBound(1);
        // nested for loop for searching
        for (int i = 0; i <= rMax; i++)
        {
            for (int j = 0; j <= cMax; j++)
            {
                if (maze[i,j] == 0)
                {
                    startRow = 1;
                    startCol = j;
                    return;
                }
            }
        }
    }
    // start at max values and search backwards to find an open space to store the goal position in
    void FindGoalPosition()
    {
        // get data
        int[,] maze = data;
        int rMax = maze.GetUpperBound(0);
        int cMax = maze.GetUpperBound(1);
        // nested for loop for searching
        for (int i = rMax; i >= 0; i--)
        {
            for (int j = cMax; j >= 0; j--)
            {
                if (maze[i,j]==0)
                {
                    goalRow = 1;
                    goalCol = j;
                    return;
                }
            }
        }
    }
    // intantiate 
    void PlaceStartTrigger(TriggerEventHandler callback)
    {
        GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
        go.transform.position = new Vector3(startCol * hallWidth, .5f, startRow * hallWidth);
        go.name = "Start Trigger";
        go.tag = "Generated";

        go.GetComponent<BoxCollider>().isTrigger = true;
        go.GetComponent<MeshRenderer>().sharedMaterial = startMat;

        TriggerEventRouter tc = go.AddComponent<TriggerEventRouter>();
        tc.callback = callback;
    }

    private void PlaceGoalTrigger(TriggerEventHandler callback)
    {
        GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
        go.transform.position = new Vector3(goalCol * hallWidth, .5f, goalRow * hallWidth);
        go.name = "Treasure";
        go.tag = "Generated";

        go.GetComponent<BoxCollider>().isTrigger = true;
        go.GetComponent<MeshRenderer>().sharedMaterial = treasureMat;

        TriggerEventRouter tc = go.AddComponent<TriggerEventRouter>();
        tc.callback = callback;
    }
}
