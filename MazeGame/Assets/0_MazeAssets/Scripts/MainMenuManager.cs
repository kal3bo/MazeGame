using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField, Tooltip("Button to start playing.")]
    private GameObject playButton;

    [SerializeField, Tooltip("Button to continue the last played level.")]
    private GameObject continueButton;

    [SerializeField, Tooltip("Button to start over the game.")]
    private GameObject startOverButton;

    [SerializeField, Tooltip("Slider for selecting the level.")]
    private Slider levelSlider;

    [SerializeField, Tooltip("Text display for the slider value.")]
    private TextMeshProUGUI sliderSizeText;

    [SerializeField, Tooltip("Input field for entering a custom game seed.")]
    private TMP_InputField inputSeed;

    [SerializeField, Tooltip("Text for displaying seed input instructions or errors.")]
    private TextMeshProUGUI seedInstructions;

    // Key for storing current level in PlayerPrefs
    private const string LEVEL_KEY = "CurrentLevel";

    void Awake()
    {
        // Ensure the button states are updated based on saved progression:
        CheckProgression();

        // Bind the update function to the slider value change event
        levelSlider.onValueChanged.AddListener(UpdateSliderValue);
    }

    /** Function to start the game and load the MazeGame scene. */
    public void StartGame()
    {
        SceneManager.LoadScene("MazeGame");
    }

    /** Attempts to start a new game with a custom seed. Validates the seed input first. */
    public void TryStartNewCustomGame()
    {
        if (inputSeed.text.Length == 5 && int.TryParse(inputSeed.text, out int seed))
        {
            // Update level based on validated seed:
            UpdatePlayerLevel(seed);

            // Update the display text:
            sliderSizeText.text = inputSeed.text;
            StartGame();
        }
        else
        {
            // If input seed was invalid -> display error message:
            seedInstructions.text = "Error: Please provide a 5 digit number.";
        }
    }

    /** Updates the CurrentLevel in PlayerPrefs to a new level specified by the caller. */
    public void UpdatePlayerLevel(int newLevel)
    {
        PlayerPrefs.SetInt(LEVEL_KEY, newLevel);
    }

    /** Function to quit the game. */
    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // Stop playing in the editor
#else
        Application.Quit(); // Quit the application
#endif
    }

    /** Accessible method to check if the player has saved progression and update button states accordingly. */
    private void CheckProgression()
    {
        if (PlayerPrefs.HasKey(LEVEL_KEY))
        {
            int currentLevel = PlayerPrefs.GetInt(LEVEL_KEY);
            if (currentLevel > 0)
            {
                // Player has progression, enable Continue and Start Over buttons, disable Play button:
                playButton.SetActive(false);
                continueButton.SetActive(true);
                startOverButton.SetActive(true);

                // Exit the function early as no further action is needed:
                return;
            }
        }

        // If no progression found or level is <= 0, enable Play button and disable others:
        playButton.SetActive(true);
        continueButton.SetActive(false);
        startOverButton.SetActive(false);
    }

    /** Dynamically updates the slider text to the current value of the slider. */
    private void UpdateSliderValue(float value)
    {
        sliderSizeText.text = value.ToString("0");
    }
}

