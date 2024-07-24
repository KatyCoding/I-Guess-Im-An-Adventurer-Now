using UnityEngine;
using System.Collections.Generic;
[System.Serializable]
public class Stat
{
    
    public Stat(int BaseValue)
    {
        baseValue = BaseValue;
        statMods = new List<StatModifier>();
    }

    [Tooltip("Will override any values set in the editor")][SerializeField] private StatScriptable ScriptableObjectOveride;

    public int Value { get { return RecalculateValue(); } }
    public int CurrentMaxValue { get { return GetMaxValue(); } }
    public int CurrentMinValue { get { return GetMinValue(); } }
    public int baseValue =1;
    public List<StatModifier> statMods = new List<StatModifier>();

    public void AddModifier(StatModifier mod)
    {
        statMods.Add(mod);
    }
    public void RemoveModifier(StatModifier mod)
    {
        if (statMods.Contains(mod))
            statMods.Remove(mod);

    }
    protected int RecalculateValue()
    {
        var val = baseValue;
        foreach(var mod in statMods)
        {
            val += mod.ModValue;
        }
        return val;
    }
    protected int GetMaxValue()
    {
        var maxVal = baseValue;
        foreach(var mod in statMods)
        {
            maxVal += mod.MaxValue;
        }
        return maxVal;
    }
    protected int GetMinValue()
    {
        return baseValue + statMods.Count;
        
    }
}

public class StatScriptable : ScriptableObject
{
    public Stat stat;
}