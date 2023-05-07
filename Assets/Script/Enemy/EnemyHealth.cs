using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float maxHealth = 20;

    private float currentHealth;

    public Animator anim;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damageAmount)
    {

        currentHealth -= damageAmount;

        if (currentHealth <= 0)
        {
            StartCoroutine(Die());
        }

    }

    private System.Collections.IEnumerator Die()
    {
        yield return new WaitForSeconds(0.3f);
        anim.SetTrigger("Die");
        yield return new WaitForSeconds(0.5f);
        gameObject.SetActive(false);
    }

}
