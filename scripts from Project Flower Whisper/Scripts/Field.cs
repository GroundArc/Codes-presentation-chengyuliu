using UnityEngine;

public class Field : MonoBehaviour
{
    public Collider fieldArea; // 田地的Collider区域

    public bool HasFlower()
    {
        // 检查田地内是否有Tag为Pickable的花朵
        Collider[] colliders = Physics.OverlapBox(fieldArea.bounds.center, fieldArea.bounds.extents, Quaternion.identity);
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Pickable"))
            {
                return true;
            }
        }
        return false;
    }

    public void PlantFlower(GameObject flowerPrefab)
    {
        // 在田地的中心位置生成花朵
        Vector3 spawnPosition = fieldArea.bounds.center;
        Instantiate(flowerPrefab, spawnPosition, Quaternion.identity);
    }

    //private Vector3 GetRandomPointInBounds(Bounds bounds)
    //{
    //    return new Vector3(
    //        Random.Range(bounds.min.x, bounds.max.x),
    //        bounds.center.y, // 假设花朵都在地面上生成
    //        Random.Range(bounds.min.z, bounds.max.z)
    //    );
    //}
}
