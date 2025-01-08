using System.Collections;
using UnityEngine;
using DG.Tweening;
using com;

public class ClickSpeak : MonoBehaviour
{
    public GameObject[] speachs;
    public float duration = 7;

    public string sfx;
    GameObject _crtSpeach;
    int _index;

    private void Start()
    {
        _crtSpeach = null;
        _index = 0;
    }

    public void Speak()
    {
        SoundSystem.instance.Play(sfx);

        _index++;
        if (_index >= speachs.Length)
            _index = 0;

        _crtSpeach = speachs[_index];
        foreach (var s in speachs)
        {
            s.transform.DOKill();
            if (s == _crtSpeach)
            {
                s.SetActive(true);
                s.transform.DOShakePosition(2, 7).OnComplete(
                () =>
                {
                    s.SetActive(false);
                    _crtSpeach = null;
                }
                );
            }
            else
                s.SetActive(false);
        }
    }
}