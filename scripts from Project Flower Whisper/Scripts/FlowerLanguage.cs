using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemLanguagePair
{
    public Item item; // The corresponding Item object
    public string language; // The corresponding flower language (symbolic meaning)
    public float eventId; // The corresponding NPC event ID
}

public class FlowerLanguage : MonoBehaviour
{
    public FlowerBehaviour flowerBehaviour; // Reference to the FlowerBehaviour script
    public List<ItemLanguagePair> languageEntries; // List of flower language entries

    public string currentFlowerLanguage; // Stores the current flower language based on the selected item
    public float currentEventId; // Stores the current event ID associated with the selected item

    void Update()
    {
        UpdateFlowerLanguage(); // Continuously update the flower language based on the current item
    }

    private void UpdateFlowerLanguage()
    {
        // If the FlowerBehaviour script or the current pigment item is not set, clear the flower language and event ID
        if (flowerBehaviour == null || flowerBehaviour.currentPigmentItem == null)
        {
            currentFlowerLanguage = string.Empty; // Clear the current flower language
            currentEventId = -1; // Use -1 to indicate no valid event ID
            // Debug.Log("No currentPigmentItem or no corresponding flower language found.");
            return;
        }

        // Get the current pigment item from the FlowerBehaviour script
        Item currentItem = flowerBehaviour.currentPigmentItem;

        // Loop through the list of language entries to find a match for the current item
        foreach (var entry in languageEntries)
        {
            if (entry.item == currentItem)
            {
                // If a match is found, update the current flower language and event ID
                currentFlowerLanguage = entry.language;
                currentEventId = entry.eventId;
                Debug.Log("Current Flower Language: " + currentFlowerLanguage);
                Debug.Log("Current Event ID: " + currentEventId);
                return; // Exit the method once a match is found
            }
        }

        // If no matching flower language is found, clear the current flower language and event ID
        currentFlowerLanguage = string.Empty;
        currentEventId = -1; // No matching event ID found
        Debug.Log("No corresponding flower language found for the material.");
    }
}
