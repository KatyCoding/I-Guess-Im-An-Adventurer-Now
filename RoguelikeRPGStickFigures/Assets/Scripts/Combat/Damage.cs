using UnityEngine;
[System.Serializable]
public class Damage : Stat
{
    public Damage(int BaseValue) : base(BaseValue)
    {

    }

    public enum DamageType
    {
        Slashing,
        Piercing,
        Bludgeoning,
        Fire,
        Ice,
        Poison,
    }

    public DamageType TypeOfDamage;
}
