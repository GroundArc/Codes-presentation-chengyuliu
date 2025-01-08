using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyFollow : MonoBehaviour
{
    private GameObject player;
    public float Minmodifier = 7f;
    public float Maxmodifier = 12f;

    Vector3 velocity = Vector3.zero;
    bool isFollowing = false;
    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }
    public void StartFollowing()
    {
        isFollowing = true;
    }
    void Update()
    {
        if (player == null)
        {
            return;
        }

        if (isFollowing)
        {
            transform.position = Vector3.SmoothDamp(transform.position, player.transform.position, ref velocity, Time.deltaTime * Random.Range(Minmodifier, Maxmodifier));
        }
    }
}
