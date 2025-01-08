using UnityEngine;

public class ClickToggleVideo : MonoBehaviour
{
    public GameObject toggleTarget;

    public void OnClickToggle()
    {
        toggleTarget.SetActive(!toggleTarget.activeSelf);
    }
}