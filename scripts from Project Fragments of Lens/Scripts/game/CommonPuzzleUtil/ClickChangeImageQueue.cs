using com;
using UnityEngine;

public class ClickChangeImageQueue : MonoBehaviour
{
    public GameObject[] queue;
    public int crtIndex;
    public string sfxId;

    private void Start()
    {
        Set(0);
    }

    public void Next()
    {
        Set(crtIndex + 1);
    }

    public void Set(int index)
    {
        var len = queue.Length;
        crtIndex = index % len;
        var i = index % len;

        for (int j = 0; j < len; j++)
            queue[j].SetActive(j == i);
    }

    public void OnClick()
    {
        Next();
        SoundSystem.instance.Play(sfxId);
    }
}