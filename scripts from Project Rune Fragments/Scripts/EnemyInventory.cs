using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyInventory : MonoBehaviour
{
    public UnityEvent onEnemyBribed;
    public UnityEvent onEnemyTakenGluttonyAbility;
    public string enemyStateText { get; private set; }
    private EnemyManager enemyManager;

    // Update is called once per frame
    private void Start()
    {
        enemyManager = GetComponent<EnemyManager>();
        enemyStateText = " ";
    }

    public void BribeEnemy()
    {
        // Debug.Log("Enemy Bribed in EnemyInventory.cs");
        enemyStateText = "$BRIBED$";
        onEnemyBribed.Invoke();
    }

    public void TakenGluttonyAbility()
    {
        onEnemyTakenGluttonyAbility?.Invoke();
    }
}
