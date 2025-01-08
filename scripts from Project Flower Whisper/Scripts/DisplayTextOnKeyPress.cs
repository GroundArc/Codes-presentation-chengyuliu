using UnityEngine;
using TMPro;

public class DisplayTextOnKeyPress : MonoBehaviour
{
    // ��Inspector��ָ��TextMeshProUGUI���
    public TextMeshProUGUI textComponent;
    // ��Inspector��ָ��Ҫ��ʾ������
    public string displayText = "Hello, World!";

    // Update is called once per frame
    void Update()
    {
        // ����Ƿ���Q��
        if (Input.GetKeyDown(KeyCode.Q))
        {
            // ����ָ��������
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
