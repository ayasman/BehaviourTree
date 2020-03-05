using System;
using System.Collections.Generic;
using System.Text;

namespace AYLib.BehaviourTree
{
    /// <summary>
    /// The current state of a node and its child nodes. Used mainly for debugging and diagnotics.
    /// </summary>
    public class BehaviourTreeNodeState : IBehaviourTreeState
    {
        /// <summary>
        /// User friendly name of the node.
        /// </summary>
        public string NodeName { get; internal set; }

        /// <summary>
        /// Current running state of the node.
        /// </summary>
        public BehaviourReturnCode CurrentState { get; internal set; }

        /// <summary>
        /// List of child nodes.
        /// </summary>
        public List<IBehaviourTreeState> Children { get; internal set; } = new List<IBehaviourTreeState>();

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="nodeName">User friendly name of the node.</param>
        /// <param name="state">Current running state of the node.</param>
        public BehaviourTreeNodeState(string nodeName, BehaviourReturnCode state)
        {
            NodeName = nodeName;
            CurrentState = state;
        }

        /// <summary>
        /// Checks if one state is the same as the other (same tree and results).
        /// </summary>
        /// <param name="otherState">The other state object to check.</param>
        /// <returns>true if they are the same, otherwise false.</returns>
        public bool StateEqual(IBehaviourTreeState otherState)
        {
            if (this.NodeName != otherState.NodeName ||
                this.CurrentState != otherState.CurrentState ||
                this.Children.Count != otherState.Children.Count)
                return false;

            for(int i=0; i<this.Children.Count; i++)
            {
                if (!this.Children[i].StateEqual(otherState.Children[i]))
                    return false;
            }
            return true;
        }
    }
}
