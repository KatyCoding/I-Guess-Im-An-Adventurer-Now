using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "CombatantData", menuName = "Combatants/CombatantData")]
public class CombatantData : ScriptableObject
{
    
    [System.Serializable]
    public class AttackDataAnimOverrideWrapper
    {
        public AttackDataScriptableObject AttackData;
        public AnimatorController AnimController;
        public AnimatorControllerParameter param => AnimController?.parameters[parameterIndex];// => bs.param;

        [SerializeField] private int parameterIndex =0;
        //[SerializeField] private BullshitWrapper bs = new BullshitWrapper();
    }
    [System.Serializable]
    public class BullshitWrapper
    {
        public AnimatorControllerParameter param = new AnimatorControllerParameter();
    }
    
    private void OnEnable()
    {
        attackAnimRef.Clear();
        Attacks.Clear();
        foreach(var a in AttackAnims)
        {
            attackAnimRef.Add(a.AttackData, a);
            Attacks.Add(a.AttackData);
        }
    }

    [SerializeField]
    protected List<AttackDataAnimOverrideWrapper> AttackAnims = new List<AttackDataAnimOverrideWrapper>();
   // public AttackDataAnimOverrideWrapper test;
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
