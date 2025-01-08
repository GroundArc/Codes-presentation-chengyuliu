using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public float radius = 3f;
    public Transform interactiontransform;

    

    public virtual void Interact() 
    {
        Debug.Log("Interact with " + transform.name);
    }
    void OnDrawGizmosSelected()
    {
        if (interactiontransform == null)
            interactiontransform = transform;
        Gizmos.color = UnityEngine.Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
