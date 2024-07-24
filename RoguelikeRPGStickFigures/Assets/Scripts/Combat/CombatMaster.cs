using UnityEngine;
using System.Collections.Generic;

public class CombatMaster : MonoBehaviour
{

    private int turnIndex = 0;
    private List<Combatant> CombatOrder = new List<Combatant>();
    #region Unity Methods
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        List<Combatant> cs = new List<Combatant>();
        for(int i = 0;i<6;i++)
        {
            cs.Add(new Combatant(new Stat(Random.Range(1,3)), new Stat(Random.Range(1, 3)),
                new Stat(Random.Range(1, 3)), new Stat(Random.Range(1, 3)),new Stat(Random.Range(1, 3))));
        }

        EnterCombat(cs);
    }
    #endregion


    public void EnterCombat(List<Combatant> combatants)
    {
        foreach(var c in combatants)
        {
            var initiativeRoll = DiceRoller.Instance.RollD20();
            c.CurrentInitiative = initiativeRoll + c.InitiativeBonus;
        }
        combatants.Sort((a,b) => { return b.CurrentInitiative.CompareTo(a.CurrentInitiative);});
        CombatOrder.AddRange(combatants);
        StartCombatTurn();
    }
    
    private void StartCombatTurn()
    {
        if (turnIndex > CombatOrder.Count - 1)
            turnIndex = 0;
        var currentCombatant = CombatOrder[turnIndex];
        if (currentCombatant.AIControlled)
            currentCombatant.StartTurnDecisionTree();
        turnIndex++;
    }


}
