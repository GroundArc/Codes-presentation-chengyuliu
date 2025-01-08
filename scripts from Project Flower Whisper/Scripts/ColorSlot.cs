using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ColorSlot : MonoBehaviour
{
    public Image icon;
    public TMP_Text quantityText; // สนำร TextMeshPro
    public Button removeButton;
    ItemStack itemStack;

    public void AddItem(ItemStack newItemStack)
    {
        itemStack = newItemStack;
        icon.sprite = itemStack.item.icon;
        icon.enabled = true;
        removeButton.interactable = true;
        quantityText.text = itemStack.quantity.ToString();
        quantityText.enabled = true;
    }

    public void ClearSlot()
    {
        itemStack = null;
        icon.sprite = null;
        icon.enabled = false;
        removeButton.interactable = false;
        quantityText.text = "";
        quantityText.enabled = false;
    }

    public void OnRemoveButton()
    {
        ColorOwned.instance.Remove(itemStack.item);
    }

    public void UseItem()
    {
        if (itemStack != null && itemStack.quantity > 0)
        {
            itemStack.item.Use();
        }
    }
}
