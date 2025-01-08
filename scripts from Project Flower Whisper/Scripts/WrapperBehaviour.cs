using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WrapperBehaviour : MonoBehaviour
{
    public Wrapper wrapperData; // �����İ�װ����

    void Start()
    {
        if (wrapperData == null)
        {
            Debug.LogError("WrapperData not assigned to WrapperBehaviour on " + gameObject.name);
        }
    }

    // ������������Ӱ�װ���ض���Ϊ�ͷ��������磺
    // - ����װ�Ƿ���ĳ��������
    // - ������װ���ض�������Ч��
    // - �����װ��״̬������

    // ʾ����������ӡ��װ����
    public void PrintWrapperName()
    {
        if (wrapperData != null)
        {
            Debug.Log("Wrapper name: " + wrapperData.wrapperName);
        }
    }
}
