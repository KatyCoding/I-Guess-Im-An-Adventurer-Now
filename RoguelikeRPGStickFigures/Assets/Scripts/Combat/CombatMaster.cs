using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;

public class CombatMaster : MonoBehaviour
{
    public static CombatMaster instance;
    private int turnIndex = 0;
    private List<Combatant> CombatOrder = new List<Combatant>();
    public Action<CombatantData> OnPlayerTurnStart;
    #region Unity Methods
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        instance = this;
    }
    #endregion

    public void Start()
    {
        List<Combatant> participants = new List<Combatant>();
        var combatants = FindObjectsByType<CombatantBehavior>(FindObjectsSortMode.None).ToList();
        foreach (var c in combatants)
        {
            participants.Add(c.combatant);
        }
        EnterCombat(participants);
    }
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
    public void EndTurn()
    {
        turnIndex++;
        StartCombatTurn();
    }
    private void StartCombatTurn()
    {
        if (turnIndex > CombatOrder.Count - 1)
            turnIndex = 0;
        var currentCombatant = CombatOrder[turnIndex];
        currentCombatant.StartTurn();
    }
    
    public static void PlayerTurnStart(CombatantData data )
    {
        instance.OnPlayerTurnStart.Invoke(data);
    }
}
