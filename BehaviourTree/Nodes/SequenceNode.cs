using System;
using System.Collections.Generic;
using System.Linq;

namespace AYLib.BehaviourTree
{
    /// <summary>
    /// Visits all of the child nodes in order until one fails or they all succeed.
    /// </summary>
    internal class SequenceNode<TTime, TContext> : BehaviourNode<TTime, TContext>, IBehaviourTreeParentNode<TTime, TContext>
    {
        private readonly List<BehaviourNode<TTime, TContext>> childNodes = null;
        private BehaviourNode<TTime, TContext> runningNode = null;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="nodeName">User friendly name of the node</param>
        public SequenceNode(string nodeName) 
            : base(nodeName)
        {
            childNodes = new List<BehaviourNode<TTime, TContext>>();
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="nodeName">User friendly name of the node</param>
        /// <param name="childNodes">The child nodes to initialize with</param>
        public SequenceNode(string nodeName, List<BehaviourNode<TTime, TContext>> childNodes)
            : base(nodeName)
        {
            this.childNodes = childNodes;
        }

        /// <summary>
        /// Visits child nodes in order.
        /// </summary>
        /// <param name="elapsedTime">The time since last visit</param>
        /// <param name="dataContext">The data context to run against</param>
        /// <returns>Completion state of the node</returns>
        public override BehaviourReturnCode Visit(TTime elapsedTime, TContext dataContext)
        {
            var processingNodes = runningNode == null
                                    ? childNodes
                                    : childNodes.SkipWhile(node => node != runningNode);

            foreach (var currentNode in processingNodes)
            {
                var status = currentNode.Visit(elapsedTime, dataContext);
                if (status == BehaviourReturnCode.Running)
                    runningNode = currentNode;
                else
                    runningNode = null;

                if (status != BehaviourReturnCode.Success)
                    return Return(status);
            }

            return Succeed();
        }

        /// <summary>
        /// Adds child node.
        /// </summary>
        /// <param name="childNode">The new child node</param>
        public void AddChild(BehaviourNode<TTime, TContext> childNode)
        {
            childNodes.Add(childNode);
        }

        /// <summary>
        /// Gets the state data of the node.
        /// </summary>
        /// <returns>Node state data</returns>
        public override IBehaviourTreeState GetState()
        {
            var state = base.GetState();
            foreach (var node in childNodes)
                state.Children.Add(node.GetState());
            return state;
        }

        /// <summary>
        /// Resets the node state to ready.
        /// </summary>
        public override void ResetState()
        {
            base.ResetState();
            foreach (var node in childNodes)
                node.ResetState();
        }
    }
}
