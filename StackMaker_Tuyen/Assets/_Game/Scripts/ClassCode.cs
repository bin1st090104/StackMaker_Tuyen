using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    public static T Instance
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<T>();
                if(instance == null)
                {
                    instance = new GameObject().AddComponent<T>();
                }
            }
            return instance;
        }
    }
}

public class Map 
{
    private int[,] matrix;
    public int[,] _matrix { get { return matrix; } set { matrix = value; } }

    private static int startX;
    public static int _startX { get { return startX; } set { startX = value; } }
    
    private static int startZ;
    public static int _startZ { get { return startZ; } set { startZ = value; } }

    private static int endX;
    public static int _endX { get { return endX; } set { endX = value; } }

    private static int endZ;
    public static int _endZ { get { return endZ; } set { endZ = value; } }

    private int sizeX;
    public int _sizeX { get { return sizeX; } set { sizeX = value; } }

    private int sizeZ;
    public int _sizeZ { get { return sizeZ; } set { sizeZ = value; } }

    public Map(int[][] matrix, int sizeX, int sizeZ)
    {
        this.matrix = new int[sizeX, sizeZ];
        this.sizeX = sizeX;
        this.sizeZ = sizeZ;
        for(int x = 0; x < sizeX; ++x)
        {
            for(int z = 0; z < sizeZ; ++z)
            {
                this.matrix[x,z] = matrix[x][z];
            }
        }
    }
}

public class ClassCode : MonoBehaviour
{
    private void Start()
    {
    }
}
