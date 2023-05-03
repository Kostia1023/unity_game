using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MoveController
{
    public float speed = 5.0f; // �������� ���� ���������
    public float boostMultiplier = 2.0f; // ������� ��� �������� � ��� 䳿 ��������

    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal"); // �������� �������� ��������������� ����� (A �� D)
        float verticalInput = Input.GetAxis("Vertical"); // �������� �������� ������������� ����� (W �� S)

        currentSpeed = speed * slowdownPower; // ������ ������� �������� ����

        if (Input.GetKey(KeyCode.LeftShift)) // ���� ������ ������ Shift
        {
            currentSpeed *= boostMultiplier; // �������� �������� ���� �� ������� boostMultiplier
        }
        Move(horizontalInput, verticalInput);
    }
}
