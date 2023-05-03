using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class HP : MonoBehaviour
{
    public Slider healthSlider;
    public MoveController MoveObj;
    public Attack AttackObj;

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

    public void getAttacked(AttackData attack)
    {
        switch (attack.debuffType)
        {
            case DebuffType.Freeze: StartCoroutine(MoveObj.Freeze(attack.timeFreeze)); break;
            case DebuffType.Weakness: AttackObj.weakness = attack.weakness; break;
            case DebuffType.Poison: break;
         //  case DebuffType.Slow: MoveObj.slowdownPower = attack.; break;
            default: break;
        }
    }

    float GetPlayerHealth()
    {
        return health / maxHeath;
    }

    void Dead()
    {

    }
}
