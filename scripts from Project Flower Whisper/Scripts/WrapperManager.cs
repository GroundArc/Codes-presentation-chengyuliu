using System.Collections.Generic;
using UnityEngine;

public class WrapperManager : MonoBehaviour
{
    #region Singleton
    public static WrapperManager instance;

    void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of WrapperManager found!");
            return;
        }
        instance = this;
    }
    #endregion

    public delegate void OnWrapperChanged();
    public OnWrapperChanged onWrapperChangedCallBack;


    public List<Wrapper> wrappers = new List<Wrapper>(); // 在 Inspector 中显示当前包装列表

    public int space = 7;

    // 方法：添加包装到列表
    public bool Add(Wrapper wrapper)
    {
        if (wrappers.Count >= space)
        {
            Debug.Log("Not enough room.");
            return false;
        }
        wrappers.Add(wrapper);

        onWrapperChangedCallBack?.Invoke();

        return true;
    }

    // 方法：从列表中移除包装
    public void Remove(Wrapper wrapper)
    {
        wrappers.Remove(wrapper);

        onWrapperChangedCallBack?.Invoke();
    }
}
