using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100; // ����������� ������'� ������
    public Slider healthSlider; // ������� ������'�

    private float currentHealth; // ������� ������'� ������
    private bool canTakeDamage = true; // ��������� ��������� �����

    private void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
    }

    public void TakeDamage(float damageAmount)
    {
        if (canTakeDamage)
        {
            currentHealth -= damageAmount;
            UpdateHealthUI();

            if (currentHealth <= 0)
            {
                Die();
            }

            canTakeDamage = false;
            Invoke("EnableDamage", 1f); // �������� � 1 ������� ����� ��������� ��������� ���������� �����
        }
    }

    private void Die()
    {
        // ��������� 䳿, ���'���� � ������ ������ ���
    }

    private void UpdateHealthUI()
    {
        healthSlider.value = currentHealth/ maxHealth;
    }

    private void EnableDamage()
    {
        canTakeDamage = true;
    }
}
