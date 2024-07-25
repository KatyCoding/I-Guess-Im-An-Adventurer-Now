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
    public Damage Damage;
    public AttackModifierType MainStat;
}
