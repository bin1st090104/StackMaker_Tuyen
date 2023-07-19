using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    [SerializeField] private GameObject[] materials;

    private Map[] map;
    public Map[] _map { get { return map; } set { map = value; } }

    private static Map currentMap;
    public static Map _currentMap { get { return currentMap; } set { currentMap = value; } }

    private int[,] matrix;
    public int[,] _matrix { get { return matrix; } set { matrix = value; } }

    private static GameObject[,] gameObjectMatrix;
    public static GameObject[,] _gameObjectMatrix { get { return gameObjectMatrix; } set { gameObjectMatrix = value; } }

    private int numberOfMap;
    public int _numberOfMap { get { return numberOfMap; } set { numberOfMap = value; } }

    [SerializeField] private int currentLevel = 0;
    public void LoadMap(int i)
    {
        string stringMap = Resources.Load<TextAsset>("map" + i).ToString();
        Debug.Log("i = " + i + ":\n" + stringMap);
        string[] rowMap = stringMap.Split("\r\n");
        int sizeX = rowMap.Length;
        int sizeZ = 0;
        int[][] matrix = new int[sizeX][];
        for(int x = 0; x < sizeX; ++x)
        {
            string[] row = rowMap[x].Split(',');
            sizeZ = row.Length;
            matrix[x] = new int[sizeZ];
            for(int z = 0; z < sizeZ; ++z)
            {
                matrix[x][z] = row[z][0] - '0';
            }
        }
        map[i] = new Map(matrix, sizeX, sizeZ);
        for(int x = 0; x < map[i]._sizeX; ++x)
        {
            string str = "[ ";
            for(int z = 0; z < map[i]._sizeZ; ++z)
            {
                str += "" + map[i]._matrix[x, z] + ' ';
            }
            str += ']';
            Debug.Log(str);
        }
        return;
    }
    public void LoadMap()
    {
        numberOfMap = 0;
        while(Resources.Load<TextAsset>("map" + numberOfMap) != null)
        {
            ++numberOfMap;
        }
        Debug.Log(numberOfMap);
        map = new Map[numberOfMap];
        for(int i = 0; i < numberOfMap; ++i)
        {
            LoadMap(i);
        }
    }

    void BuildMap()
    {
        Map currentMap = map[currentLevel];
        matrix = currentMap._matrix;
        int startX, endX;
        int startZ, endZ;
        for(int x = 0; x < currentMap._sizeX; ++x)
        {
            for(int z = 0; z < currentMap._sizeZ; ++z)
            {
                if (matrix[x, z] == 0)
                {
                    startX = x;
                    startZ = z;
                }
                else
                if (matrix[x, z] == 1)
                {
                    endX = x;
                    endZ = z;
                }
                gameObjectMatrix[x, z] = Instantiate(materials[matrix[x, z]]);
                gameObjectMatrix[x, z].transform.position = new Vector3(x, -0.5f, z);
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        LoadMap();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
