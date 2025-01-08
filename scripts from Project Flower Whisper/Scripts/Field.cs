using UnityEngine;

public class Field : MonoBehaviour
{
    public Collider fieldArea; // ��ص�Collider����

    public bool HasFlower()
    {
        // ���������Ƿ���TagΪPickable�Ļ���
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
        // ����ص�����λ�����ɻ���
        Vector3 spawnPosition = fieldArea.bounds.center;
        Instantiate(flowerPrefab, spawnPosition, Quaternion.identity);
    }

    //private Vector3 GetRandomPointInBounds(Bounds bounds)
    //{
    //    return new Vector3(
    //        Random.Range(bounds.min.x, bounds.max.x),
    //        bounds.center.y, // ���軨�䶼�ڵ���������
    //        Random.Range(bounds.min.z, bounds.max.z)
    //    );
    //}
}
