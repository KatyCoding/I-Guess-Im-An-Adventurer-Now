using System.Linq.Expressions;
using UnityEngine;

[CreateAssetMenu(fileName = "TESTRECURSION", menuName = "Scriptable Objects/TESTRECURSION")]
public class TESTRECURSION : ScriptableObject
{
    public int TestInteger;
    public TestData testData = new TestData();
}
[System.Serializable]
public class TestData
{
    public TestData()
    {
        RecurringInteger = 3;
       
    }
    public int RecurringInteger;
    public TestData fuckitup;
}