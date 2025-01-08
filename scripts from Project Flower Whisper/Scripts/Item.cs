using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "ColorOwned/Item")]
public class Item : ScriptableObject
{
    new public string name = "New Item";
    public Sprite icon = null;
    public Color renderColor;
    public bool isDefaultItem = false;
    public Material colorMaterial;  // ���ڴ洢��ɫ��Ϣ�Ĳ���
    public GameObject pigmentPrefab; // ���Ͽ�Ԥ����
    public GameObject tubePrefab;

    public virtual void Use()
    {
        // Use...
        Debug.Log("Using " + name);
    }
}