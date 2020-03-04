using System;
using System.Collections.Generic;
using System.Linq;

namespace AYLib.BehaviourTree
{
    /// <summary>
    /// Visits all of the children during every call to the Visit method. Returns a failure result when the first one
    /// fails, a success result if they all succeed, or running if any are still working.
    /// </summary>
    internal class ParallelNode<TTime, TContext> : BehaviourNode<TTime, TContext>, IBehaviourTreeParentNode<TTime, TContext>
    {
        private readonly List<BehaviourNode<TTime, TContext>> childNodes = null;
        private readonly List<BehaviourNode<TTime, TContext>> runningNodes = new List<BehaviourNode<TTime, TContext>>();

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="nodeName">User friendly name of the node</param>
        public ParallelNode(string nodeName)
            : base(nodeName)
        {
            childNodes = new List<BehaviourNode<TTime, TContext>>();
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="nodeName">User friendly name of the node</param>
        /// <param name="childNodes">The child nodes to initialize with</param>
        public ParallelNode(string nodeName, List<BehaviourNode<TTime, TContext>> childNodes)
            : base(nodeName)
        {
            this.childNodes = childNodes;
        }

        /// <summary>
        /// Calls all child nodes each visit.
        /// </summary>
        /// <param name="elapsedTime">The time since last visit</param>
        /// <param name="dataContext">The data context to run against</param>
        /// <returns>Completion state of the node</returns>
        public override BehaviourReturnCode Visit(TTime elapsedTime, TContext dataContext)
        {
            bool anyFailed = false;

            var processingNodes = runningNodes.Count == 0
                                    ? childNodes
                                    : runningNodes.ToList();

            foreach (var node in processingNodes)
            {
                var status = node.Visit(elapsedTime, dataContext);

                if (status == BehaviourReturnCode.Running && !runningNodes.Contains(node))
                    runningNodes.Add(node);
                if (status == BehaviourReturnCode.Success && runningNodes.Contains(node))
                    runningNodes.Remove(node);
                if (status == BehaviourReturnCode.Failure)
                    anyFailed = true;
            }

            if (anyFailed)
            {
                runningNodes.Clear();
                return Failed();
            }
            if (runningNodes.Count > 0)
                return Running();
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
