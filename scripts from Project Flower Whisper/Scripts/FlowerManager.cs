using System.Collections.Generic;
using UnityEngine;

public class FlowerManager : MonoBehaviour
{
    #region Singleton
    public static FlowerManager instance;

    void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of FlowerManager found!");
            return;
        }
        instance = this;
    }
    #endregion

    public delegate void OnFlowerChanged();
    public OnFlowerChanged onFlowerChangedCallBack;

    public List<Flower> flowers = new List<Flower>(); // �� Inspector ����ʾ��ǰ�����б�

    public int space = 7;

    // ��������ӻ��䵽�б�
    public bool Add(Flower flower)
    {
        if (flowers.Count >= space)
        {
            Debug.Log("Not enough room.");
            return false;
        }

        Debug.Log("Adding flower: " + flower.flowerName);
        flowers.Add(flower);

        onFlowerChangedCallBack?.Invoke();

        return true;
    }

    // ���������б����Ƴ�����
    public void Remove(Flower flower)
    {
        Debug.Log("Removing flower: " + flower.flowerName);
        flowers.Remove(flower);

        onFlowerChangedCallBack?.Invoke();
    }
}
