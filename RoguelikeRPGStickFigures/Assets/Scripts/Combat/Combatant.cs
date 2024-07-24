using UnityEngine;
using System.Collections.Generic;

//base class for any combat able creature
[System.Serializable]
public class Combatant : IDamageable
{
    public Combatant(Stat con, Stat str, Stat dex, Stat arc, Stat cha, int team = 1)
    {
        Constitution = con;
        Strength = str;
        Dexterity = dex;
        Arcana = arc;
        Charisma = cha;
        MaxHealth = new Stat(Constitution.Value * 10);
        Health = MaxHealth.Value;
        Team = team;
    }
    //TEMP
    public void SetName(string s)
    {
        Name = s;
    }

    public string Name;
    public int Level;
    //Team 0 reserved for player team
    public int Team;
    public int CurrentInitiative = 0;
    public int InitiativeBonus = 0;
    public bool AIControlled = true;
    public Stat Constitution;
    public Stat Strength;
    public Stat Dexterity;
    public Stat Arcana;
    public Stat Charisma;
    public int Health { get; protected set; }
    [HideInInspector] public Stat MaxHealth; 

    public virtual void StartTurnDecisionTree()
    {
        List<Stat> stats = new List<Stat> { Strength, Dexterity, Arcana, Charisma };
        stats.Sort((a, b) => { return b.Value.CompareTo(a.Value); });
        if (stats[0] ==Strength)
        {

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

    
}
