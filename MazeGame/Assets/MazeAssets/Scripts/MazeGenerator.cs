using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    // Exposed variables:
    [SerializeField, Tooltip("Maze Size in X.")]
    private int mazeWidth = 10;

    [SerializeField, Tooltip("Maze Size in Y.")]
    private int mazeHeight = 10;

    [SerializeField, Tooltip("Wall Prefab to Build Maze.")]
    private GameObject wallPrefab;

    [SerializeField, Tooltip("GameObject at Entry Location.")]
    private GameObject entryPrefab;

    [SerializeField, Tooltip("GameObject at Exit Location.")]
    private GameObject exitPrefab;

    [SerializeField, Tooltip("Floor Prefab Underneath Maze.")]
    private GameObject floorPrefab;

    [SerializeField, Tooltip("Seed to Generate Maze.")]
    private int seed = 12345;

    // Private variables:
    private bool[,] visited;
    private int actualWidth;
    private int actualHeight;
    private int floorExtraSize = 10;

    /** Start is called before the first frame update. It initializes the maze generation and floor building processes. */
    void Start()
    {
        actualWidth = mazeWidth * 2 + 1;
        actualHeight = mazeHeight * 2 + 1;

        GenerateMaze();
        BuildFloor();
    }

    /** Generates the maze layout using a Depth-First Search algorithm and marks all cells as unvisited. */
    void GenerateMaze()
    {
        visited = new bool[actualWidth, actualHeight];
        Random.InitState(seed);

        // Initialize all cells as unvisited
        for (int x = 0; x < actualWidth; x++)
        {
            for (int y = 0; y < actualHeight; y++)
            {
                visited[x, y] = false;
            }
        }

        // Start the maze generation from the bottom-left corner
        DFS(1, 1);
        // Instantiate maze walls, entry, and exit based on the visited array
        InstantiateMaze();
    }

    /** Instantiates walls, entry, and exit prefabs in the maze based on the DFS visited array. */
    void InstantiateMaze()
    {
        for (int x = 0; x < actualWidth; x++)
        {
            for (int y = 0; y < actualHeight; y++)
            {
                Vector3 position = new Vector3(x, 0, y);
                if (x == 1 && y == 0)
                {
                    Instantiate(entryPrefab, position, Quaternion.identity);
                }

                else if (x == actualWidth - 2 && y == actualHeight - 1)
                {
                    Instantiate(exitPrefab, position, Quaternion.identity);
                }

                else if (!visited[x, y])
                {
                    Instantiate(wallPrefab, position, Quaternion.identity);
                }
            }
        }
    }

    /** Builds a floor underneath the maze with extra size around the edges. */
    void BuildFloor()
    {
        Vector3 scale = new Vector3(actualWidth + floorExtraSize, actualHeight + floorExtraSize, 1);
        Vector3 position = new Vector3(actualWidth / 2.0f - 0.5f, -0.5f, actualHeight / 2.0f - 0.5f);
        Quaternion rotation = Quaternion.Euler(90, 0, 0);

        GameObject floor = Instantiate(floorPrefab, position, rotation);
        floor.transform.localScale = scale;
    }

    /** Depth-First Search algorithm used to generate the maze paths. */
    void DFS(int x, int y)
    {
        visited[x, y] = true;
        int[,] directions = { { 0, 2 }, { 2, 0 }, { 0, -2 }, { -2, 0 } };
        ShuffleArray(directions);

        for (int i = 0; i < 4; i++)
        {
            int nx = x + directions[i, 0];
            int ny = y + directions[i, 1];

            if (IsInBounds(nx, ny) && !visited[nx, ny])
            {
                visited[x + directions[i, 0] / 2, y + directions[i, 1] / 2] = true;
                DFS(nx, ny);
            }
        }
    }

    /** Checks if a given cell is within the bounds of the maze array. */
    bool IsInBounds(int x, int y)
    {
        return x > 0 && y > 0 && x < actualWidth - 1 && y < actualHeight - 1;
    }

    /** Randomly shuffles an array of directions to provide varied maze paths. */
    void ShuffleArray(int[,] array)
    {
        for (int i = 0; i < array.GetLength(0); i++)
        {
            int rand = Random.Range(0, i + 1);
            int tempX = array[i, 0];
            int tempY = array[i, 1];
            array[i, 0] = array[rand, 0];
            array[i, 1] = array[rand, 1];
            array[rand, 0] = tempX;
            array[rand, 1] = tempY;
        }
    }
}
