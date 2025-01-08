using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarManager : MonoBehaviour
{
    Slider healthSlider;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private bool isPlayer;

    private void Start()
    {
        healthSlider = GetComponentInChildren<Slider>();
    }


    public void SetHealth(float health)
    {
        healthSlider.value = health;
        if (isPlayer)
        {
            healthText.text = health.ToString("0") + "/200";
        }
    }
}
