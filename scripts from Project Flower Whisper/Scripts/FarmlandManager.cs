using UnityEngine;
using System.Collections.Generic;

public class FarmlandManager : MonoBehaviour
{
    public GameObject[] flowerPrefabs; // Array of flower prefabs representing the eight types of flowers
    public Field[] fields; // Array of fields, each represented by a Field component
    public float checkInterval = 60f; // Time interval (in seconds) to check each field for flower planting

    // Dictionary to keep track of the count of each flower type currently planted
    private Dictionary<string, int> flowerCounts = new Dictionary<string, int>();

    void Start()
    {
        // Initialize the flower counts for each flower type to zero
        foreach (GameObject flowerPrefab in flowerPrefabs)
        {
            flowerCounts[flowerPrefab.name] = 0;
        }

        // Start periodically checking each field for planting flowers
        InvokeRepeating(nameof(CheckFields), 0f, checkInterval);
    }

    // Method to check each field and plant a flower if the field is empty
    void CheckFields()
    {
        foreach (Field field in fields)
        {
            // Check if the field has a flower planted
            if (!field.HasFlower())
            {
                // Select a flower type to plant based on the current counts
                GameObject selectedFlower = SelectFlower();
                if (selectedFlower != null)
                {
                    // Plant the selected flower in the field
                    field.PlantFlower(selectedFlower);
                    // Increment the count of the planted flower type
                    flowerCounts[selectedFlower.name]++;
                }
            }
        }
    }

    // Method to select a flower type based on the current counts, favoring those with the least instances
    GameObject SelectFlower()
    {
        List<GameObject> candidates = new List<GameObject>(); // List to store flower candidates with the minimum count
        int minCount = int.MaxValue; // Variable to track the minimum count of any flower type

        foreach (GameObject flowerPrefab in flowerPrefabs)
        {
            int count = flowerCounts[flowerPrefab.name]; // Get the current count of this flower type
            if (count < minCount)
            {
                // If this flower type has the lowest count, clear the candidate list and add this flower
                minCount = count;
                candidates.Clear();
                candidates.Add(flowerPrefab);
            }
            else if (count == minCount)
            {
                // If this flower type matches the current minimum count, add it to the candidate list
                candidates.Add(flowerPrefab);
            }
        }

        // Randomly select one flower from the candidates list
        if (candidates.Count > 0)
        {
            return candidates[Random.Range(0, candidates.Count)];
        }

        // Return null if no candidates are available (this shouldn't normally happen)
        return null;
    }
}
