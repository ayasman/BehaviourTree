using System;

namespace AYLib.BehaviourTree
{
    internal class SucceedNode<TTime, TContext> : BehaviourNode<TTime, TContext>
    {
        private readonly ActionNode<TTime, TContext> childNode;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="nodeName">User friendly name of the node</param>
        /// <param name="childNode">The action node that is visited</param>
        public SucceedNode(string nodeName, ActionNode<TTime, TContext> childNode)
            : base(nodeName)
        {
            this.childNode = childNode;
        }

        /// <summary>
        /// Calls the child action node.
        /// </summary>
        /// <param name="elapsedTime">The time since last visit</param>
        /// <param name="dataContext">The data context to run against</param>
        /// <returns>Completion state of the node</returns>
        public override BehaviourReturnCode Visit(TTime elapsedTime, TContext dataContext)
        {
            var status = childNode.Visit(elapsedTime, dataContext);
            return status == BehaviourReturnCode.Failure ? Succeed() : Return(status);
        }

        /// <summary>
        /// Gets the state data of the node.
        /// </summary>
        /// <returns>Node state data</returns>
        public override IBehaviourTreeState GetState()
        {
            var state = base.GetState();
            state.Children.Add(childNode.GetState());
            return state;
        }

        /// <summary>
        /// Resets the node state to ready.
        /// </summary>
        public override void ResetState()
        {
            base.ResetState();
            childNode?.ResetState();
        }
    }
}
