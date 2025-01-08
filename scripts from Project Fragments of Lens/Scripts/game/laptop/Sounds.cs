using com;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sounds : MonoBehaviour
{
    public void MouseClick() 
    {
        SoundSystem.instance.Play("mouseclick");
    }

    public void PageTurn()
    {
        SoundSystem.instance.Play("pageturn");
    }
}
