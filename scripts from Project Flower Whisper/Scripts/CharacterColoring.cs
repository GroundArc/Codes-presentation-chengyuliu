using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterColoring : MonoBehaviour
{
    public List<GameObject> bodyParts; // 角色的三个子对象
    public Material defaultMaterial; // 默认材质

    void Start()
    {
        UpdateBodyParts(); // 初始化时更新子对象材质
    }

    void Update()
    {
        UpdateBodyParts(); // 每帧更新子对象材质
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

                // 如果有足够的itemsArray中的材料
                if (i < itemsArray.Length)
                {
                    Material newMaterial = itemsArray[i].colorMaterial;

                    // 将所有材质都替换成itemsArray[i].colorMaterial
                    for (int j = 0; j < materials.Length; j++)
                    {
                        materials[j] = newMaterial;
                    }
                }
                else
                {
                    // 如果itemsArray不够，则使用默认材质
                    for (int j = 0; j < materials.Length; j++)
                    {
                        materials[j] = defaultMaterial;
                    }
                }

                // 更新材质列表
                renderer.materials = materials;
            }
        }
    }

}
