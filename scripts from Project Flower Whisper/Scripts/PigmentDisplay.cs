using UnityEngine;
using TMPro;

public class PigmentDisplay : MonoBehaviour
{
    public TMP_Text quantityText;
    private Camera mainCamera;

    

    public void SetQuantity(int quantity)
    {
        if (quantityText != null)
        {
            quantityText.text = quantity.ToString();
        }
    }
}
