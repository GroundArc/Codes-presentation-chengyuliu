using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreedAbility : MonoBehaviour
{
    [SerializeField] private GameObject MoneyPrefab;      // Assign the gold coin projectile prefab in the editor
    private Transform projectileSpawnPoint; // Assign a spawn point for the gold coin projectile in the editor
    [SerializeField] private AudioClip moneyShotSound;

    public int Money = 0;
    private void Start()
    {
        projectileSpawnPoint = GameObject.Find("Muzzel").transform;
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            PlayerInventory playerInventory = gameObject.GetComponent<PlayerInventory>();

            if (playerInventory.money > 0)
            {
                ShootMoney();
                playerInventory.MoneyCollected(-1);
            }
            else
            {
                Debug.Log("No Money");
            }

        }
    }

    private void ShootMoney()
    {
        if (moneyShotSound != null)
        {
            AudioSource.PlayClipAtPoint(moneyShotSound, transform.position);
        }
        Vector3 direction = projectileSpawnPoint.forward;
        Quaternion bulletRotation = Quaternion.LookRotation(direction);
        GameObject Money = Instantiate(MoneyPrefab, projectileSpawnPoint.position, bulletRotation);
        ProjectileManager projectileManager = Money.GetComponent<ProjectileManager>();
        if (projectileManager)
        {
            projectileManager.isMoney = true;
            //Debug.Log("Money is true");
        }
        direction.y = 0;
    }
}
