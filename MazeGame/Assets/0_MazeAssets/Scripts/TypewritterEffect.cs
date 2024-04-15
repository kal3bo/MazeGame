using System.Collections;
using TMPro;
using UnityEngine;

public class TypewriterEffect : MonoBehaviour
{
    [SerializeField, Tooltip("Typing Speed")]
    private float typingSpeed = 0.05f;

    [SerializeField, Tooltip("Array of messages to cycle through")]
    private string[] messages;

    [SerializeField, Tooltip("Delay between messages")]
    private float delayBetweenMessages = 2.0f;

    // Reference to the TMP_Text component
    private TMP_Text textComponent;

    // Wheather or not this text is active for typewritting effect.
    private bool activeTypewritter = true;

    public void SetTypewritterActive(bool active)
    {
        activeTypewritter = active;
    }

    /** Gets the TMP_Text component. */
    private void Awake()
    {
        textComponent = GetComponent<TMP_Text>();
    }

    private void Start()
    {
        StartCoroutine(CycleTextWithEffect());
    }

    IEnumerator CycleTextWithEffect()
    {
        // Loop as long as the effect is active:
        while (activeTypewritter)
        {
            foreach (string message in messages)
            {
                // Display each message
                yield return StartCoroutine(TypeText(message));

                // Wait before the next message
                yield return new WaitForSeconds(delayBetweenMessages);
            }
        }
    }

    IEnumerator TypeText(string text)
    {
        // Clear existing text:
        textComponent.text = "";
        foreach (char letter in text.ToCharArray())
        {
            // Add one letter at a time:
            textComponent.text += letter;

            // Wait before adding the next letter:
            yield return new WaitForSeconds(typingSpeed);
        }
    }
}
