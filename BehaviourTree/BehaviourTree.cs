using System;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("BehaviourTreeTests")]

namespace AYLib.BehaviourTree
{
    /// <summary>
    /// Top level of a behaviour tree, and contains a single child node, which can be of any type.
    /// While not totally necessary to use this as the top level, the Visit method will be used
    /// to reset the state in the tree after a completed run.
    /// </summary>
    internal class BehaviourTree : BehaviourNode, IBehaviourTreeParentNode
    {
        private BehaviourNode childNode;

        internal BehaviourNode ChildNode => childNode;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="nodeName">User friendly name of the node</param>
        public BehaviourTree(string nodeName)
            : base(nodeName)
        {

        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="nodeName">User friendly name of the node</param>
        /// <param name="childNode">The initial node to add to the root of the tree</param>
        public BehaviourTree(string nodeName, BehaviourNode childNode)
            : base(nodeName)
        {
            AddChild(childNode);
        }

        /// <summary>
        /// Sets the root node of the tree.
        /// </summary>
        /// <param name="childNode">The new root node</param>
        public void AddChild(BehaviourNode childNode)
        {
            this.childNode = childNode;
        }

        /// <summary>
        /// Visits the root of the tree. Will reset the state if it isn't running at all, and then visit the
        /// child nodes.
        /// </summary>
        /// <param name="elapsedTime">The time since last visit</param>
        /// <param name="dataContext">The data context to run against</param>
        /// <returns>The state of the tree (running if any running, failed, or success</returns>
        public override BehaviourReturnCode Visit(long elapsedTime, object dataContext)
        {
            if (childNode == null)
                throw new ApplicationException("Root node cannot have a null child.");

            if (CurrentState != BehaviourReturnCode.Running)
                ResetState();

            return Return(childNode.Visit(elapsedTime, dataContext));
        }

        /// <summary>
        /// Gets the state of all nodes in the tree.
        /// </summary>
        /// <returns>Object containing state of all tree nodes</returns>
        public override IBehaviourTreeState GetState()
        {
            var state = base.GetState();
            state.Children.Add(childNode?.GetState());
            return state;
        }

        /// <summary>
        /// Resets the state of all nodes in the tree to Ready.
        /// </summary>
        public override void ResetState()
        {
            base.ResetState();
            childNode?.ResetState();
        }
    }
}
