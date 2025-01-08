using UnityEngine;
using TMPro;

public class RandomTextColor : MonoBehaviour
{
    public TMP_Text tmpText; // ���õ�TextMeshPro���
    public Gradient colorGradient; // ����ɫ�������������ɲ�ͬ�������ɫ����ѡ��

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
        // ��ȡ�ı�����
        string text = tmpText.text;

        // ����һ�� TMP_TextInfo�����ڴ洢�ı�����Ϣ
        TMP_TextInfo textInfo = tmpText.textInfo;

        // ǿ�Ƹ����ı���Ϣ��ȷ��TMP_TextInfo�ѳ�ʼ����
        tmpText.ForceMeshUpdate();

        // ��ȡ�ı����ַ�������
        int charCount = textInfo.characterCount;

        // ����ÿ���ַ�
        for (int i = 0; i < charCount; i++)
        {
            // ��ȡ�ַ���Ϣ
            var charInfo = textInfo.characterInfo[i];

            // ������ɼ��ַ�
            if (!charInfo.isVisible)
                continue;

            // ���������ɫ
            Color32 randomColor = GenerateRandomColor();

            // ���ʶ�����ɫ��һ���ַ�ͨ�����ĸ����㹹�ɣ�
            int vertexIndex = charInfo.vertexIndex;

            tmpText.mesh.colors32[vertexIndex + 0] = randomColor; // ���½�
            tmpText.mesh.colors32[vertexIndex + 1] = randomColor; // ���½�
            tmpText.mesh.colors32[vertexIndex + 2] = randomColor; // ���Ͻ�
            tmpText.mesh.colors32[vertexIndex + 3] = randomColor; // ���Ͻ�
        }

        // ����Mesh��Ϣ��Ӧ�ø���
        tmpText.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
    }

    Color32 GenerateRandomColor()
    {
        // ����ʹ������ķ�ʽ����һ�������ɫ
        return new Color32(
            (byte)Random.Range(0, 256),
            (byte)Random.Range(0, 256),
            (byte)Random.Range(0, 256),
            255 // Alphaֵ��Ϊ255����ʾ��ȫ��͸��
        );

        // ���ߣ��������Ҫʹ��һ������ɫ��������ɫ������ʹ�����·�����
        // return colorGradient.Evaluate(Random.Range(0f, 1f));
    }
}
