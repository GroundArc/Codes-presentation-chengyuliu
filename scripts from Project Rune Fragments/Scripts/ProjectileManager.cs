using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
    [SerializeField] private Vector3 projectileSpeed;
    [SerializeField] private String targetTag1;
    [SerializeField] private String targetTag2;
    [SerializeField] private String BossTag;

    [SerializeField] private String buildingTag;

    [SerializeField] private float damage;
    [SerializeField] private ParticleSystemRenderer destroyEffect;
    private float originalDamage;
    private float originalSpeed;
    public bool isMoney = false;
    public bool isPlayerProjectile = false;
    private PlayerInventory playerInventory;
    [SerializeField] private AudioClip hitWallSound;

    private void Start()
    {
        playerInventory = PlayerInventory.Instance;
        if (!isPlayerProjectile)
        {
            GlobalEnemyEvents.Instance.OnProjectDamageChange += SetProjectileDamageWithMultiplier;
            GlobalEnemyEvents.Instance.OnProjectileSpeedChange += SetProjectileSpeedWithMultiplier;
        }

    }
    // Update is called once per frame
    void Update()
    {
        transform.Translate(this.projectileSpeed * Time.deltaTime);
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (GameManager.isGameOver)
        {
            return;
        }

        if (collision.gameObject.tag == targetTag1 || collision.gameObject.tag == targetTag2)
        {
            var enemyManager = collision.gameObject.GetComponent<EnemyManager>();
            if (enemyManager && isMoney)
            {
                EnemyInventory enemyInventory = collision.gameObject.GetComponent<EnemyInventory>();
                enemyManager.Bribe();
                enemyInventory.BribeEnemy();
                playerInventory.GreedAbilityUsed();
                // Debug.Log("Bribing Enemy");
            }
            else
            {
                collision.gameObject.GetComponent<HealthManager>().TakeDamage(damage);
            }
            Destroy(this.gameObject);
            //Debug.Log("Destroying Projectile because of collision with " + collision.gameObject.tag);
        }
        else if (collision.gameObject.tag == buildingTag)
        {
            if (!isMoney)
            {
                if (hitWallSound != null)
                {
                    AudioSource.PlayClipAtPoint(hitWallSound, transform.position);
                }
                Instantiate(destroyEffect, transform.position, transform.rotation * Quaternion.Euler(0, 180, 0));
            }
            Destroy(this.gameObject);
            // Debug.Log("Destroying Projectile because of collision with " + collision.gameObject.tag);
        }
        else if (collision.gameObject.tag == BossTag)
        {
            collision.gameObject.GetComponent<HealthManager>().TakeDamage(damage);
            Destroy(this.gameObject);
        }

    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
        // Debug.Log("Destroying Projectile because of being invisible");
    }

    public void SetProjectileDamageWithMultiplier(float multiplier)
    {
        this.damage = this.originalDamage * multiplier;
    }

    public void SetProjectileSpeedWithMultiplier(float multiplier)
    {
        this.projectileSpeed.z = this.originalSpeed * multiplier;
    }

    public void InitializeBulletAttributes(float damage, float speed)
    {
        this.originalDamage = damage;
        this.originalSpeed = speed;
    }
}

