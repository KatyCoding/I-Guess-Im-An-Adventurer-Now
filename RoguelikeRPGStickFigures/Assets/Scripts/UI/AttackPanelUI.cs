using UnityEngine;
using UnityEngine.UI;

public class AttackPanelUI : MonoBehaviour
{
    public Button ActionButtonRef;
    private void Start()
    {
        CombatMaster.instance.OnPlayerTurnStart += Populate;
    }
    public void Populate(CombatantData data)
    {
        foreach (var attack in data.Attacks)
        {
            var button = Instantiate<Button>(ActionButtonRef, this.transform);
            button.gameObject.SetActive(true);
            var tmp = button.GetComponentInChildren<TMPro.TextMeshProUGUI>();
            tmp.text = attack.data.name;
            button.gameObject.SetActive(true);
        }
    }
}
