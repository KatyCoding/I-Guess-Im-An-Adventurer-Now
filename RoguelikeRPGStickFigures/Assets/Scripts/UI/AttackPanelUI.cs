using UnityEngine;
using UnityEngine.UI;

public class AttackPanelUI : MonoBehaviour
{
    public Button ActionButtonRef;
    private void Start()
    {
        CombatMaster.instance.OnPlayerTurnStart += Populate;
    }
    public void Populate(Combatant currentCombatant)
    {
        foreach (var attack in currentCombatant.CombatData.Attacks)
        {
            var button = Instantiate<Button>(ActionButtonRef, this.transform);
            button.gameObject.SetActive(true);
            var tmp = button.GetComponentInChildren<TMPro.TextMeshProUGUI>();
            tmp.text = attack.name;
            button.onClick.AddListener(()=> { currentCombatant.SelectAttack(attack); }) ;
            button.gameObject.SetActive(true);
        }
    }
}
