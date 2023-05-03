using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MoveController : MonoBehaviour
{
    public bool canMove = true;
    public float timeCantMove = 0f;
    public float slowdownTime = 0f;
    public float slowdownPower = 0f;
    protected float currentSpeed = 0f;

    public IEnumerator Freeze(float duration)
    {
        canMove = false;
        yield return new WaitForSeconds(duration);
        canMove = true;
    }

    public void Move(float horizontal, float verticalInput)
    {
        if (canMove)
        {
            Vector3 movement = new Vector3(horizontal, 0, verticalInput) * currentSpeed * Time.deltaTime; // ��������� ������ ����

            transform.Translate(movement, Space.Self); // ������ ������� ���������
        }
    }
}
