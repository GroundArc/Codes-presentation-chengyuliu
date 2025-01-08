using UnityEngine;
using System.Collections.Generic;

public class MaterialSwitcher : MonoBehaviour
{
    public Material grayMaterial; // 用于设置为灰色的材质
    private Dictionary<Renderer, Material[]> originalMaterials; // 用于存储每个子对象的原始材质

    void Start()
    {
        // 初始化存储原始材质的字典
        originalMaterials = new Dictionary<Renderer, Material[]>();

        // 获取该对象的所有子对象的Renderer组件
        Renderer[] renderers = GetComponentsInChildren<Renderer>();

        // 遍历所有Renderer组件
        foreach (Renderer renderer in renderers)
        {
            // 保存原始材质
            originalMaterials[renderer] = renderer.materials;

            // 创建一个新的材质数组，设置为灰色材质
            Material[] grayMaterials = new Material[renderer.materials.Length];
            for (int i = 0; i < grayMaterials.Length; i++)
            {
                grayMaterials[i] = grayMaterial;
            }

            // 将所有材质替换为灰色材质
            renderer.materials = grayMaterials;
        }
    }

    // 恢复所有子对象的原始材质
    public void RestoreOriginalMaterials()
    {
        // 遍历原始材质的字典，恢复每个子对象的原始材质
        foreach (var entry in originalMaterials)
        {
            entry.Key.materials = entry.Value;
        }
    }
}
