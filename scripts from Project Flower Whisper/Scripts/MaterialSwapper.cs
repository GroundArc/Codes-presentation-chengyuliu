using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MaterialSwapper : MonoBehaviour
{
    public GameObject targetObject; // 要替换材质的目标对象
    public Material grayMaterial; // 灰色材质
    public float transitionDuration = 1f; // 转换过程的持续时间

    private Dictionary<Renderer, Material[]> originalMaterials; // 用于存储原始材质

    void Start()
    {
        originalMaterials = new Dictionary<Renderer, Material[]>();
        SwapToGray();
    }

    // 将所有子对象的材质替换成灰色
    private void SwapToGray()
    {
        if (targetObject != null && grayMaterial != null)
        {
            Renderer[] renderers = targetObject.GetComponentsInChildren<Renderer>();
            foreach (Renderer rend in renderers)
            {
                // 为每个Renderer创建材质的实例
                Material[] materials = rend.materials;
                Material[] instantiatedMaterials = new Material[materials.Length];
                for (int i = 0; i < materials.Length; i++)
                {
                    instantiatedMaterials[i] = new Material(materials[i]);
                }
                rend.materials = instantiatedMaterials;

                // 存储原始材质
                if (!originalMaterials.ContainsKey(rend))
                {
                    originalMaterials[rend] = materials;
                }

                // 使用DoTween实现颜色过渡
                foreach (Material mat in rend.materials)
                {
                    mat.DOColor(grayMaterial.color, transitionDuration);
                }
            }
        }
    }

    // 恢复原始材质
    public void RestoreOriginalMaterials()
    {
        foreach (var entry in originalMaterials)
        {
            Renderer rend = entry.Key;
            Material[] originalMats = entry.Value;
            Material[] currentMats = rend.materials;
            for (int i = 0; i < currentMats.Length; i++)
            {
                Material originalMat = originalMats[i];
                Material currentMat = currentMats[i];
                currentMat.DOColor(originalMat.color, transitionDuration).OnComplete(() =>
                {
                    currentMats[i] = originalMat; // 完成颜色过渡后恢复原始材质
                });
            }
            rend.materials = currentMats;
        }
        originalMaterials.Clear(); // 清除存储的原始材质
    }
}
