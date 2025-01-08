using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using com;

public class AnimationPlayer : MonoBehaviour
{
    public Animator animator; // 拖拽你的Animator组件
    public string animationName = "YourAnimationName"; // 你要控制的动画名称
    public List<Button> controlButtons; // 在Inspector中拖拽多个按钮
    

    private bool isPlaying = false;

    // 公共属性，用于返回动画是否处于暂停状态
    public bool IsPaused => !isPlaying;

    void Start()
    {
        // 为每个按钮添加监听事件
        foreach (Button button in controlButtons)
        {
            button.onClick.AddListener(ToggleAnimation);
        }

        // 动画默认暂停
        animator.Play(animationName, 0, 0); // 将动画设置到起始位置
        animator.speed = 0; // 暂停动画
    }

    void ToggleAnimation()
    {
        if (isPlaying)
        {
            PauseAnimation();
        }
        else
        {
            PlayAnimation();
        }
    }

    void PlayAnimation()
    {
         
        animator.speed = 1; // 开始播放动画
        isPlaying = true;
    }

    void PauseAnimation()
    {
       
        animator.speed = 0; // 暂停动画
        isPlaying = false;
    }

   
}
