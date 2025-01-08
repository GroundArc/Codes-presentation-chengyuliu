using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasSpawner : MonoBehaviour
{
    public GameObject canvasPrefab;
    // Start is called before the first frame update
    void Start()
    {
        SpawnCanvas();
    }
    public void SpawnCanvas()
    {
        if (canvasPrefab)
        {
            Instantiate(canvasPrefab, Vector3.zero, Quaternion.identity);
        }
    }
}
