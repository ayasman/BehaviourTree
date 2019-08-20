using System;
using System.Collections.Generic;
using System.Text;

namespace C4i.BehaviourTree
{
    public interface IBehaviourTreeState
    {
        string NodeName { get; }

        BehaviourReturnCode CurrentState { get; }

        List<IBehaviourTreeState> Children { get; }
    }
}
