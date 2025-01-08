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
        // 获取所有材质
        Renderer renderer = GetComponent<Renderer>();
        materials = renderer.materials;

        // 初始化保存每个材质的原始颜色
        originalColors = new Color[materials.Length];

        for (int i = 0; i < materials.Length; i++)
        {
            // 保存每个材质的原始颜色
            originalColors[i] = materials[i].GetColor("_MainColor");

            // 初始化颜色为灰色
            materials[i].SetColor("_MainColor", grayColor);
        }
    }

    // 这个方法将通过事件系统触发
    public void StartColorTransition()
    {
        // 启动颜色过渡协程
        StartCoroutine(ColorTransition());
    }

    private IEnumerator ColorTransition()
    {
        float timeElapsed = 0;

        while (timeElapsed < transitionDuration)
        {
            // 计算过渡进度
            float t = timeElapsed / transitionDuration;

            // 逐渐将每个材质的颜色从灰色变为原始颜色
            for (int i = 0; i < materials.Length; i++)
            {
                Color currentColor = Color.Lerp(grayColor, originalColors[i], t);
                materials[i].SetColor("_MainColor", currentColor);
            }

            // 增加经过的时间
            timeElapsed += Time.deltaTime;

            // 等待下一帧
            yield return null;
        }

        // 确保最后每个材质的颜色都为原始颜色
        for (int i = 0; i < materials.Length; i++)
        {
            materials[i].SetColor("_MainColor", originalColors[i]);
        }
    }
}
