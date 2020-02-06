using System;
using System.Collections.Generic;

namespace AYLib.BehaviourTree
{
    /// <summary>
    /// Visits one of the child nodes at random, and continues visiting that child until it finishes running.
    /// </summary>
    internal class RandomNode : BehaviourNode, IBehaviourTreeParentNode
    {
        private readonly List<BehaviourNode> childNodes = null;
        private BehaviourNode runningNode = null;
        private Random random = new Random();

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="nodeName">User friendly name of the node</param>
        public RandomNode(string nodeName)
            : base(nodeName)
        {
            childNodes = new List<BehaviourNode>();
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="nodeName">User friendly name of the node</param>
        /// <param name="childNodes">The child nodes to initialize with</param>
        public RandomNode(string nodeName, List<BehaviourNode> childNodes)
            : base(nodeName)
        {
            this.childNodes = childNodes;
        }

        /// <summary>
        /// Randomly visits one node in the list.
        /// </summary>
        /// <param name="elapsedTime">The time since last visit</param>
        /// <param name="dataContext">The data context to run against</param>
        /// <returns>Completion state of the node</returns>
        public override BehaviourReturnCode Visit(long elapsedTime, object dataContext)
        {
            if (childNodes.Count == 0)
                return Error();

            var processingNode = runningNode ?? childNodes[random.Next(childNodes.Count)];

            var status = processingNode.Visit(elapsedTime, dataContext);
            if (status == BehaviourReturnCode.Running)
                runningNode = processingNode;
            else
                runningNode = null;
            return Return(status);
        }

        /// <summary>
        /// Adds child node.
        /// </summary>
        /// <param name="childNode">The new child node</param>
        public void AddChild(BehaviourNode childNode)
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
