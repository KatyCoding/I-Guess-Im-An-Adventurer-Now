using UnityEngine;

public class CombatantBehavior : MonoBehaviour
{
    public Combatant combatant;
    private void Awake()
    {
        combatant = new Combatant(combatant);
    }
    public void TriggerDamageOutput()
    {
        combatant.TriggerDamage();
    }

}

