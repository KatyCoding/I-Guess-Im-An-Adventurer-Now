using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
[CreateAssetMenu(fileName = "CombatantData", menuName = "Combatants/CombatantData")]
public class CombatantData : ScriptableObject
{
    
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
    private void OnEnable()
    {
        attackAnimRef.Clear();
        Attacks.Clear();
        foreach(var a in AttackAnims)
        {
            attackAnimRef.Add(a.data, a);
            Attacks.Add(a.data);
        }
    }
    [SerializeField]protected List<AttackDataAnimOverrideWrapper> AttackAnims = new List<AttackDataAnimOverrideWrapper>();
    [HideInInspector] public List<AttackDataScriptableObject> Attacks = new List<AttackDataScriptableObject>();
    private Dictionary<AttackDataScriptableObject, AttackDataAnimOverrideWrapper> attackAnimRef = new Dictionary<AttackDataScriptableObject, AttackDataAnimOverrideWrapper>();
    public AttackDataScriptableObject GetAttackRandom(AttackDataScriptableObject.AttackModifierType type)
    {
        if(Attacks==null||Attacks.Count==0) return null;
        AttackDataScriptableObject attack = null;
        int fallback = 1000;
        while(attack==null || attack.MainStat!=type)
        {
            if (fallback < 0) break;
            fallback--;
            attack = Attacks[Random.Range(0, Attacks.Count)];
        }
        return attack;
    }
    public AttackDataAnimOverrideWrapper GetAnimationInfo(AttackDataScriptableObject attack)
    {
        if(attackAnimRef.ContainsKey(attack))
        {
            return attackAnimRef[attack];
        }
        return null;
           
    }
}
