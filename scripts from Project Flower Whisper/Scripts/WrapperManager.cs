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


    public List<Wrapper> wrappers = new List<Wrapper>(); // �� Inspector ����ʾ��ǰ��װ�б�

    public int space = 7;

    // ��������Ӱ�װ���б�
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

    // ���������б����Ƴ���װ
    public void Remove(Wrapper wrapper)
    {
        wrappers.Remove(wrapper);

        onWrapperChangedCallBack?.Invoke();
    }
}
