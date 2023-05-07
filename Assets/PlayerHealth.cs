using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100; // Максимальне здоров'я гравця
    public Slider healthSlider; // Слайдер здоров'я

    private float currentHealth; // Поточне здоров'я гравця
    private bool canTakeDamage = true; // Можливість отримання урону

    private void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
    }

    public void TakeDamage(float damageAmount)
    {
        if (canTakeDamage)
        {
            currentHealth -= damageAmount;
            UpdateHealthUI();

            if (currentHealth <= 0)
            {
                Die();
            }

            canTakeDamage = false;
            Invoke("EnableDamage", 1f); // Затримка у 1 секунду перед можливістю отримання наступного урону
        }
    }

    private void Die()
    {
        // Виконайте дії, пов'язані зі смертю гравця тут
    }

    private void UpdateHealthUI()
    {
        healthSlider.value = currentHealth/ maxHealth;
    }

    private void EnableDamage()
    {
        canTakeDamage = true;
    }
}
