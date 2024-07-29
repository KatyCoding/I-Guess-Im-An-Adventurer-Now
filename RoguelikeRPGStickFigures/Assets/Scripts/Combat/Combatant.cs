using UnityEngine;
using System.Collections.Generic;
using System.Collections;
//base class for any combat able creature
[System.Serializable]
public class Combatant : IDamageable
{
    public Combatant(Stat con, Stat str, Stat dex, Stat arc, Stat cha, int initBonus = 0, int team = 1, string name = "")
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
        Constitution = c.Constitution;
        Strength = c.Strength;
        Dexterity = c.Dexterity;
        Arcana = c.Arcana;
        Charisma = c.Charisma;
        MaxHealth = new Stat(Constitution.Value * 10);
        Health = MaxHealth.Value;
        Team = c.Team;
        InitiativeBonus = c.InitiativeBonus;
        Name = c.Name;
        EntityTransformRef = c.EntityTransformRef;
        Level = c.Level;
        AIControlled = c.AIControlled;
        CombatData = c.CombatData;
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

    public Stat Constitution;
    public Stat Strength;
    public Stat Dexterity;
    public Stat Arcana;
    public Stat Charisma;
    public int Health { get; protected set; }
    [HideInInspector] public Stat MaxHealth;
    public System.Action<AttackDataScriptableObject> OnAttackTriggered;
    public virtual void StartTurn()
    {
        if (AIControlled)
            StartTurnDecisionTree();
        else
        {
            CombatMaster.PlayerTurnStart(this);

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
        var val = dam.Value;
        GlobalEvents.OnDamageTaken?.Invoke(val, EntityTransformRef.position);
        Health -= val;
        if (Health <= 0)
            Debug.Log(Name + " is dead.");
    }
    public void SelectAttack(AttackDataScriptableObject attackData)
    {
        //TODO: confirmation and display info about attack
        //TODO: select target
        CombatMaster.instance.StartCoroutine(DoAttackSequence(attackData));
    }
    protected IEnumerator DoAttackSequence(AttackDataScriptableObject Attack)
    {
        var positionCache = new Vector3(EntityTransformRef.position.x, EntityTransformRef.position.y, EntityTransformRef.position.z);
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
            var travel = travelDist.normalized * Time.deltaTime * 3;
            EntityTransformRef.position += travel;
            distLeft -= travel;
            if (distLeft.magnitude < .1f)
            {
                EntityTransformRef.position = targetPos;
                distLeft = Vector3.zero;
            }
            yield return null;
        }
        yield return new WaitForSeconds(.5f);
        OnAttackTriggered?.Invoke(Attack);
        CombatMaster.instance.StartCoroutine(WaitForDamageTrigger(target, Attack.Damage));
        yield return new WaitForSeconds(2);


        while (EntityTransformRef.position != positionCache)
        {
            EntityTransformRef.position -= travelDist.normalized * Time.deltaTime *3;
            if ((EntityTransformRef.position - positionCache).magnitude < .1f)
                EntityTransformRef.position = positionCache;
            yield return null;
        }
        yield return null;
        EndTurn();
    }
    protected virtual void EndTurn()
    {
        CombatMaster.instance.EndTurn();
    }
    protected virtual Combatant ChooseTarget()
    {
        Combatant target = null;
        var targets = CombatMaster.GetOpposingTeam(Team);
        foreach (var c in targets)
        {
            if (c.Health <= 0)
                continue;
            if (target == null)
                target = c;
            if (target.Health > c.Health)
                target = c;
        }
        return target;
    }
    protected bool DamageTrigger = false;
    protected virtual IEnumerator WaitForDamageTrigger(Combatant target, Damage dam)
    {
        yield return new WaitUntil(() => { return DamageTrigger; });
        target.TakeDamage(dam);
        DamageTrigger = false;

    }
    public virtual void TriggerDamage()
    {
        DamageTrigger = true;
    }


}
