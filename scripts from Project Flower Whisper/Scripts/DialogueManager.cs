using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI nameText; // UI element to display the character's name
    public TextMeshProUGUI dialogueText; // UI element to display the dialogue text
    public Animator animator; // Animator for handling dialogue panel animations
    public GameObject optionsPanel; // Panel to display dialogue options
    public GameObject optionButtonPrefab; // Prefab for the dialogue option buttons
    public GameObject hintPanel; // Hint panel that shows before the dialogue starts
    public float typingSpeed = 0.05f; // Speed at which dialogue text is typed out
    public AudioSource audioSource; // Audio source component for playing sound effects
    public AudioClip dialogueSound; // Sound effect for playing during dialogue typing

    private Queue<string> sentences; // Queue to manage the dialogue sentences
    private bool playerInRange = false; // Flag to check if the player is in range to start dialogue
    private Dialogue initialDialogue; // Initial dialogue to start with
    private Dialogue currentDialogue; // The currently active dialogue

    void Start()
    {
        // Initialize the queue to hold sentences
        sentences = new Queue<string>();

        // Hide the options panel initially
        optionsPanel.SetActive(false);

        // Ensure the AudioSource is initialized
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }

        // Show the hint panel at the start
        if (hintPanel != null)
        {
            hintPanel.SetActive(true);
        }
    }

    void Update()
    {
        // Check if the player is in range and presses the 'E' key
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            // If the dialogue panel is open, display the next sentence
            if (animator.GetBool("IsOpen"))
            {
                DisplayNextSentence();
            }
            else
            {
                // Otherwise, start the dialogue with the initial dialogue
                StartDialogue(initialDialogue);

                // Hide the hint panel when the dialogue starts
                if (hintPanel != null)
                {
                    hintPanel.SetActive(false);
                }
            }
        }
    }

    // Plays a sound effect using the provided AudioClip
    public void PlaySoundEffect(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }

    // Starts the dialogue by displaying the first sentence
    public void StartDialogue(Dialogue dialogue)
    {
        currentDialogue = dialogue; // Set the current dialogue
        animator.SetBool("IsOpen", true); // Open the dialogue panel
        nameText.text = dialogue.characterName; // Display the character's name
        sentences.Clear(); // Clear any previous sentences

        // Enqueue all sentences from the dialogue
        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        // Display the first sentence
        DisplayNextSentence();
    }

    // Displays the next sentence in the queue
    public void DisplayNextSentence()
    {
        // If no sentences are left, handle the end of the dialogue
        if (sentences.Count == 0)
        {
            HandleEndOfDialogue();
            return;
        }

        // Play the dialogue typing sound effect
        PlaySoundEffect(dialogueSound);

        // Get the next sentence and start typing it out
        string sentence = sentences.Dequeue();
        StopAllCoroutines(); // Stop any ongoing typing coroutine
        StartCoroutine(TypeSentence(sentence));
    }

    // Types out each letter of the sentence with a delay
    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = ""; // Clear the dialogue text
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter; // Add one letter at a time
            yield return new WaitForSeconds(typingSpeed); // Wait for the specified typing speed
        }
    }

    // Handles what happens when the dialogue ends
    void HandleEndOfDialogue()
    {
        // Take action based on the end action defined in the current dialogue
        switch (currentDialogue.endAction)
        {
            case DialogueEndAction.TriggerEvent:
                currentDialogue.endEvent?.Invoke(); // Trigger the event
                EndDialogue(); // End the dialogue
                break;
            case DialogueEndAction.ExitDialogue:
                EndDialogue(); // Simply end the dialogue
                break;
            case DialogueEndAction.ShowOptions:
                DisplayOptions(currentDialogue.options); // Show dialogue options
                break;
        }
    }

    // Displays the dialogue options on the screen
    void DisplayOptions(DialogueOption[] options)
    {
        ClearOptions(); // Clear any existing options
        if (options != null && options.Length > 0)
        {
            optionsPanel.SetActive(true); // Show the options panel
            foreach (DialogueOption option in options)
            {
                // Only display options that are not hidden initially
                if (!option.isHiddenInitially)
                {
                    // Instantiate the option button and set its text
                    GameObject optionButton = Instantiate(optionButtonPrefab, optionsPanel.transform);
                    optionButton.GetComponentInChildren<TextMeshProUGUI>().text = option.optionText;
                    // Add a listener to handle the option selection
                    optionButton.GetComponent<Button>().onClick.AddListener(() => OnOptionSelected(option));
                }
            }
        }
        else
        {
            EndDialogue(); // End dialogue if there are no options
        }
    }

    // Clears all current options from the options panel
    void ClearOptions()
    {
        // Destroy all child objects of the options panel
        foreach (Transform child in optionsPanel.transform)
        {
            Destroy(child.gameObject);
        }
        optionsPanel.SetActive(false); // Hide the options panel
    }

    // Handles what happens when an option is selected
    public void OnOptionSelected(DialogueOption option)
    {
        option.optionEvent?.Invoke(); // Trigger the event associated with the option
        optionsPanel.SetActive(false); // Hide the options panel after selection

        // Decide what to do based on whether the option ends the dialogue
        if (option.exitDialogue || option.nextDialogue == null)
        {
            EndDialogue(); // End dialogue if specified
        }
        else if (option.showOptionsAfterSelection)
        {
            StartDialogue(option.nextDialogue); // Start the next dialogue if there are more options
        }
        else
        {
            StartDialogue(option.nextDialogue); // Start the next dialogue
        }
    }

    // Ends the dialogue by closing the panel and clearing the options
    void EndDialogue()
    {
        animator.SetBool("IsOpen", false); // Close the dialogue panel
        ClearOptions(); // Clear any remaining options
        if (hintPanel != null)
        {
            hintPanel.SetActive(true); // Re-enable the hint panel after dialogue ends
        }
    }

    // Sets whether the player is in range to start a dialogue
    public void SetPlayerInRange(bool inRange, Dialogue dialogue = null)
    {
        playerInRange = inRange; // Update the inRange flag
        if (inRange)
        {
            initialDialogue = dialogue; // Set the initial dialogue to the provided dialogue
            currentDialogue = dialogue; // Set the current dialogue to the initial dialogue
        }
        else
        {
            EndDialogue(); // End the dialogue if the player is out of range
            currentDialogue = initialDialogue; // Reset the current dialogue to the initial dialogue
        }
    }
}
