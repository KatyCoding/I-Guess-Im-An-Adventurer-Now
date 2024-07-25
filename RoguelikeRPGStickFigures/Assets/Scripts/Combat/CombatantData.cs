using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
[CreateAssetMenu(fileName = "CombatantData", menuName = "Combatants/CombatantData")]
public class CombatantData : ScriptableObject
{
    public Stat Strength;
    public Stat Dexterity;
    public Stat Arcana;
    public Stat Charisma;
    public Stat Constitution;
    public List<AttackDataScriptableObject> Attacks = new List<AttackDataScriptableObject>();
    public AttackDataScriptableObject GetAttackRandom(AttackDataScriptableObject.AttackModifierType type)
    {
        if(Attacks==null||Attacks.Count==0) return null;
        AttackDataScriptableObject attack = null;
        while(attack==null || attack.MainStat!=type)
        {
            attack = Attacks[Random.Range(0, Attacks.Count)];
        }
        return attack;
    }
}
