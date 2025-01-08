using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using com;

public class AnimationPlayer : MonoBehaviour
{
    public Animator animator; // ��ק���Animator���
    public string animationName = "YourAnimationName"; // ��Ҫ���ƵĶ�������
    public List<Button> controlButtons; // ��Inspector����ק�����ť
    

    private bool isPlaying = false;

    // �������ԣ����ڷ��ض����Ƿ�����ͣ״̬
    public bool IsPaused => !isPlaying;

    void Start()
    {
        // Ϊÿ����ť��Ӽ����¼�
        foreach (Button button in controlButtons)
        {
            button.onClick.AddListener(ToggleAnimation);
        }

        // ����Ĭ����ͣ
        animator.Play(animationName, 0, 0); // ���������õ���ʼλ��
        animator.speed = 0; // ��ͣ����
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
         
        animator.speed = 1; // ��ʼ���Ŷ���
        isPlaying = true;
    }

    void PauseAnimation()
    {
       
        animator.speed = 0; // ��ͣ����
        isPlaying = false;
    }

   
}
