using UnityEngine;

public class CombatantBehavior : MonoBehaviour
{
    public Combatant combatant;
    private void Awake()
    {
        combatant = new Combatant(combatant.Constitution,combatant.Strength, combatant.Dexterity, combatant.Arcana,
            combatant.Charisma,combatant.InitiativeBonus,combatant.Team);
    }
}

