using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    [SerializeField, Tooltip("Wall Prefab to Build Maze.")]
    private GameObject wallPrefab;

    [SerializeField, Tooltip("GameObject at Entry Location.")]
    private GameObject entryPrefab;

    [SerializeField, Tooltip("GameObject at First Exit Location.")]
    private GameObject exitPrefab;

    [SerializeField, Tooltip("Floor Prefab Underneath Maze.")]
    private GameObject floorPrefab;

    // Maze Settings:
    private int mazeWidth;
    private int mazeHeight;
    private int seed;

    // Maze Generator Variables:
    private bool[,] visited;
    private int actualWidth;
    private int actualHeight;

    private int floorExtraSize = 10;

    /** Get Player Progression. */
    private void Awake()
    {
        // Get the current level from player preferences:
        int currentLevel = PlayerPrefs.GetInt("CurrentLevel", 0);
        mazeWidth = 10 + currentLevel;
        mazeHeight = 10 + currentLevel;

        // Get the current seed from player preferences:
        int currentSeed = PlayerPrefs.GetInt("CurrentSeed", 0);
        if (currentSeed >= 10000 && currentSeed <= 99999)
        {
            seed = currentSeed;
        }

        else
        {
            // Generate a random 5-digit number for seed:
            seed = Random.Range(10000, 100000);
        }

        actualWidth = mazeWidth * 2 + 1;
        actualHeight = mazeHeight * 2 + 1;
    }

    private void Start()
    {
        GenerateMaze();
        BuildFloor();
    }

    /** Initializes a maze by setting up a grid of unvisited cells. */
    void GenerateMaze()
    {
        visited = new bool[actualWidth, actualHeight];
        Random.InitState(seed);

        for (int x = 0; x < actualWidth; x++)
        {
            for (int y = 0; y < actualHeight; y++)
            {
                visited[x, y] = false;
            }
        }

        DFS(1, 1);
        InstantiateMaze();
    }

    /** Iterates through a grid to place entry and exit points at specified coordinates and fills in the unvisited cells with wall prefabs */
    void InstantiateMaze()
    {
        for (int x = 0; x < actualWidth; x++)
        {
            for (int y = 0; y < actualHeight; y++)
            {
                Vector3 position = new Vector3(x, 0, y);

                // Entry location at bottom left:
                if (x == 1 && y == 0)
                {
                    Instantiate(entryPrefab, position, Quaternion.identity);
                }

                // First exit location at top right:
                else if (x == actualWidth - 2 && y == actualHeight - 1)
                {
                    Instantiate(exitPrefab, position, Quaternion.identity);
                }

                // Second exit location at top left:
                else if (x == 1 && y == actualHeight - 1)
                {
                    Instantiate(exitPrefab, position, Quaternion.identity);
                }

                // Place walls where not visited:
                else if (!visited[x, y])
                {
                    Instantiate(wallPrefab, position, Quaternion.identity);
                }
            }
        }
    }

    /** Instantiates and scales a corresponding plane as floor. */
    void BuildFloor()
    {
        Vector3 scale = new Vector3(actualWidth + floorExtraSize, actualHeight + floorExtraSize, 1);
        Vector3 position = new Vector3(actualWidth / 2.0f - 0.5f, -0.5f, actualHeight / 2.0f - 0.5f);
        Quaternion rotation = Quaternion.Euler(90, 0, 0);

        GameObject floor = Instantiate(floorPrefab, position, rotation);
        floor.transform.localScale = scale;
    }

    /** Performs a Depth-First Search (DFS) to generate the maze by recursively exploring and marking cells as visited, 
     * using randomized directions to ensure varied maze pathways. */
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

    /** Checks whether the provided coordinates (x, y) are within the valid boundaries of the maze grid. */
    bool IsInBounds(int x, int y)
    {
        return x > 0 && y > 0 && x < actualWidth - 1 && y < actualHeight - 1;
    }

    /** Shuffles the elements of a 2D integer array in-place to randomize the order. */
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
