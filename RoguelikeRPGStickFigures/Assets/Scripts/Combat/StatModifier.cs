using UnityEngine;
using System.Collections.Generic;
[CreateAssetMenu(fileName = "StatModifier", menuName = "Stats/StatModifier")]
public class StatModifier : ScriptableObject
{

    //-1 is permanent modifier. Or until manually removed. 
    public int TurnsLeft = -1;
    public int ModValue { get { return GetValueWithDice(); } }
    public int MaxValue { get { return GetMaxValue(); } }
    [SerializeField] private int baseValue = 0;
    public List<DiceRoller.Dice> Dice = new List<DiceRoller.Dice>();
    private int GetValueWithDice()
    {
        var val = baseValue;
        foreach(var die in Dice)
        {
            val += DiceRoller.Instance.RollDie(die);
        }
        return val;
    }
    private int GetMaxValue()
    {
        var maxVal = baseValue;
        foreach(var die in Dice)
        {
            maxVal += (int)die;
        }
        return maxVal;
    }
}
