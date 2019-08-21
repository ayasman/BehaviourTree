using System;
using System.Collections.Generic;
using System.Text;

namespace AYLib.BehaviourTree
{
    /// <summary>
    /// The current state of a node and its child nodes.
    /// </summary>
    public class BehaviourTreeNodeState : IBehaviourTreeState
    {
        public string NodeName { get; internal set; }
        public BehaviourReturnCode CurrentState { get; internal set; }
        public List<IBehaviourTreeState> Children { get; internal set; } = new List<IBehaviourTreeState>();

        public BehaviourTreeNodeState(string nodeName, BehaviourReturnCode state)
        {
            NodeName = nodeName;
            CurrentState = state;
        }
    }
}
