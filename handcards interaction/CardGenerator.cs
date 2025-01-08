using UnityEngine;

public class CardGenerator : MonoBehaviour
{
    

    public void GenerateCard()
    {
        if (HandCardManager.Instance != null)
        {
            HandCardManager.Instance.RandomGenerateCard();
        }
        else
        {
            Debug.LogError("HandCardManager instance is not found!");
        }
    }
}