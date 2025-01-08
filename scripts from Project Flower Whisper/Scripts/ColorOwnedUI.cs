using UnityEngine;

public class ColorOwnedUI : MonoBehaviour
{
    public Transform itemsParent;
    public GameObject colorOwnedUI;
    ColorOwned colorOwned;
    ColorSlot[] slots;

    void Start()
    {
        colorOwned = ColorOwned.instance;
        colorOwned.onItemChangedCallBack += UpdateUI;
        slots = itemsParent.GetComponentsInChildren<ColorSlot>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Color Pallet"))
        {
            colorOwnedUI.SetActive(!colorOwnedUI.activeSelf);
        }
    }

    void UpdateUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (i < colorOwned.items.Count)
            {
                slots[i].AddItem(colorOwned.items[i]);
            }
            else
            {
                slots[i].ClearSlot();
            }
        }
        Debug.Log("UPDATING UI");
    }
}
