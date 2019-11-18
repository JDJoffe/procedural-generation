using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeDataGenerator 
{
    public float placementThreshold;

    public MazeDataGenerator()
    {
        placementThreshold = .1f;
    }
    // array?  takes parameters from mazeconstructor which are taken from gamecontroller
    public int[,] FromDimensions(int sizeRows, int sizeCols)
    {
        int[,] maze = new int[sizeRows, sizeCols];
        // return the last index in the first dimension of the array i think 
        int rMax = maze.GetUpperBound(0);
       //?
        int cMax = maze.GetUpperBound(1);

        // nested for loop 
        for (int i = 0; i <= rMax; i++)
        {
            for (int j = 0; j <= cMax; j++)
            {
                // if either i or j == 0 or i/j == maze upperbound
                if (i == 0 || j == 0 || i == rMax || j == cMax)
                {
                    // maze (i,j) all together = 1
                    maze[i, j] = 1;
                }
                else if (i % 2 == 0 && j % 2 == 0)
                {
                    // if random.value is over .1 which is a 90% chance
                    if (Random.value > placementThreshold)
                    {
                        maze[i, j] = 1;
                        // ternary operators
                        // if the random value is under .5  then a is 0, or a is the result of another 50% chance then it will be either -1 or 1
                        int a = Random.value < 0.5 ? 0 : (Random.value < 0.5 ? -1 : 1);
                        // if b is not 0 then b is 0 or the result of a 50% chance to be -1 or 1
                        int b = a != 0 ? 0 : (Random.value < 0.5 ? -1 : 1);
                        // maze int array = (i + a and j + b) all together = 1
                        maze[i + a, j + b] = 1;
                    }
                }

            }
        }
        // return array? int
        return maze;
    }
}
