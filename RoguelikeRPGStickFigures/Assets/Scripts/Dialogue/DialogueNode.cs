using UnityEngine;
using System.Collections.Generic;
using System;
[Serializable]
[CreateAssetMenu(fileName = "DialogueNode", menuName = "Dialogue/Dialogue Node")]
public class DialogueNode : ScriptableObject
{
    public string Text;
    public List<DialogueNode> BackLinks = new List<DialogueNode>();
    public List<Transition> Transitions = new List<Transition>();
    private void OnValidate()
    {
        foreach(var t in Transitions)
        {
            t.Next.BackLinks.Add(this);
        }

    }
}
[Serializable]
public class Transition
{
    public DialogueNode Next;
    public List<Func<bool>> Conditions = new List<Func<bool>>();
}
[CreateAssetMenu(fileName = "DialogueCondition",menuName ="Dialogue/Condition")]
public class DialogueCondition:ScriptableObject
{
    public UnityEngine.Object First;
    public UnityEngine.Object Second;
    
}

/*[CreateAssetMenu(fileName = "SpeakingNode", menuName = "Dialogue/Speaking Node")]
public class SpeakingNode : DialogueNode
{
    public string Text;
}
*/