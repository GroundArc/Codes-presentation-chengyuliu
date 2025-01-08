using UnityEngine;

public class DialogueTriggerZone : MonoBehaviour
{
    public Dialogue dialogue;
    public Collider triggerCollider;


    private DialogueManager dialogueManager;

    private void Start()
    {
        dialogueManager = FindObjectOfType<DialogueManager>();
        if (dialogueManager == null)
        {
            Debug.LogError("DialogueManager not found!");
        }

        if (triggerCollider == null)
        {
            Debug.LogError("Trigger Collider not assigned!");
        }
        else
        {
            triggerCollider.isTrigger = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            dialogueManager.SetPlayerInRange(true, dialogue);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            dialogueManager.SetPlayerInRange(false);
        }
    }
}
