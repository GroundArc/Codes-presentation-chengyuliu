using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerTagCheckTrigger : MonoBehaviour
{
    public GameObject targetObject; // Ҫ����Ŀ�����
    public Dialogue dialogueCorrectFound; // �ҵ��ض�Tag���Ӷ���ʱ�����ĶԻ�
    public Dialogue dialogueNotFound; // δ�ҵ��ض�Tag���Ӷ���ʱ�����ĶԻ�
    public Collider triggerCollider;
    public float npcEventId; // NPC �ɴ������¼� ID

    public UnityEvent onTagFound; // �ҵ��ض�Tagʱ�������¼�
    public UnityEvent onTagNotFound; // δ�ҵ��ض�Tagʱ�������¼�

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
            onTagFound?.Invoke(); // �����¼�
            dialogueManager.StartDialogue(dialogueCorrectFound);
        }
        else
        {
            onTagNotFound?.Invoke(); // �����¼�
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

            if (CheckForEventIdInChildren(child)) // �ݹ����Ӷ���
            {
                return true;
            }
        }
        return false;
    }
}
