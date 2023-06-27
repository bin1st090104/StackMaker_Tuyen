using UnityEngine;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject anim;
    [SerializeField] private GameObject[] playerBrick;
    [SerializeField] private GameObject[] materials;

    internal int tmp = 3;

    private GameObject[] playerBrickStack = new GameObject[100];
    private GameObject[,] currentMap;

    private int playerBrickStackSize = 0;

    private Vector3 playerBrickShift = new Vector3(0f, 0.1f, 0f);

    private Direct directMove;

    private Map[] map;
    private int numberOfMap = 2;
    [SerializeField] private int currenLevel = 0;
    private bool[,] visited;

    private Vector2 mouseStartPos;
    private Vector2 mouseEndPos;
    public enum Direct
    {
        Forward,
        Back,
        Right,
        Left,
        None
    }

    private int[] dx = { 0, 0, 1, -1 };
    private int[] dz = { 1, -1, 0, 0 };

    private int currentX;
    private int currentZ;

    private bool isRunning = false;
    private bool isWinning = false;

    [SerializeField] private float speed = 3f;

    public Player()
    {
    }

    private Direct AngleToDirect(float Angle)
    {
        if (Angle >= 45 && Angle < 135)
        {
            return Direct.Forward;
        }
        if (Angle >= 135 && Angle < 225)
        {
            return Direct.Left;
        }
        if (Angle >= 135 && Angle < 315)
        {
            return Direct.Back;
        }
        return Direct.Right;
    }
    void BuildMap(Map map)
    {
        int[,] matrix = map.matrix;
        int n = matrix.GetLength(0);
        int m = matrix.GetLength(1);
        currentMap = new GameObject[n, m];
        visited = new bool[n, m];
        for(int x = 0; x < n; ++x)
        {
            for(int z = 0; z < m; ++z)
            {
                if(matrix[x, z] == 0)
                {
                    map.pointStartX = x;
                    map.pointStartZ = z;
                }
                else
                if(matrix[x, z] == 1)
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
    void Start()
    {
        LoadMap();
        //CreateMap();
        BuildMap(map[currenLevel]);
    }
    // Update is called once per frame
    void LoadMap()
    {
        TextAsset text;
        numberOfMap = 0;
        while((text = Resources.Load<TextAsset>("map" + numberOfMap)) != null){
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
            for(int x = 0; x < rowMap.Length; ++x)
            {
                string[] str = rowMap[x].Split(',');
                matrixMap[x] = new int[str.Length];
                m = str.Length;
                Debug.Log(rowMap[x] + "   " + x + "   " + str.Length);
                for(int z = 0; z < str.Length; ++z)
                {
                    matrixMap[x][z] = str[z][0] - '0';
                }
            }
            map[i] = new Map(matrixMap, n, m);
            Debug.Log(matrixMap.ToString());
        }
    }
    void CreateMap()
    {
        /*-1 : Nothing
         * 0 : StartPoint
         * 1 : FinishBlock
         * 2 : BrickBlock
         * 3 : UnbrickBlock
         * 4 : Wall 
         */
        //map[0] = new Map();
        //map[0].matrix = new int[,]{
        //    { 4,4,4,4,4,4,4,4,4,4 },
        //    { 4,0,4,2,2,2,2,2,2,4 },
        //    { 4,2,4,2,4,4,4,4,2,4 },
        //    { 4,2,4,3,4,1,2,4,3,4 },
        //    { 4,2,4,3,4,4,2,4,2,4 },
        //    { 4,2,4,2,4,4,2,4,2,4 },
        //    { 4,2,4,2,2,2,2,4,2,4 },
        //    { 4,3,4,4,4,4,4,4,2,4 },
        //    { 4,2,2,2,2,2,2,2,2,4 },
        //    { 4,4,4,4,4,4,4,4,4,4 }
        //};
        //map[1] = new Map();
        //map[1].matrix = new int[,]
        //{
        //    { 4,4,4,4,4,4,4,4,4,4 },
        //    { 4,3,2,2,2,0,4,4,1,4 },
        //    { 4,2,4,4,4,4,4,4,2,4 },
        //    { 4,2,4,2,4,4,4,4,2,4 },
        //    { 4,2,4,3,4,4,4,4,2,4 },
        //    { 4,2,4,2,4,4,4,4,2,4 },
        //    { 4,3,2,2,4,4,4,4,2,4 },
        //    { 4,4,4,2,4,4,4,4,2,4 },
        //    { 4,4,4,2,2,3,3,3,2,4 },
        //    { 4,4,4,4,4,4,4,4,4,4 }
        //};
    }
    void Update()
    {
        Control();
    }
    private void AddBrick()
    {
        playerBrickStack[playerBrickStackSize++] = Instantiate(playerBrick[playerBrickStackSize & 1], transform);
        playerBrickStack[playerBrickStackSize - 1].transform.position += (playerBrickStackSize - 1) * playerBrickShift;
        anim.transform.position += playerBrickShift;
    }
    private void RemoveBrick()
    {
        if(playerBrickStackSize > 0)
        {
            --playerBrickStackSize;
            Destroy(playerBrickStack[playerBrickStackSize]);
            anim.transform.position -= playerBrickShift;
        }
    }
    private void ClearBrick()
    {
        while(playerBrickStackSize > 0)
        {
            RemoveBrick();
        }
    }
    private void ChangeLevel()
    {
        if(currenLevel + 1 == numberOfMap)
        {
            isWinning = true;
            return;
        }
        ClearBrick();
        isRunning = false;
        int n = currentMap.GetLength(0);
        int m = currentMap.GetLength(1);
        for(int i = 0; i < n; ++i)
        {
            for(int j = 0; j < m; ++j)
            {
                Destroy(currentMap[i,j]);
            }
        }
        ++currenLevel;
        BuildMap(map[currenLevel]);
    }
    private void Control()
    {
        if (isWinning)
        {
            return;
        }
        if (isRunning)
        {
            int nextX = currentX + dx[(int)directMove];
            int nextZ = currentZ + dz[(int)directMove];
            Vector3 nextP = Vector3.MoveTowards(transform.position, new Vector3(nextX, transform.position.y, nextZ), speed * Time.deltaTime);
            if (Mathf.Abs(nextP.x - nextX) < 1e-6 && Mathf.Abs(nextP.z - nextZ) < 1e-6)
            {
                currentX = nextX;
                currentZ = nextZ;
                if (map[currenLevel].matrix[currentX, currentZ] == 2 && !visited[currentX,currentZ])
                {
                    visited[currentX,currentZ] = true;
                    Destroy(currentMap[currentX, currentZ]);
                    currentMap[currentX, currentZ] = Instantiate(materials[5]);
                    currentMap[currentX, currentZ].transform.position = new Vector3(currentX, -0.5f, currentZ);
                    AddBrick();
                }
                else
                if (map[currenLevel].matrix[currentX, currentZ] == 3 && !visited[currentX, currentZ])
                {
                    visited[currentX, currentZ] = true;
                    Destroy(currentMap[currentX, currentZ]);
                    currentMap[currentX, currentZ] = Instantiate(materials[2]);
                    currentMap[currentX, currentZ].transform.position = new Vector3(currentX, -0.5f, currentZ);
                    RemoveBrick();
                }
                else
                if (map[currenLevel].matrix[currentX, currentZ] == 1)
                {
                    ChangeLevel();
                    return;
                }
                nextP = new Vector3(currentX, transform.position.y, currentZ);
                nextX = currentX + dx[(int)directMove];
                nextZ = currentZ + dz[(int)directMove];
                if (map[currenLevel].matrix[nextX, nextZ] == 4)
                {
                    isRunning = false;
                }
            }
            transform.position = nextP;   
            return;
        }
        if (Input.GetMouseButtonDown(0))
        {
            mouseStartPos = Input.mousePosition;
        }
        else
        if (Input.GetMouseButtonUp(0))
        {
            mouseEndPos = Input.mousePosition;
            Vector2 horizontal = new Vector2(1, 0);
            Vector2 direct = mouseEndPos - mouseStartPos;
            if(direct.magnitude < 1e-3)
            {
                return;
            }
            float angle = Vector2.Angle(new Vector2(1, 0), mouseEndPos - mouseStartPos);
            if (direct.y < 0)
            {
                angle = 360f - angle;
            }
            directMove = AngleToDirect(angle);
            Debug.Log(directMove);
            int nextX = currentX + dx[(int)directMove];
            int nextZ = currentZ + dz[(int)directMove];
            if (map[currenLevel].matrix[nextX, nextZ] == 4)
            {
                return;
            }
            isRunning = true;
        }
        return;
    }
}
