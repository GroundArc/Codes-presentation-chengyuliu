using UnityEngine;
using System.Collections;

public class ColorSwitch : MonoBehaviour
{
    private Material[] materials;
    private Color[] originalColors;
    private Color grayColor = Color.gray;
    public float transitionDuration = 2.0f;

    void Start()
    {
        // ��ȡ���в���
        Renderer renderer = GetComponent<Renderer>();
        materials = renderer.materials;

        // ��ʼ������ÿ�����ʵ�ԭʼ��ɫ
        originalColors = new Color[materials.Length];

        for (int i = 0; i < materials.Length; i++)
        {
            // ����ÿ�����ʵ�ԭʼ��ɫ
            originalColors[i] = materials[i].GetColor("_MainColor");

            // ��ʼ����ɫΪ��ɫ
            materials[i].SetColor("_MainColor", grayColor);
        }
    }

    // ���������ͨ���¼�ϵͳ����
    public void StartColorTransition()
    {
        // ������ɫ����Э��
        StartCoroutine(ColorTransition());
    }

    private IEnumerator ColorTransition()
    {
        float timeElapsed = 0;

        while (timeElapsed < transitionDuration)
        {
            // ������ɽ���
            float t = timeElapsed / transitionDuration;

            // �𽥽�ÿ�����ʵ���ɫ�ӻ�ɫ��Ϊԭʼ��ɫ
            for (int i = 0; i < materials.Length; i++)
            {
                Color currentColor = Color.Lerp(grayColor, originalColors[i], t);
                materials[i].SetColor("_MainColor", currentColor);
            }

            // ���Ӿ�����ʱ��
            timeElapsed += Time.deltaTime;

            // �ȴ���һ֡
            yield return null;
        }

        // ȷ�����ÿ�����ʵ���ɫ��Ϊԭʼ��ɫ
        for (int i = 0; i < materials.Length; i++)
        {
            materials[i].SetColor("_MainColor", originalColors[i]);
        }
    }
}
