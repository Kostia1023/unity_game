using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MoveController
{
    public float radius;
    public string targetTag;

    private GameObject target;

    private void Start()
    {
        target = null;
    }

    private void Update()
    {
        if (target == null)
        {

            // Find player with target tag
            GameObject[] targets = GameObject.FindGameObjectsWithTag(targetTag);
            foreach (GameObject potentialTarget in targets)
            {
                float distance = Vector3.Distance(transform.position, potentialTarget.transform.position);
                if (distance <= radius)
                {
                    target = potentialTarget;
                    break;
                }
            }
        }
        else
        {
            float distance = Vector3.Distance(transform.position, target.transform.position);
            if (distance > radius)
            {
                target = null;
            }
            else
            {
                // Move towards target
                Vector3 direction = (target.transform.position - transform.position).normalized;
                Move(direction.x, direction.z);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Attack attack = gameObject.GetComponent<EnemyAttack>();
            attack.ToAttack(other.gameObject.GetComponent<HP>());
        }
    }
}
