using JetBrains.Annotations;
using UnityEngine;

[CreateAssetMenu(fileName = "AttackData", menuName = "Combatant/AttackData")]
public class AttackDataScriptableObject : ScriptableObject
{
    public enum AttackModifierType
    {
        strength,
        dexterity,
        arcana,
        charisma,
        NONE,
    }

    public bool IsRangedAttack = false;
    public BaseProjectile ProjectilePrefab;
    public Damage Damage;
    public AttackModifierType MainStat;
}
