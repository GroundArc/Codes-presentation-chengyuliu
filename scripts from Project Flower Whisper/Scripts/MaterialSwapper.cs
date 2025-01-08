using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MaterialSwapper : MonoBehaviour
{
    public GameObject targetObject; // Ҫ�滻���ʵ�Ŀ�����
    public Material grayMaterial; // ��ɫ����
    public float transitionDuration = 1f; // ת�����̵ĳ���ʱ��

    private Dictionary<Renderer, Material[]> originalMaterials; // ���ڴ洢ԭʼ����

    void Start()
    {
        originalMaterials = new Dictionary<Renderer, Material[]>();
        SwapToGray();
    }

    // �������Ӷ���Ĳ����滻�ɻ�ɫ
    private void SwapToGray()
    {
        if (targetObject != null && grayMaterial != null)
        {
            Renderer[] renderers = targetObject.GetComponentsInChildren<Renderer>();
            foreach (Renderer rend in renderers)
            {
                // Ϊÿ��Renderer�������ʵ�ʵ��
                Material[] materials = rend.materials;
                Material[] instantiatedMaterials = new Material[materials.Length];
                for (int i = 0; i < materials.Length; i++)
                {
                    instantiatedMaterials[i] = new Material(materials[i]);
                }
                rend.materials = instantiatedMaterials;

                // �洢ԭʼ����
                if (!originalMaterials.ContainsKey(rend))
                {
                    originalMaterials[rend] = materials;
                }

                // ʹ��DoTweenʵ����ɫ����
                foreach (Material mat in rend.materials)
                {
                    mat.DOColor(grayMaterial.color, transitionDuration);
                }
            }
        }
    }

    // �ָ�ԭʼ����
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
                    currentMats[i] = originalMat; // �����ɫ���ɺ�ָ�ԭʼ����
                });
            }
            rend.materials = currentMats;
        }
        originalMaterials.Clear(); // ����洢��ԭʼ����
    }
}
