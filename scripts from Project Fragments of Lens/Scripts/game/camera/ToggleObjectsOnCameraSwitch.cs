using UnityEngine;
using System.Collections.Generic;

public class ToggleObjectsOnCameraSwitch : MonoBehaviour
{
    [System.Serializable]
    public class ToggleEntry
    {
        public GameObject targetObject;
        public MonoBehaviour targetComponent;
        public bool activateOnSwitch;
    }

    [Header("Switch to Render Texture Camera")]
    public List<ToggleEntry> onSwitchToRenderTexture;

    [Header("Switch to Main Camera")]
    public List<ToggleEntry> onSwitchToMainCamera;

    public void SwitchToRenderTextureCamera()
    {
        SetActiveState(onSwitchToRenderTexture);
    }

    public void SwitchToMainCamera()
    {
        SetActiveState(onSwitchToMainCamera);
    }

    private void SetActiveState(List<ToggleEntry> entries)
    {
        foreach (var entry in entries)
        {
            if (entry.targetObject != null)
            {
                entry.targetObject.SetActive(entry.activateOnSwitch);
            }

            if (entry.targetComponent != null)
            {
                entry.targetComponent.enabled = entry.activateOnSwitch;
            }
        }
    }
}
