using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowNavigation : MonoBehaviour
{
    public Transform arrow; // Reference to the arrow Transform
    public float distanceFromPlayer = 1.0f; // Distance from the player where the arrow should appear
    public Vector3 rotationOffset; // Rotation offset for the arrow
    private FragmentSpawnerNew fragmentSpawner;
    private bool isArrowActive = false;

    private void Start()
    {
        fragmentSpawner = FindObjectOfType<FragmentSpawnerNew>();
        isArrowActive = true;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            isArrowActive = !isArrowActive;
        }

        if (!isArrowActive)
        {
            arrow.gameObject.SetActive(false);
            return;
        }
        GameObject nearestFragment = FindNearestFragment();

        if (nearestFragment != null)
        {
            // Calculate direction from player to fragment
            Vector3 directionToFragment = (nearestFragment.transform.position - transform.position).normalized;

            // Position the arrow around the player, based on the direction to the fragment
            arrow.position = transform.position + directionToFragment * distanceFromPlayer;

            // Look at the fragment and apply rotation offset
            arrow.LookAt(nearestFragment.transform);
            arrow.rotation *= Quaternion.Euler(rotationOffset);
            arrow.gameObject.SetActive(true);
        }
        else
        {
            arrow.gameObject.SetActive(false);
        }
    }

    private GameObject FindNearestFragment()
    {
        GameObject nearest = null;
        float shortestDistance = Mathf.Infinity;

        // Use a loop that can modify the list while iterating
        for (int i = fragmentSpawner.spawnedFragments.Count - 1; i >= 0; i--)
        {
            GameObject fragment = fragmentSpawner.spawnedFragments[i];

            // Check if fragment still exists
            if (fragment == null)
            {
                fragmentSpawner.spawnedFragments.RemoveAt(i);
                continue;
            }

            float distance = Vector3.Distance(transform.position, fragment.transform.position);
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                nearest = fragment;
            }
        }

        return nearest;
    }
}
