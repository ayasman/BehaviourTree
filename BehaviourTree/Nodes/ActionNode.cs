using System;

namespace AYLib.BehaviourTree
{
    /// <summary>
    /// A leaf, or action, node within a tree. Contains no children, and does the action sent in when the Visit method is called.
    /// </summary>
    internal class ActionNode<TTime, TContext> : BehaviourNode<TTime, TContext>
    {
        private Func<TTime, TContext, BehaviourReturnCode> actionFunction = null;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="nodeName">User friendly name of the node</param>
        /// <param name="action">The action to take when visiting the node</param>
        public ActionNode(string nodeName, Func<TTime, TContext, BehaviourReturnCode> action)
            : base(nodeName)
        {
            actionFunction = action;
        }

        /// <summary>
        /// Calls the action for the node, returns the result.
        /// </summary>
        /// <param name="elapsedTime">The time since last visit</param>
        /// <param name="dataContext">The data context to run against</param>
        /// <returns>Completion state of the node</returns>
        public override BehaviourReturnCode Visit(TTime elapsedTime, TContext dataContext)
        {
            if (actionFunction != null)
            {
                return Return(actionFunction.Invoke(elapsedTime, dataContext));
            }
            return Error();
        }
    }
}
