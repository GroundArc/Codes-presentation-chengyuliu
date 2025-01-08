using UnityEngine;
using TMPro;

public class FlowerPickup : MonoBehaviour
{
    public Flower flowerData; // ��������Ӧ�� ScriptableObject ����
    private TMP_Text pickupText; // ������ʾItem���Ƶ�TMP���
    private bool isPlayerInRange = false;
    private Outline outline; // ���� Outline ���

    void Start()
    {
        // ��ȡ Outline ���
        outline = GetComponent<Outline>();
        if (outline != null)
        {
            outline.enabled = false; // ��ʼʱ���� Outline
        }

        // �ڳ����в�����Ϊ "PickupFlower" �� TMP_Text ���
        GameObject pickupTextObject = GameObject.Find("PickupFlower");
        if (pickupTextObject != null)
        {
            pickupText = pickupTextObject.GetComponent<TMP_Text>();
        }
        else
        {
            Debug.LogError("No GameObject named 'PickupFlower' found in the scene.");
        }
    }

    void Update()
    {
        // �����Ұ���F��
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.F))
        {
            PickUpFlower();
        }
    }

    private void PickUpFlower()
    {
        // ��������Ϣ��ӵ� FlowerManager �б���
        FlowerManager.instance.Add(flowerData);
        Debug.Log("Picked up flower: " + flowerData.flowerName);

        // ���ٻ���Ԥ����
        Destroy(gameObject);

        // ���� PickupFlower TMP ���
        if (pickupText != null)
        {
            pickupText.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // �����ҽ��뷶Χ
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;

            // ���� Outline
            if (outline != null)
            {
                outline.enabled = true;
            }

            // ��ʾ������ PickupFlower TMP ���
            if (pickupText != null)
            {
                pickupText.text = "Pick " + flowerData.flowerName;
                pickupText.gameObject.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // �������뿪��Χ
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;

            // ���� Outline
            if (outline != null)
            {
                outline.enabled = false;
            }

            // ���� PickupFlower TMP ���
            if (pickupText != null)
            {
                pickupText.gameObject.SetActive(false);
            }
        }
    }
}
