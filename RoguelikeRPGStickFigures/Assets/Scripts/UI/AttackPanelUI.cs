using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
public class AttackPanelUI : MonoBehaviour
{
    public Button ActionButtonRef;
    private List<GameObject> Buttons = new List<GameObject>();
    private List<AttackDataScriptableObject> attacks = new List<AttackDataScriptableObject>();
    private void Start()
    {
        CombatMaster.instance.OnPlayerTurnStart += Populate;
    }
    public void Populate(Combatant currentCombatant)
    {
        
        foreach (var attack in currentCombatant.CombatData.Attacks)
        {
            if(attacks.Contains(attack))
                continue;
            attacks.Add(attack);
            var button = Instantiate<Button>(ActionButtonRef, this.transform);
            button.gameObject.SetActive(true);
            var tmp = button.GetComponentInChildren<TMPro.TextMeshProUGUI>();
            tmp.text = attack.name;
            button.onClick.AddListener(()=> { currentCombatant.SelectAttack(attack); }) ;
            button.gameObject.SetActive(true);
            Buttons.Add(button.gameObject);
            
        }
    }
}
