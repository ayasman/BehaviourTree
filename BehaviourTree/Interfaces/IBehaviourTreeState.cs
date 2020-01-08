using System;
using System.Collections.Generic;
using System.Text;

namespace AYLib.BehaviourTree
{
    /// <summary>
    /// Contains the data for the current state of a node in the tree. Used for debugging and general information output.
    /// </summary>
    public interface IBehaviourTreeState
    {
        string NodeName { get; }

        BehaviourReturnCode CurrentState { get; }

        List<IBehaviourTreeState> Children { get; }
    }
}
