using UnityEngine;
using TMPro;

public class DisplayTextOnKeyPress : MonoBehaviour
{
    // 在Inspector中指定TextMeshProUGUI组件
    public TextMeshProUGUI textComponent;
    // 在Inspector中指定要显示的文字
    public string displayText = "Hello, World!";

    // Update is called once per frame
    void Update()
    {
        // 检查是否按下Q键
        if (Input.GetKeyDown(KeyCode.Q))
        {
            // 设置指定的文字
            if (textComponent != null)
            {
                textComponent.text = displayText;
            }
            else
            {
                Debug.LogError("TextMeshProUGUI component is not assigned.");
            }
        }
    }
}
