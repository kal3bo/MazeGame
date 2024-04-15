using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] private PlayerTrigger entranceTrigger;
    [SerializeField] private PlayerTrigger exitTrigger1;
    [SerializeField] private PlayerTrigger exitTrigger2;

    [SerializeField] private GameObject instructionsUI;
    [SerializeField] private GameObject winMenuUI;
    [SerializeField] private TextMeshProUGUI currentLevelText;

    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private PlayerCamera playerCamera;

    private bool playerEnteredMaze = false;
    private bool playerExitMaze = false;
    private bool firstLevel = false;

    /** Setting instructions on Start if player is in the first level. */
    private void Start()
    {
        // If this is the first level, show instructions:
        if(PlayerPrefs.GetInt("CurrentLevel", 0) == 0)
        {
            instructionsUI.SetActive(true);
            firstLevel = true;
        }
    }

    /** Binding trigger overlapp functions. */
    private void OnEnable()
    {
        if (entranceTrigger != null)
        {
            entranceTrigger.OnPlayerEnterTrigger += EnterMaze;
        }
        if (exitTrigger1 != null)
        {
            exitTrigger1.OnPlayerEnterTrigger += ExitMaze;
        }
        if (exitTrigger2 != null)
        {
            exitTrigger2.OnPlayerEnterTrigger += ExitMaze;
        }
    }

    /** Disabling overlap functions from triggers when this GameObject is disabled. */
    private void OnDisable()
    {
        if (entranceTrigger != null)
        {
            entranceTrigger.OnPlayerEnterTrigger -= EnterMaze;
        }
        if (exitTrigger1 != null)
        {
            exitTrigger1.OnPlayerEnterTrigger -= ExitMaze;
        }
        if (exitTrigger2 != null)
        {
            exitTrigger2.OnPlayerEnterTrigger -= ExitMaze;
        }
    }

    /** Detecting when the player entered the maze.
        If this is the first level, hide instructions. */
    private void EnterMaze(Collider player)
    {
        if (!playerEnteredMaze)
        {
            playerEnteredMaze = true;

            if (firstLevel)
            {
                instructionsUI.SetActive(false);
            }
        }
    }

    /** Detecting when the player exits the maze.
        Showing UI to return to Main Menu or start next Level. */
    private void ExitMaze(Collider player)
    {
        if (!playerExitMaze)
        {
            winMenuUI.SetActive(true);
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
            if (playerMovement != null && playerCamera != null)
            {
                playerMovement.enabled = false;
                playerCamera.enabled = false;

                // Setting new level:
                int currentLevel = PlayerPrefs.GetInt("CurrentLevel", 0);
                PlayerPrefs.SetInt("CurrentLevel", currentLevel + 1);
                currentLevelText.text = "Level " + (currentLevel +1).ToString();

                int newSeed = Random.Range(10000, 100000);
                PlayerPrefs.SetInt("CurrentSeed", newSeed);
            }
            playerExitMaze = true;
        }
    }

    public void StartNextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
