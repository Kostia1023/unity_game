using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MoveController
{
    public float radius = 2f; // Радіус квадрата руху
    public string targetTag = "Player";
    public float minPauseTime = 3f; // Мінімальний час паузи після досягнення точки
    public float maxPauseTime = 8f; // Максимальний час паузи після досягнення точки

    public CharacterController characterController;

    private GameObject target;
    private Vector3 originalPosition;
    private Vector3 randomDestination;
    private bool isMoving;
    private bool isPaused;
    private float pauseTimer;

    private void Start()
    {
        target = null;
        originalPosition = transform.position;
        SetRandomDestination();

        if (target == null)
        {
            isMoving = true;
        }
    }

    private void Update()
    {
        if (target == null)
        {
            FindPlayerTarget();
        }
        else
        {
            float distance = Vector3.Distance(transform.position, target.transform.position);
            if (distance > radius)
            {
                target = null;
                isMoving = true;
            }
            else
            {
                MoveTowardsTarget();
            }
        }

        if (isMoving)
        {
            MoveWithinSquare();
        }
        else if (isPaused)
        {
            pauseTimer -= Time.deltaTime;
            if (pauseTimer <= 0f)
            {
                isMoving = true;
                isPaused = false;
                SetRandomDestination();
            }
        }
    }

    private void FindPlayerTarget()
    {
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

    private void MoveTowardsTarget()
    {
        Vector3 direction = (target.transform.position - transform.position).normalized;
        transform.LookAt(target.transform);
        characterController.Move(getSpeed(direction));
    }

    private void MoveWithinSquare()
    {
        if (Vector3.Distance(transform.position, randomDestination) <= 0.1f)
        {
            isMoving = false;
            isPaused = true;
            pauseTimer = Random.Range(minPauseTime, maxPauseTime);
        }
        else
        {
            Vector3 direction = (randomDestination - transform.position).normalized;
            characterController.Move(getSpeed(direction*0.4f));
        }
    }

    private void SetRandomDestination()
    {
        float randomX = Random.Range(originalPosition.x - radius, originalPosition.x + radius);
        float randomZ = Random.Range(originalPosition.z - radius, originalPosition.z + radius);
        randomDestination = new Vector3(randomX, originalPosition.y, randomZ);
    }
}
