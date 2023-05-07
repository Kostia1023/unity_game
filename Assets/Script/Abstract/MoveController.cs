using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MoveController : MonoBehaviour
{
    public bool canMove = true;
    public float timeCantMove = 0f;
    public float slowdownTime = 0f;
    public float slowdownPower = 0f;
    public float Speed = 0f;

    public IEnumerator Freeze(float duration)
    {
        canMove = false;
        yield return new WaitForSeconds(duration);
        canMove = true;
    }

    public IEnumerator Slowdown(float duration, float power)
    {
        slowdownPower = power;
        yield return new WaitForSeconds(duration);
        slowdownPower = 0;
    }
    
    public Vector3 getSpeed(Vector3 vector)
    {
        if (!canMove)
        {
            return Vector3.zero;
        }
        return (vector - vector * slowdownPower) * Time.deltaTime * Speed;
    }
}
