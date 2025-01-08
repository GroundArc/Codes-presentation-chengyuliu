using System.Collections.Generic;
using UnityEngine;

public class ActivateObjectsOnClick : MonoBehaviour
{
    public List<GameObject> targetObjects; // The list of objects to activate

    public void ActivateObjects()
    {
        if (targetObjects != null && targetObjects.Count > 0)
        {
            foreach (var obj in targetObjects)
            {
                if (obj != null)
                {
                    obj.SetActive(true);
                }
                else
                {
                    Debug.LogWarning("One of the target objects is not set.");
                }
            }
        }
        else
        {
            Debug.LogWarning("Target objects list is not set or is empty.");
        }
    }
}
