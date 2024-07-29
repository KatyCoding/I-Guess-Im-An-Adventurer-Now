using UnityEngine;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System;
[Serializable]
public abstract class DialogueNode
{
    public List<DialogueNode> BackLinks = new List<DialogueNode>();
    public List<DialogueNode> ForwardLinks = new List<DialogueNode>();
}
[Serializable]
public class BranchNode:DialogueNode
{

    [Serializable]
    public class Branch
    {
        public DialogueNode From;
        public DialogueNode To;
        public List<System.Func<bool>> Conditions = new List<System.Func<bool>>();
    }
    public List<Branch> Branches = new List<Branch>();
}
public class SpeakingNode:DialogueNode
{
    public string Text;
}