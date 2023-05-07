using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackScript : MonoBehaviour
{
    public Animator anim;

    public float attackRange = 5f; 
    public int attackDamage = 15;
    public LayerMask enemyLayer;

    private bool canAttack = true; 
    private float attackCooldown = 1.5f; 

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        if (Input.GetMouseButtonDown(0) && canAttack)
        {
            anim.SetTrigger("attack");

            Attack();
        }
    }

    private void Attack()
    {
        canAttack = false;
        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, attackRange, enemyLayer);

        if (hitEnemies.Length > 0)
        {
            foreach (Collider enemy in hitEnemies)
            {
                EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();

                if (enemyHealth != null)
                {
                    enemyHealth.TakeDamage(attackDamage);
                }
            }
        }

        StartCoroutine(ResetAttackCooldown());
    }

    private System.Collections.IEnumerator ResetAttackCooldown()
    {
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }
}
