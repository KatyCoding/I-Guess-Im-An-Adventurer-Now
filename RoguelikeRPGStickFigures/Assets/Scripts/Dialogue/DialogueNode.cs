using UnityEngine;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
public abstract class DialogueNode
{
    public List<DialogueNode> BackLinks = new List<DialogueNode>();
    public List<DialogueNode> ForwardLinks = new List<DialogueNode>();
}
public class BranchNode:DialogueNode
{
    public class Branch
    {
        public DialogueNode From;
        public DialogueNode To;
        public List<System.Func<bool>> Conditions = new List<System.Func<bool>>();
    }
}