using UnityEngine;
using TMPro;

public class FlowerPickup : MonoBehaviour
{
    public Flower flowerData; // 这个花朵对应的 ScriptableObject 数据
    private TMP_Text pickupText; // 用于显示Item名称的TMP组件
    private bool isPlayerInRange = false;
    private Outline outline; // 引用 Outline 组件

    void Start()
    {
        // 获取 Outline 组件
        outline = GetComponent<Outline>();
        if (outline != null)
        {
            outline.enabled = false; // 初始时禁用 Outline
        }

        // 在场景中查找名为 "PickupFlower" 的 TMP_Text 组件
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
        // 检测玩家按下F键
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.F))
        {
            PickUpFlower();
        }
    }

    private void PickUpFlower()
    {
        // 将花朵信息添加到 FlowerManager 列表中
        FlowerManager.instance.Add(flowerData);
        Debug.Log("Picked up flower: " + flowerData.flowerName);

        // 销毁花朵预制体
        Destroy(gameObject);

        // 禁用 PickupFlower TMP 组件
        if (pickupText != null)
        {
            pickupText.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // 检测玩家进入范围
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;

            // 启用 Outline
            if (outline != null)
            {
                outline.enabled = true;
            }

            // 显示并更新 PickupFlower TMP 组件
            if (pickupText != null)
            {
                pickupText.text = "Pick " + flowerData.flowerName;
                pickupText.gameObject.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // 检测玩家离开范围
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;

            // 禁用 Outline
            if (outline != null)
            {
                outline.enabled = false;
            }

            // 隐藏 PickupFlower TMP 组件
            if (pickupText != null)
            {
                pickupText.gameObject.SetActive(false);
            }
        }
    }
}
