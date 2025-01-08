using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationSceneInstance : MonoBehaviour
{
    [Header("播放这个动画短片最大时长")]
    public float duration;
    [Header("播放这个动画短片时的摄像机位")]
    public Transform cameraPos;
    [Header("播放这个动画短片时要关闭的对象")]
    public GameObject[] toDisactivates;
    [Header("播放这个动画短片时要启用的对象")]
    public GameObject[] toActivates;
    [Header("非必要，播放这个动画短片时的背景音乐")]
    public AudioSource bgSound;

    void Start()
    {

    }
    void Update()
    {

    }
}
