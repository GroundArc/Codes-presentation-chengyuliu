using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class CountDown : MonoBehaviour
{
    // countdown timer
    public float timeLeft;

    public TextMeshProUGUI CountDownText;

    private bool _countDown;



    // Start is called before the first frame update
    void Start()
    {
        _countDown = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (_countDown && !GameManager.isGameOver && !GameManager.isGameLost)
        {
            timeLeft -= Time.deltaTime;
            CountDownText.text = (timeLeft).ToString("0");

            if (timeLeft <= 0)
            {
                Debug.Log("Time is up");
                _countDown = false;
                FindObjectOfType<GameManager>().GameEnding();
            }
        }
    }
}

