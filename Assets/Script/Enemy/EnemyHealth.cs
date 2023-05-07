using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float maxHealth = 20; 

    private float currentHealth;
    private bool canTakeDamage = true; 

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damageAmount)
    {
        if (canTakeDamage)
        {
            currentHealth -= damageAmount;

            if (currentHealth <= 0)
            {
                Die();
            }

            canTakeDamage = false;
            Invoke("EnableDamage", 1f);
        }
    }

    private void Die()
    {
        gameObject.SetActive(false);
    }

    private void EnableDamage()
    {
        canTakeDamage = true;
    }
}
