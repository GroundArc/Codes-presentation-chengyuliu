using Assets.Scripts.game.InformationBoard;
using com;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

public class CaptureVideoFeedback : MonoBehaviour
{
    public Image whiteScreen;
    public PostProcessProfile ppp_normal;
    public PostProcessProfile ppp_video;
    public PostProcessProfile ppp_capture;

    public PostProcessVolume ppv;
    public GameObject stickerRef;
    private bool _stickerRefPosSet;

    

    // Start is called before the first frame update
    void Start()
    {
        SetPostProcessingVolume_Normal();
        
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(Time.time);
        if (Input.GetKeyDown("x"))
        {
            // Capture();
        }
        if (Input.GetKeyDown("c"))
        {
            SetPostProcessingVolume_Normal();
        }
        if (Input.GetKeyDown("v"))
        {
            SetPostProcessingVolume_Video();
        }
        if (Input.GetKeyDown("t"))
        {
            FreezeTime();
        }
        if (Input.GetKeyDown("y"))
        {
            ResumeTime();
        }
    }


    void FreezeTime()
    {
        StartCoroutine(ReduceTimeScale(1, 0));
    }

    void ResumeTime()
    {
        StartCoroutine(IncreaseTimeScale(1, 1));
    }

    IEnumerator IncreaseTimeScale(float duration, float target)
    {
        var speed = 1 / duration;
        while (GameTime.timeScale < target)
        {
            GameTime.timeScale += speed * Time.deltaTime;
            if (GameTime.timeScale >= target)
            {
                GameTime.timeScale = target;
                yield break;
            }
            yield return null;
        }
    }

    IEnumerator ReduceTimeScale(float duration, float target)
    {
        var speed = 1 / duration;
        while (GameTime.timeScale > target)
        {
            GameTime.timeScale -= speed * Time.deltaTime;
            if (GameTime.timeScale <= target)
            {
                GameTime.timeScale = target;
                yield break;
            }
            yield return null;
        }
    }
    public void SetPostProcessingVolume_Video()
    {
        ppv.profile = ppp_video;
    }
    public void SetPostProcessingVolume_Normal()
    {
        ppv.profile = ppp_normal;
    }

    void Flash()
    {
        whiteScreen.color = new Color(1, 1, 1, 0);
        whiteScreen.DOKill();
        whiteScreen.DOColor(new Color(1, 1, 1, 1), 0.35f).OnComplete(

           () => { whiteScreen.DOColor(new Color(1, 1, 1, 0), 0.1f); }
            );

    }
    public void Capture(StickerInformation proto)
    {
        if (!_stickerRefPosSet)
        {
            _stickerRefStartEular = stickerRef.transform.eulerAngles;
            _stickerRefStartPos = stickerRef.transform.position;
            //  Debug.Log(_stickerRefStartEular);
            //   Debug.Log(_stickerRefStartPos);
            _stickerRefPosSet = true;
        }

        StartCoroutine(CaptureCoroutine(proto));
        //sfx
        //mono color
        //pause anim
    }

    public Transform stickerRefRotateTarget;
    public Transform stickerRefDiscardTarget;
    private Vector3 _stickerRefStartEular;
    private Vector3 _stickerRefStartPos;

    IEnumerator CaptureCoroutine(StickerInformation proto)
    {
        Debug.Log("1  GameTime.timeScale" + GameTime.timeScale);
        ppv.profile = ppp_capture;
        StartCoroutine(ReduceTimeScale(0.3f, 0));
        yield return new WaitForSeconds(0.35f);
        Flash();
        yield return new WaitForSeconds(0.3f);
        //stickerRef.SetActive(true);

        StickerBehaviour sticker = Instantiate(proto.stickerPrefab, stickerRef.transform.parent);
        sticker.Init(proto); // 设置贴纸的初始属性

        sticker.transform.localScale = 1500 * Vector3.one;
        var gs = sticker.gameObject.GetComponentsInChildren<Transform>(true);
        foreach (var g in gs)
        {
            g.gameObject.layer = 5;
        }
        sticker.transform.GetChild(0).localEulerAngles = new Vector3(0, 0, 0);
        sticker.transform.eulerAngles = _stickerRefStartEular;
        sticker.transform.position = _stickerRefStartPos;
        sticker.transform.DORotate(stickerRefRotateTarget.eulerAngles, 1.5f, RotateMode.FastBeyond360).SetEase(Ease.InOutBack);
        ppv.profile = ppp_video;
        yield return new WaitForSeconds(1.6f);
        sticker.transform.DOMove(stickerRefDiscardTarget.position, 0.7f).SetEase(Ease.InBack);

        StartCoroutine(IncreaseTimeScale(0.3f, 1));

        yield return new WaitForSeconds(0.7f);
        // stickerRef.SetActive(false);
        Destroy(sticker.gameObject);

        Debug.Log("2  GameTime.timeScale" + GameTime.timeScale);
    }
}