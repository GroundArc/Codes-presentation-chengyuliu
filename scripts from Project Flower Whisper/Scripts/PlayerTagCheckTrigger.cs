using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerTagCheckTrigger : MonoBehaviour
{
    public GameObject targetObject; // 要检查的目标对象
    public Dialogue dialogueCorrectFound; // 找到特定Tag的子对象时触发的对话
    public Dialogue dialogueNotFound; // 未找到特定Tag的子对象时触发的对话
    public Collider triggerCollider;
    public float npcEventId; // NPC 可触发的事件 ID

    public UnityEvent onTagFound; // 找到特定Tag时触发的事件
    public UnityEvent onTagNotFound; // 未找到特定Tag时触发的事件

    private DialogueManager dialogueManager;
    private bool playerInRange = false;

    private void Start()
    {
        dialogueManager = FindObjectOfType<DialogueManager>();
        if (dialogueManager == null)
        {
            Debug.Log("DialogueManager not found!");
        }

        if (triggerCollider == null)
        {
            Debug.Log("Trigger Collider not assigned!");
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
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.G))
        {
            HandlePlayerAction();
        }
    }

    private void HandlePlayerAction()
    {
        if (targetObject == null)
        {
            Debug.LogError("Target object not assigned!");
            return;
        }

        bool eventIdFound = CheckForEventIdInChildren(targetObject.transform);

        if (eventIdFound)
        {
            onTagFound?.Invoke(); // 触发事件
            dialogueManager.StartDialogue(dialogueCorrectFound);
        }
        else
        {
            onTagNotFound?.Invoke(); // 触发事件
            dialogueManager.StartDialogue(dialogueNotFound);
        }
    }

    private bool CheckForEventIdInChildren(Transform parent)
    {
        foreach (Transform child in parent)
        {
            if (child.CompareTag("Flower"))
            {
                Debug.Log("Found a Flower child: " + child.name);

                FlowerLanguage flowerLanguage = child.GetComponent<FlowerLanguage>();
                if (flowerLanguage != null)
                {
                    Debug.Log("Found FlowerLanguage component on: " + child.name);
                    Debug.Log("Current event ID: " + flowerLanguage.currentEventId);

                    if (flowerLanguage.currentEventId == npcEventId)
                    {
                        Debug.Log("Found matching event ID: " + npcEventId);
                        return true;
                    }
                }
                else
                {
                    Debug.LogWarning("No FlowerLanguage component found on: " + child.name);
                }
            }

            if (CheckForEventIdInChildren(child)) // 递归检查子对象
            {
                return true;
            }
        }
        return false;
    }
}
