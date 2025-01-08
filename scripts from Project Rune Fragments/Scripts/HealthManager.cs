using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class HealthManager : MonoBehaviour
{
    [SerializeField] private int totalHealth;
    [SerializeField] private UnityEvent IsDeath;
    [SerializeField] private UnityEvent IsTakeDamage;
    [SerializeField] private UnityEvent<float> HealthChanged;

    private HealthBarManager healthBarManager;
    private float currentHealth;
    public float CurrentHealth
    {
        get { return this.currentHealth; }
    }
    void Start()
    {
        this.healthBarManager = FindObjectOfType<HealthBarManager>();
        currentHealth = totalHealth;
    }


    public void TakeDamage(float damage)
    {
        this.currentHealth -= damage;
        var healthPercentage = (float)this.currentHealth / this.totalHealth;
        this.HealthChanged?.Invoke(healthPercentage);
        this.healthBarManager.SetHealth(currentHealth);
        if (currentHealth <= 0)
        {
            this.IsDeath?.Invoke();
            Destroy(this.gameObject);
            if (this.gameObject.tag == "Enemy")
            {
                GlobalEnemyEvents.Instance.EnemyDied();
            }
            if (GameManager.isGameOver == false && this.gameObject == FindObjectOfType<PlayerCharacter>().gameObject)
            {
                FindObjectOfType<GameManager>().GameOver();
            }
        }
        else
        {
            this.IsTakeDamage?.Invoke();
        }
    }
    public void RestoreFullHealth()
    {
        this.currentHealth = this.totalHealth;
        this.HealthChanged?.Invoke(1f);
        this.healthBarManager.SetHealth(currentHealth);
        Debug.Log("Health: " + currentHealth);
    }
}