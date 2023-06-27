using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : Singleton<Map>
{
    public int[,] matrix;
    public int pointStartX, pointStartZ;
    public int pointEndX, pointEndZ;
    public Map(int[][] matrixMap, int n, int m)
    {
        matrix = new int[n, m];
        Debug.Log("n = " + n + " m = " + m);
        for (int x = 0; x < n; ++x)
        {
            string str = "";
            for (int z = 0; z < m; ++z)
            {
                matrix[x, z] = matrixMap[x][z];
                str += "," + matrix[x, z];
            }
            Debug.Log(str);
        }
    }
}
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
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

public class Manager : MonoBehaviour
{
    // Start is called before the first frame update
    private int numberOfMap;
    private Map[] map;

    [SerializeField] private int currentLevel;

    private GameObject[,] currentMap;
    private bool[,] visited;
    private GameObject[] materials;
   
    private int currentX;
    private int currentZ;

    public GameObject[,] getCurrentMap
    {
        get
        {
            return currentMap;
        }
    }
    public bool[,] getVisited
    {
        get
        {
            return visited;
        }
    }
    public GameObject[] getMaterials
    {
        get
        {
            return materials;
        }
    }
    public int getCurrentX
    {
        get
        {
            return currentX;
        }
    }
    public int getCurrentZ
    {
        get
        {
            return currentZ;
        }
    }
    void Start()
    {
        LoadMap();
    }

    void BuildMap(Map map)
    {
        int[,] matrix = map.matrix;
        int n = matrix.GetLength(0);
        int m = matrix.GetLength(1);
        currentMap = new GameObject[n, m];
        visited = new bool[n, m];
        for (int x = 0; x < n; ++x)
        {
            for (int z = 0; z < m; ++z)
            {
                if (matrix[x, z] == 0)
                {
                    map.pointStartX = x;
                    map.pointStartZ = z;
                }
                else
                if (matrix[x, z] == 1)
                {
                    map.pointEndX = x;
                    map.pointEndZ = z;
                }
                currentMap[x, z] = Instantiate(materials[matrix[x, z]]);
                currentMap[x, z].transform.position = new Vector3(x, -0.5f, z);
            }
        }
        transform.position = new Vector3(map.pointStartX, transform.position.y, map.pointStartZ);
        currentX = map.pointStartX;
        currentZ = map.pointStartZ;
    }

    void LoadMap()
    {
        TextAsset text;
        numberOfMap = 0;
        while ((text = Resources.Load<TextAsset>("map" + numberOfMap)) != null)
        {
            ++numberOfMap;
        }
        Debug.Log("number of map = " + numberOfMap);
        map = new Map[numberOfMap];
        for (int i = 0; i < numberOfMap; ++i)
        {
            string strMap = Resources.Load<TextAsset>("map" + i).ToString();
            string[] rowMap = strMap.Split(new string("\r\n"));
            int[][] matrixMap = new int[rowMap.Length][];
            int n = rowMap.Length;
            int m = 0;
            Debug.Log("map = " + i);
            for (int x = 0; x < rowMap.Length; ++x)
            {
                string[] str = rowMap[x].Split(',');
                matrixMap[x] = new int[str.Length];
                m = str.Length;
                Debug.Log(rowMap[x] + "   " + x + "   " + str.Length);
                for (int z = 0; z < str.Length; ++z)
                {
                    matrixMap[x][z] = str[z][0] - '0';
                }
            }
            map[i] = new Map(matrixMap, n, m);
            Debug.Log(matrixMap.ToString());
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
