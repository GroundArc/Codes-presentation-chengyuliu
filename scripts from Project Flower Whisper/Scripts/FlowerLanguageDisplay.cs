using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class FlowerLanguageDisplay : MonoBehaviour
{
    public TMP_Text flowerLanguageText; // TextMeshPro UI
    public float fadeDuration = 1.0f; // 慢慢浮现的时间

    private void Start()
    {
        if (flowerLanguageText == null)
        {
            Debug.LogError("flowerLanguageText is not assigned.");
            return;
        }

        // 初始时隐藏文本
        flowerLanguageText.alpha = 0;
       
    }
    void Update()
    {
        UpdateFlowerLanguages();
    }

    public void DisplayFlowerLanguages()
    {
        Debug.Log("Starting to display flower languages.");

        List<string> flowerLanguages = new List<string>();

        foreach (Transform child in transform)
        {
            if (child.CompareTag("Flower"))
            {
                Debug.Log("Found a Flower child: " + child.name);

                FlowerLanguage flowerLanguage = child.GetComponent<FlowerLanguage>();
                if (flowerLanguage != null)
                {
                    Debug.Log("Found FlowerLanguage component on: " + child.name);
                    Debug.Log("Current flower language: " + flowerLanguage.currentFlowerLanguage);

                    if (!string.IsNullOrEmpty(flowerLanguage.currentFlowerLanguage))
                    {
                        flowerLanguages.Add(flowerLanguage.currentFlowerLanguage);
                    }
                }
                else
                {
                   Debug.LogWarning("No FlowerLanguage component found on: " + child.name);
                }
            }
            else
            {
                Debug.Log("Child is not a Flower: " + child.name);
            }
        }

        if (flowerLanguages.Count > 0)
        {
            flowerLanguageText.text = string.Join("\n", flowerLanguages);
            Debug.Log("Updated flowerLanguageText with languages: " + flowerLanguageText.text);

            // 使用DoTween插件进行慢慢浮现效果
            flowerLanguageText.DOFade(1, fadeDuration).SetEase(Ease.InOutQuad);
        }
        else
        {
            Debug.Log("No flower languages found to display.");
            flowerLanguageText.text = string.Empty;
            flowerLanguageText.alpha = 0;
        }
    }

    // 手动调用以更新显示的花语
    public void UpdateFlowerLanguages()
    {
        DisplayFlowerLanguages();
    }
}
