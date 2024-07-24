using UnityEngine;

public class Slime : Combatant
{
    public Slime(Stat con, Stat str, Stat dex, Stat arc, Stat cha, int team = 1) : base(con, str, dex, arc, cha, team)
    {

    }

    public override void StartTurnDecisionTree()
    {
        
    }
    public override void TakeDamage(Damage dam)
    {
        //TODO Handle Resistances
        Health -= dam.Value;
        if (Health <= 0)
            Debug.Log(Name + " is dead");
    }
}
