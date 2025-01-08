using UnityEngine;
using TMPro;

public class RandomTextColor : MonoBehaviour
{
    public TMP_Text tmpText; // 引用到TextMeshPro组件
    public Gradient colorGradient; // 渐变色，可以用来生成不同的随机颜色（可选）

    void Start()
    {
        if (tmpText == null)
        {
            tmpText = GetComponent<TMP_Text>();
        }

        if (tmpText != null)
        {
            ApplyRandomColors();
        }
    }

    void ApplyRandomColors()
    {
        // 获取文本内容
        string text = tmpText.text;

        // 创建一个 TMP_TextInfo，用于存储文本的信息
        TMP_TextInfo textInfo = tmpText.textInfo;

        // 强制更新文本信息（确保TMP_TextInfo已初始化）
        tmpText.ForceMeshUpdate();

        // 获取文本中字符的数量
        int charCount = textInfo.characterCount;

        // 遍历每个字符
        for (int i = 0; i < charCount; i++)
        {
            // 获取字符信息
            var charInfo = textInfo.characterInfo[i];

            // 仅处理可见字符
            if (!charInfo.isVisible)
                continue;

            // 生成随机颜色
            Color32 randomColor = GenerateRandomColor();

            // 访问顶点颜色（一个字符通常由四个顶点构成）
            int vertexIndex = charInfo.vertexIndex;

            tmpText.mesh.colors32[vertexIndex + 0] = randomColor; // 左下角
            tmpText.mesh.colors32[vertexIndex + 1] = randomColor; // 右下角
            tmpText.mesh.colors32[vertexIndex + 2] = randomColor; // 左上角
            tmpText.mesh.colors32[vertexIndex + 3] = randomColor; // 右上角
        }

        // 更新Mesh信息以应用更改
        tmpText.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
    }

    Color32 GenerateRandomColor()
    {
        // 可以使用下面的方式生成一个随机颜色
        return new Color32(
            (byte)Random.Range(0, 256),
            (byte)Random.Range(0, 256),
            (byte)Random.Range(0, 256),
            255 // Alpha值设为255，表示完全不透明
        );

        // 或者，如果你想要使用一个渐变色来生成颜色，可以使用如下方法：
        // return colorGradient.Evaluate(Random.Range(0f, 1f));
    }
}
