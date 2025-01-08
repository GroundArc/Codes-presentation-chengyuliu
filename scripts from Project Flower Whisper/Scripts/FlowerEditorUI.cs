using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FlowerEditorUI : MonoBehaviour
{
    public GameObject canvas; // �ض���Canvas
    public TextMeshProUGUI hintTextBox; // ��ʾ��
    public Animator wrapperTrayAnimator;
    public Animator accessoryTrayAnimator;
    public Animator flowerTrayAnimator;
    public Animator flowerColorTrayAnimator;
    public FlowerTray flowerTray; // ��������

    private Animator currentOpenAnimator = null; // ��¼��ǰ�򿪵�����

    void Start()
    {
        // ����Canvas
        canvas.SetActive(false);
    }

    public void EnterFlowerEditorMode()
    {
        // ����Canvas
        canvas.SetActive(true);
    }

    public void ExitFlowerEditorMode()
    {
        // ����Canvas
        canvas.SetActive(false);
    }

    public void OnSelectWrapperButtonClicked()
    {
        // ������ʾ���ı�
        hintTextBox.text = "Select Wrapper";
        StartCoroutine(ToggleTray(wrapperTrayAnimator));
    }

    public void OnSelectAccessoryButtonClicked()
    {
        // ������ʾ���ı�
        hintTextBox.text = "Select Accessory";
        StartCoroutine(ToggleTray(accessoryTrayAnimator));
    }

    public void OnSelectFlowerButtonClicked()
    {
        // ������ʾ���ı�
        hintTextBox.text = "Select Flower";
        StartCoroutine(ToggleTray(flowerTrayAnimator, true));
    }

    public void OnSelectFlowerColorButtonClicked()
    {
        // ������ʾ���ı�
        hintTextBox.text = "Select Flower Color";
        StartCoroutine(ToggleTray(flowerColorTrayAnimator));
    }

    private IEnumerator ToggleTray(Animator targetAnimator, bool refreshFlowers = false)
    {
        // �ȹرյ�ǰ�򿪵�����
        if (currentOpenAnimator != null && currentOpenAnimator != targetAnimator)
        {
            currentOpenAnimator.SetBool("isOpen", false);
            yield return new WaitUntil(() => currentOpenAnimator.GetCurrentAnimatorStateInfo(0).IsName("Closed"));
        }

        // ���µ�����
        targetAnimator.SetBool("isOpen", true);
        currentOpenAnimator = targetAnimator;

        // �����Ҫˢ�»�����ʾ������ DisplayFlowers ����
        //if (refreshFlowers && flowerTray != null)
        //{
            //flowerTray.DisplayFlowers();
       // }
    }
}
