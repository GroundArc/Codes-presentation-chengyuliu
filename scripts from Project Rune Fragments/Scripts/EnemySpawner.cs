using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private WaveController[] waves;
    [SerializeField] private float timeBetweenWaves;
    [SerializeField] private float waveCountdown;
    [SerializeField] private Transform[] spawners;
    [SerializeField] private GameObject[] enemyTemplate;
    [SerializeField] private GameObject BossTemplate;
    [SerializeField] private int maxEnemiesInRoom;
    public enum SpawnState { SPAWNING, WAITING, COUNTING };
    private SpawnState state = SpawnState.COUNTING;
    private int currentWave = 0;
    private bool bossSpawned = false;
    private bool playerInRoom = false;
    private int currentEnemiesInRoom = 0;
    private int extraEnemies = 0;
    private PlayerInventory playerInventory;

    private void Start()
    {
        waveCountdown = timeBetweenWaves;
        GlobalEnemyEvents.Instance.OnEnemyDeath += OnEnemyDied;
    }

    private void Update()
    {
        if (!playerInRoom)
        {
            return;
        }

        if (waveCountdown <= 0)
        {
            if (state != SpawnState.SPAWNING)
            {
                StartCoroutine(SpawnWave(waves[currentWave]));
                waveCountdown = timeBetweenWaves;
                state = SpawnState.COUNTING;
            }
        }
        else
        {
            waveCountdown -= Time.deltaTime;
        }
    }

    private IEnumerator SpawnWave(WaveController wave)
    {
        state = SpawnState.SPAWNING;

        for (int i = 0; i < wave.enemyAmount + extraEnemies; i++)
        {
            if (currentEnemiesInRoom < maxEnemiesInRoom)
            {
                SpawnEnemy();
                currentEnemiesInRoom++;
            }
            else
            {
                // Debug.Log("Reached maximum enemies in room!");
                break;
            }
            yield return new WaitForSeconds(wave.delayBetweenEnemies);
        }

        state = SpawnState.WAITING;

        yield break;
    }


    private void SpawnEnemy()
    {

        Transform spawner = spawners[Random.Range(0, spawners.Length)];
        if (enemyTemplate.Length > 0)
        {
            if (!bossSpawned && BossTemplate != null)
            {
                GameObject enemyBoss = Instantiate(BossTemplate, spawner.position, spawner.rotation);
                bossSpawned = true;
                Debug.Log("Boss spawned");
            }
            else
            {
                GameObject enemy = Instantiate(enemyTemplate[Random.Range(0, enemyTemplate.Length)], spawner.position, spawner.rotation);
            }
            // Debug.Log("Enemy spawned");
        }
        GlobalEnemyEvents.Instance.UpdateEnemyList();
        GlobalEnemyEvents.Instance.UpdatePlayerList();
    }

    public void StartSpawning()
    {
        playerInRoom = true;
        waveCountdown = timeBetweenWaves;
    }

    public void StopSpawning()
    {
        playerInRoom = false;
        waveCountdown = timeBetweenWaves;
    }

    public void OnEnemyDied()
    {
        if (currentEnemiesInRoom > 0)
            currentEnemiesInRoom--;
    }

    private void OnEnable()
    {
        if (PlayerInventory.Instance != null)
        {
            SubscribeToFragmentsCollected();
        }
        else
        {
            PlayerInventory.OnInstanceReady += OnPlayerInventoryReady;
        }
    }

    private void OnPlayerInventoryReady()
    {
        PlayerInventory.OnInstanceReady -= OnPlayerInventoryReady;
        SubscribeToFragmentsCollected();
    }

    private void SubscribeToFragmentsCollected()
    {
        playerInventory = PlayerInventory.Instance;
        if (playerInventory == null)
        {
            Debug.LogWarning("PlayerInventory is null on: " + gameObject.name);
        }
        else if (playerInventory.OnFragmentsCollected == null)
        {
            Debug.LogWarning("OnFragmentsCollected is null on: " + gameObject.name);
        }
        else
        {
            playerInventory.OnFragmentsCollected.AddListener(UpdateWaveSetting);
        }
    }

    public void UpdateWaveSetting()
    {
        switch (playerInventory.fragments)
        {
            case 0:
                timeBetweenWaves = 4;
                extraEnemies = 0;
                break;
            case 1:
                timeBetweenWaves = 3;
                extraEnemies = 0;
                break;
            case 2:
                timeBetweenWaves = 3;
                extraEnemies = 1;
                break;
            case 3:
                timeBetweenWaves = 2;
                extraEnemies = 1;
                break;
            case 4:
                timeBetweenWaves = 2;
                extraEnemies = 2;
                break;
            case 5:
                timeBetweenWaves = 1;
                extraEnemies = 2;
                break;
            default: break;
        }
    }
}