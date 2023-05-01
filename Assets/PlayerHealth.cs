using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public Slider healthSlider;
    public GameObject MoveObj;

    private float health = 100f;
    private float maxHeath = 100f;
    private float timeInvulnerability = 0.5f;
    private float lastTakedDamage = 0;
    void Start()
    {

        healthSlider.value = 1f;
    }

    void FixedUpdate()
    {
        healthSlider.value = GetPlayerHealth();
        if (Time.time - lastTakedDamage > timeInvulnerability)
        {
            
            lastTakedDamage = Time.time;
        }
    }

    void getAttacked(Attack attack)
    {

    }

    float GetPlayerHealth()
    {
        return health / maxHeath;
    }

    void Dead()
    {

    }
}
