using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FlowerEditorUI : MonoBehaviour
{
    public GameObject canvas; // 特定的Canvas
    public TextMeshProUGUI hintTextBox; // 提示框
    public Animator wrapperTrayAnimator;
    public Animator accessoryTrayAnimator;
    public Animator flowerTrayAnimator;
    public Animator flowerColorTrayAnimator;
    public FlowerTray flowerTray; // 花朵托盘

    private Animator currentOpenAnimator = null; // 记录当前打开的托盘

    void Start()
    {
        // 隐藏Canvas
        canvas.SetActive(false);
    }

    public void EnterFlowerEditorMode()
    {
        // 激活Canvas
        canvas.SetActive(true);
    }

    public void ExitFlowerEditorMode()
    {
        // 隐藏Canvas
        canvas.SetActive(false);
    }

    public void OnSelectWrapperButtonClicked()
    {
        // 更新提示框文本
        hintTextBox.text = "Select Wrapper";
        StartCoroutine(ToggleTray(wrapperTrayAnimator));
    }

    public void OnSelectAccessoryButtonClicked()
    {
        // 更新提示框文本
        hintTextBox.text = "Select Accessory";
        StartCoroutine(ToggleTray(accessoryTrayAnimator));
    }

    public void OnSelectFlowerButtonClicked()
    {
        // 更新提示框文本
        hintTextBox.text = "Select Flower";
        StartCoroutine(ToggleTray(flowerTrayAnimator, true));
    }

    public void OnSelectFlowerColorButtonClicked()
    {
        // 更新提示框文本
        hintTextBox.text = "Select Flower Color";
        StartCoroutine(ToggleTray(flowerColorTrayAnimator));
    }

    private IEnumerator ToggleTray(Animator targetAnimator, bool refreshFlowers = false)
    {
        // 先关闭当前打开的托盘
        if (currentOpenAnimator != null && currentOpenAnimator != targetAnimator)
        {
            currentOpenAnimator.SetBool("isOpen", false);
            yield return new WaitUntil(() => currentOpenAnimator.GetCurrentAnimatorStateInfo(0).IsName("Closed"));
        }

        // 打开新的托盘
        targetAnimator.SetBool("isOpen", true);
        currentOpenAnimator = targetAnimator;

        // 如果需要刷新花朵显示，调用 DisplayFlowers 方法
        //if (refreshFlowers && flowerTray != null)
        //{
            //flowerTray.DisplayFlowers();
       // }
    }
}
