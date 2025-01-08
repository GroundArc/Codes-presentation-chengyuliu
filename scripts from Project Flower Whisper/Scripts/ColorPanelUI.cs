using UnityEngine;

public class ColorPanelUI : MonoBehaviour
{
    public Transform itemsParent; // This should be SlotParent in your hierarchy
    public GameObject colorOwnedUI;
    private ColorOwned colorOwned;
    private ColorSlot[] slots;

    void Start()
    {
        colorOwned = ColorOwned.instance;
        if (colorOwned != null)
        {
            colorOwned.onItemChangedCallBack += UpdateUI;
        }
        slots = itemsParent.GetComponentsInChildren<ColorSlot>();
        UpdateUI();
    }

    void UpdateUI()
    {
        if (slots == null || colorOwned == null) return;

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
