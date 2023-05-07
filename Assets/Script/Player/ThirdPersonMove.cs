using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonMove : MonoBehaviour
{
    public CharacterController characterController;
    public Transform cam;
    public float speed = 5f;
    public float rotTime = 5f;
    public float rotSpeed = 5f;


    private void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 movement = new Vector3(horizontal, 0, vertical).normalized;

        if (movement.magnitude >= 0.1f)
        {
            // Обчислити напрямок руху персонажа
            Vector3 moveDir = Quaternion.Euler(0f, cam.eulerAngles.y, 0f) * movement;

            // Обчислити напрямок взгляду камери
            Vector3 cameraDirection = cam.transform.forward;
            cameraDirection.y = 0;

            // Обчислити кут між напрямком взгляду камери та напрямком руху персонажа
            float targetAngle = Vector3.Angle(cameraDirection, moveDir);
            if (moveDir.x < 0)
            {
                targetAngle = -targetAngle;
            }

            // Повернути персонажа
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0f, cam.eulerAngles.y + targetAngle, 0f), rotSpeed * Time.deltaTime);

            // Рухати персонажа
            characterController.Move(moveDir.normalized * speed * Time.deltaTime);
        }
    }
}
