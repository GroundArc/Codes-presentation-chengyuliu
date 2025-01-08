using UnityEngine;

public class DestroyChildren : MonoBehaviour
{

    void Update()
    {
        // ����Ƿ�����H��
        if (Input.GetKeyDown(KeyCode.H))
        {
            DestroyAllChildren();
        }
    }
    // �������ݻ������Ӷ���
    public void DestroyAllChildren()
    {
        // ���������Ӷ��󲢴ݻ�����
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }


}
