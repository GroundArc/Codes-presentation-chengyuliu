using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "ColorOwned/Item")]
public class Item : ScriptableObject
{
    new public string name = "New Item";
    public Sprite icon = null;
    public Color renderColor;
    public bool isDefaultItem = false;
    public Material colorMaterial;  // 用于存储颜色信息的材质
    public GameObject pigmentPrefab; // 颜料块预制体
    public GameObject tubePrefab;

    public virtual void Use()
    {
        // Use...
        Debug.Log("Using " + name);
    }
}