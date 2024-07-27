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
    [System.Serializable]
    public class AttackDataAnimOverrideWrapper
    {
        public enum AnimationParameterType
        {
            BOOL,
            TRIGGER,
            INT,
            FLOAT
        }

        public AttackDataScriptableObject data;
        public string AnimationParameter;
        public AnimationParameterType ParameterType;
    }
    public List<AttackDataAnimOverrideWrapper> Attacks = new List<AttackDataAnimOverrideWrapper>();
    public AttackDataAnimOverrideWrapper GetAttackRandom(AttackDataScriptableObject.AttackModifierType type)
    {
        if(Attacks==null||Attacks.Count==0) return null;
        AttackDataAnimOverrideWrapper attack = null;
        while(attack==null || attack.data.MainStat!=type)
        {
            attack = Attacks[Random.Range(0, Attacks.Count)];
        }
        return attack;
    }
}
