using UnityEngine;

public enum DebuffType
{
    Freeze,
    Slow,
    Weakness,
    Poison,
    Default
}

public class AttackData
{
    public float damage;
    public float knockback;
    public float periodicDamage;
    public float timeFreeze;
    public float weakness;
    public DebuffType debuffType;
}

public class Attack : MonoBehaviour
{
    public float damage = 0f;
    public float knockback = 0f;
    public float periodicDamage = 0f;
    public float timeFreeze = 0f;
    public float buffDamage = 0f;
    public float buffPeriodicDamage = 0f;
    public float weakness = 0f;
    public float slowdownPower = 0f;
    public float slowdownTime = 0f;

    public DebuffType debuffType;

    public void ToAttack(HP obj)
    {
        obj.getAttacked(new AttackData
        {
            damage = damage * buffDamage * weakness,
            knockback = knockback,
            periodicDamage = periodicDamage * buffPeriodicDamage * weakness,
            timeFreeze = timeFreeze,
            debuffType = debuffType
        });
    }
}
