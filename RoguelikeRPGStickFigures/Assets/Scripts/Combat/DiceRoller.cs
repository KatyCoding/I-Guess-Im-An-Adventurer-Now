using UnityEngine;
using System.Collections.Generic;
public class DiceRoller : MonoBehaviour
{
    public static DiceRoller Instance;
    public enum Dice
    {
        D4 = 4,
        D6 = 6,
        D8 = 8,
        D10 = 10,
        D12 = 12,
        D20 = 20,

    }
    private void Start()
    {
        Instance = this;
    }
    public int RollDie(Dice die)
    {
        switch (die)
        {
            case Dice.D4: return RollD4();
            case Dice.D6: return RollD6();
            case Dice.D8: return RollD8();
            case Dice.D10: return RollD10();
            case Dice.D12: return RollD12();
            case Dice.D20: return RollD20();
            default: return 0;
        }
    }

    public int RollD20()
    {
        return Random.Range(1, 21);
    }
    public int RollD20(int numberOfDice = 1)
    {

        var total = 0;
        for (int i = 0; i < numberOfDice; i++)
        {
            total += RollD20();
        }
        return total;
    }
    /// <param name="numberOfDice">number of dice to roll</param>
    /// <param name="rolls">list of individual rolls</param>
    /// <returns>total number rolled</returns>
    public int RollD20(out List<int> rolls, int numberOfDice = 1)
    {
        var total = 0;
        rolls = new List<int>();
        for (int i = 0; i < numberOfDice; i++)
        {
            var roll = RollD20();
            total += roll;
            rolls.Add(roll);
        }
        return total;
    }

    public int RollD4()
    {
        return Random.Range(1, 5);
    }
    public int RollD4(int numberOfDice = 1)
    {
        var total = 0;
        for (int i = 0; i < numberOfDice; i++)
        {
            total += RollD4();
        }
        return total;
    }
    public int RollD6()
    {
        return Random.Range(1, 7);
    }
    public int RollD8()
    {
        return Random.Range(1, 9);
    }
    public int RollD10()
    {
        return Random.Range(1, 11);
    }
    public int RollD12()
    {
        return Random.Range(1, 13);
    }


}
