using System;
using System.Collections.Generic;
using System.Text;

namespace C4i.BehaviourTree
{
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
