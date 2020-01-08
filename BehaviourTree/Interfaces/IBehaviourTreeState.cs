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
        /// <summary>
        /// User friendly name of the node.
        /// </summary>
        string NodeName { get; }

        /// <summary>
        /// Current running state of the node.
        /// </summary>
        BehaviourReturnCode CurrentState { get; }

        /// <summary>
        /// List of child nodes.
        /// </summary>
        List<IBehaviourTreeState> Children { get; }
    }
}
