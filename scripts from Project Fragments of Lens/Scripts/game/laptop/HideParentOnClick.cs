using UnityEngine;

public class HideParentOnClick : MonoBehaviour
{
    public void HideParent()
    {
        // check parent exit
        if (transform.parent != null)
        {
            // hide parent
            transform.parent.gameObject.SetActive(false);
        }
        else
        {
            // if no parent, hide itself
            gameObject.SetActive(false);
        }
    }
}
