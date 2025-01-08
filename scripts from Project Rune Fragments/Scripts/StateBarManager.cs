using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class StateBarManager : MonoBehaviour
{

    private EnemyInventory enemyInventory;
    public TextMeshProUGUI FloatingText;
    // // Start is called before the first frame update
    void Start()
    {
        FloatingText.text = " ";
        enemyInventory = transform.parent.GetComponent<EnemyInventory>();
        if (enemyInventory != null && enemyInventory.onEnemyBribed != null)
        {
            // Debug.Log("Listening to onEnemyBribed");
            enemyInventory.onEnemyBribed.AddListener(UpdateStateBar);
        }
    }

    // show the state of the enemy
    public void UpdateStateBar()
    {
        if (enemyInventory != null)
        {
            FloatingText.text = enemyInventory.enemyStateText;
        }
    }
}
