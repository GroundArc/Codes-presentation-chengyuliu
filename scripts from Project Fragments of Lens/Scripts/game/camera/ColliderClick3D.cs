using UnityEngine;
using UnityEngine.Events;

public class ColliderClick3D : MonoBehaviour
{
    public static int igonreFrame;

    public LayerMask layerMask;
    public UnityEvent evt;
    public int mouseBtnNum = 0;
    public float dist = 30;
    // Update is called once per frame

    private bool _check;
    void LateUpdate()
    {
        if (Time.frameCount==igonreFrame)
           return;

        if (!_check)
        {
            // Check if the left mouse button was clicked
            if (Input.GetMouseButtonDown(mouseBtnNum))
            {
                // Create a ray from the mouse cursor on screen in the direction of the camera
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                // Perform the raycast
                if (Physics.Raycast(ray, out hit, dist, layerMask))
                {
                    if (hit.transform.gameObject == this.gameObject)
                    {
                        _check = true;
                    }
                }
            }
        }
        else
        {
            if (Input.GetMouseButtonUp(mouseBtnNum))
            {
                _check = false;
                // Create a ray from the mouse cursor on screen in the direction of the camera
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                // Perform the raycast
                if (Physics.Raycast(ray, out hit, dist, layerMask))
                {
                    if (hit.transform.gameObject == this.gameObject)
                    {
                        // Call your custom function
                        evt?.Invoke();
                    }
                }
            }
        }
    }
}
