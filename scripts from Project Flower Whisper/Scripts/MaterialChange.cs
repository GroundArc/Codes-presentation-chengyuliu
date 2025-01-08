using UnityEngine;
using System.Collections.Generic;

public class MaterialSwitcher : MonoBehaviour
{
    public Material grayMaterial; // ��������Ϊ��ɫ�Ĳ���
    private Dictionary<Renderer, Material[]> originalMaterials; // ���ڴ洢ÿ���Ӷ����ԭʼ����

    void Start()
    {
        // ��ʼ���洢ԭʼ���ʵ��ֵ�
        originalMaterials = new Dictionary<Renderer, Material[]>();

        // ��ȡ�ö���������Ӷ����Renderer���
        Renderer[] renderers = GetComponentsInChildren<Renderer>();

        // ��������Renderer���
        foreach (Renderer renderer in renderers)
        {
            // ����ԭʼ����
            originalMaterials[renderer] = renderer.materials;

            // ����һ���µĲ������飬����Ϊ��ɫ����
            Material[] grayMaterials = new Material[renderer.materials.Length];
            for (int i = 0; i < grayMaterials.Length; i++)
            {
                grayMaterials[i] = grayMaterial;
            }

            // �����в����滻Ϊ��ɫ����
            renderer.materials = grayMaterials;
        }
    }

    // �ָ������Ӷ����ԭʼ����
    public void RestoreOriginalMaterials()
    {
        // ����ԭʼ���ʵ��ֵ䣬�ָ�ÿ���Ӷ����ԭʼ����
        foreach (var entry in originalMaterials)
        {
            entry.Key.materials = entry.Value;
        }
    }
}
