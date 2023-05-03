using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MoveController
{
    public float speed = 5.0f; // швидкість руху персонажа
    public float boostMultiplier = 2.0f; // множник для швидкості в разі дії пристрою

    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal"); // отримати значення горизонтального вводу (A та D)
        float verticalInput = Input.GetAxis("Vertical"); // отримати значення вертикального вводу (W та S)

        currentSpeed = speed * slowdownPower; // задати поточну швидкість руху

        if (Input.GetKey(KeyCode.LeftShift)) // якщо зажата клавіша Shift
        {
            currentSpeed *= boostMultiplier; // збільшити швидкість руху на множник boostMultiplier
        }
        Move(horizontalInput, verticalInput);
    }
}
