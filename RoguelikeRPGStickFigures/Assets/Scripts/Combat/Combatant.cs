using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using JetBrains.Annotations;

//base class for any combat able creature
[System.Serializable]
public class Combatant : IDamageable
{
    public Combatant(Stat con, Stat str, Stat dex, Stat arc, Stat cha,int initBonus = 0, int team = 1,string name = "")
    {
        Constitution = con;
        Strength = str;
        Dexterity = dex;
        Arcana = arc;
        Charisma = cha;
        MaxHealth = new Stat(Constitution.Value * 10);
        Health = MaxHealth.Value;
        Team = team;
        InitiativeBonus = initBonus;
        Name = name;
    }
    public Combatant(Combatant c)
    {
        
    }
    //TEMP
    public void SetName(string s)
    {
        Name = s;
    }
    public Transform EntityTransformRef;
    public string Name;
    public int Level;
    //Team 0 reserved for player team
    public int Team;
    public int CurrentInitiative = 0;
    public int InitiativeBonus = 0;
    public bool AIControlled = true;
    public CombatantData CombatData;

    [HideInInspector] public Stat Constitution;
    [HideInInspector] public Stat Strength;
    [HideInInspector] public Stat Dexterity;
    [HideInInspector] public Stat Arcana;
    [HideInInspector] public Stat Charisma;
    public int Health { get; protected set; }
    [HideInInspector] public Stat MaxHealth;
    public virtual void StartTurn()
    {
        if (AIControlled)
            StartTurnDecisionTree();
        else
        {
            CombatMaster.PlayerTurnStart(CombatData);
            
        }
    }
    
    public virtual void StartTurnDecisionTree()
    {
        List<Stat> stats = new List<Stat> { Strength, Dexterity, Arcana, Charisma };
        stats.Sort((a, b) => { return b.Value.CompareTo(a.Value); });
        if (stats[0] == Strength)
        {
            var attack = CombatData.GetAttackRandom(AttackDataScriptableObject.AttackModifierType.strength);
            CombatMaster.instance.StartCoroutine(DoAttackSequence(attack));
        }
        else if (stats[0] == Dexterity)
        {
        }
        else if (stats[0] == Arcana)
        {

        }
        else//Charisma
        {

        }
    }
    public virtual void TakeDamage(Damage dam)
    {
        Health -= dam.Value;
        if (Health <= 0)
            Debug.Log(Name + " is dead.");
    }

    protected IEnumerator DoAttackSequence(CombatantData.AttackDataAnimOverrideWrapper Attack)
    {
        var positionCache = new Vector3(EntityTransformRef.position.x,EntityTransformRef.position.y,EntityTransformRef.position.z);
        Combatant target = ChooseTarget();
        //TODO: set position for ranged attack (walk forward a few steps and shoot projectile)

        //walk up to do melee attack
       
        var travelDist = (target.EntityTransformRef.position - EntityTransformRef.position);
        travelDist = travelDist - travelDist.normalized;
        var targetPos = EntityTransformRef.position + travelDist;
        //TODO: make MoveTo method (in script that controls movement) with callback instead
        var distLeft = travelDist;
        while (distLeft.magnitude != 0)
        {
            EntityTransformRef.position += travelDist.normalized * Time.deltaTime;
            distLeft -= travelDist.normalized * Time.deltaTime;
            if (distLeft.magnitude < .1f)
            {
                EntityTransformRef.position = targetPos;
                distLeft = Vector3.zero;
            }
                yield return null;
        }
        yield return new WaitForSeconds(.5f);
        //Do Attack
        yield return new WaitForSeconds(.5f);
        while (EntityTransformRef.position != positionCache)
        {
            EntityTransformRef.position -= travelDist.normalized * Time.deltaTime;
            if ((EntityTransformRef.position - positionCache).magnitude < .1f)
                EntityTransformRef.position = positionCache;
            yield return null;
        }
        yield return null;
    }
    protected virtual Combatant ChooseTarget()
    {
        Combatant target = null;
        var targets = CombatMaster.GetOpposingTeam(Team);
        foreach(var c in targets)
        {
            if (c.Health <= 0)
                continue;
            if (target==null)
                target = c;
            if (target.Health > c.Health)
                target = c;
        }
        return target;
    }
}
