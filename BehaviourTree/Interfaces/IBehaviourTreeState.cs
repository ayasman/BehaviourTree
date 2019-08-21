using System;
using System.Collections.Generic;
using System.Text;

namespace AYLib.BehaviourTree
{
    public interface IBehaviourTreeState
    {
        string NodeName { get; }

        BehaviourReturnCode CurrentState { get; }

        List<IBehaviourTreeState> Children { get; }
    }
}
