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

    public List<Flower> flowers = new List<Flower>(); // 在 Inspector 中显示当前花朵列表

    public int space = 7;

    // 方法：添加花朵到列表
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

    // 方法：从列表中移除花朵
    public void Remove(Flower flower)
    {
        Debug.Log("Removing flower: " + flower.flowerName);
        flowers.Remove(flower);

        onFlowerChangedCallBack?.Invoke();
    }
}
