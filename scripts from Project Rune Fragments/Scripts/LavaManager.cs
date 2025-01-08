using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (GameManager.isGameOver)
        {
            return;
        }
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Player is in lava");
            other.gameObject.GetComponent<HealthManager>().TakeDamage(1000);
        }
    }
}
