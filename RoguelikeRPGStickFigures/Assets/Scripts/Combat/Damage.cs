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
        Fire,
        Ice,
        Poison,
    }
    public DamageType TypeOfDamage {get; protected set;}
}
