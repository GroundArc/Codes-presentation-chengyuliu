using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterColoring : MonoBehaviour
{
    public List<GameObject> bodyParts; // ��ɫ�������Ӷ���
    public Material defaultMaterial; // Ĭ�ϲ���

    void Start()
    {
        UpdateBodyParts(); // ��ʼ��ʱ�����Ӷ������
    }

    void Update()
    {
        UpdateBodyParts(); // ÿ֡�����Ӷ������
    }

    public void UpdateBodyParts()
    {
        Item[] itemsArray = Backpack.instance.GetItemsArray();
        for (int i = 0; i < bodyParts.Count; i++)
        {
            Renderer renderer = bodyParts[i].GetComponent<Renderer>();
            if (renderer != null)
            {
                Material[] materials = renderer.materials;

                // ������㹻��itemsArray�еĲ���
                if (i < itemsArray.Length)
                {
                    Material newMaterial = itemsArray[i].colorMaterial;

                    // �����в��ʶ��滻��itemsArray[i].colorMaterial
                    for (int j = 0; j < materials.Length; j++)
                    {
                        materials[j] = newMaterial;
                    }
                }
                else
                {
                    // ���itemsArray��������ʹ��Ĭ�ϲ���
                    for (int j = 0; j < materials.Length; j++)
                    {
                        materials[j] = defaultMaterial;
                    }
                }

                // ���²����б�
                renderer.materials = materials;
            }
        }
    }

}
