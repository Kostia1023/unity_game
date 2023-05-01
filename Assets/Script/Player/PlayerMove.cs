using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float speed = 5.0f; // �������� ���� ���������
    public float boostMultiplier = 2.0f; // ������� ��� �������� � ��� 䳿 ��������

    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal"); // �������� �������� ��������������� ����� (A �� D)
        float verticalInput = Input.GetAxis("Vertical"); // �������� �������� ������������� ����� (W �� S)

        float currentSpeed = speed; // ������ ������� �������� ����

        if (Input.GetKey(KeyCode.LeftShift)) // ���� ������ ������ Shift
        {
            currentSpeed *= boostMultiplier; // �������� �������� ���� �� ������� boostMultiplier
        }

        Vector3 movement = new Vector3(horizontalInput, 0, verticalInput) * currentSpeed * Time.deltaTime; // ��������� ������ ����

        transform.Translate(movement, Space.Self); // ������ ������� ���������
    }
}
