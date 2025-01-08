using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Transform player;
    public float moveSpeed;
    public Vector3 offset;

    public float folllowDistance;
    private float followSpeed;
    private float currentMinZ;
    private float currentMinX;
    private float targetMinX;
    private float currentMaxX;
    private float targetMaxX;
    private float targetMinZ;
    public float minZAdjustSpeed;
    public float minXAdjustSpeed;
    public float maxXAdjustSpeed;

    public void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        currentMinZ = player.position.z;
        currentMinX = player.position.x;
        currentMaxX = player.position.x;
        followSpeed = moveSpeed;
    }

    private void Update()
    {
        if (GameManager.isGameOver || GameManager.isGameLost || player == null)
        {
            return;
        }
        currentMinZ = Mathf.Lerp(currentMinZ, targetMinZ, minZAdjustSpeed * Time.deltaTime);
        currentMinX = Mathf.Lerp(currentMinX, targetMinX, minXAdjustSpeed * Time.deltaTime);
        currentMaxX = Mathf.Lerp(currentMaxX, targetMaxX, maxXAdjustSpeed * Time.deltaTime);

        if (Mathf.Abs(currentMinX - targetMinX) < 0.01f &&
            Mathf.Abs(currentMaxX - targetMaxX) < 0.01f ||
            (player.position.x <= targetMaxX + 0.5f && player.position.x >= targetMinX - 0.5f))
        {
            followSpeed = moveSpeed;
        }

        // Debug
        Vector3 targetPosition = player.position + offset - transform.forward * folllowDistance;
        Vector3 pos = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);

        pos.z = Mathf.Clamp(pos.z, currentMinZ, float.MaxValue);
        pos.x = Mathf.Clamp(pos.x, currentMinX, currentMaxX);

        transform.position = pos;
    }

    public void SetBound(float minZ, float minX, float maxX)
    {
        targetMinZ = minZ;
        targetMinX = minX;
        targetMaxX = maxX;
        followSpeed = 0;
    }
}
