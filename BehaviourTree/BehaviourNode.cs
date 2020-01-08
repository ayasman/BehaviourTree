using System;

namespace AYLib.BehaviourTree
{
    /// <summary>
    /// Base class for all of the nodes in a behaviour tree.
    /// </summary>
    public abstract class BehaviourNode : IBehaviourTreeNode
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="nodeName">User friendly name of the node</param>
        public BehaviourNode(string nodeName)
        {
            Name = nodeName;
        }

        /// <summary>
        /// The current running state of the node.
        /// </summary>
        public BehaviourReturnCode CurrentState { get; internal set; } = BehaviourReturnCode.Ready;

        /// <summary>
        /// User friendly name of the node.
        /// </summary>
        public string Name { get; internal set; } = string.Empty;

        /// <summary>
        /// Visits the node.
        /// </summary>
        /// <param name="elapsedTime">The time since last visit</param>
        /// <param name="dataContext">The data context to run against</param>
        /// <returns>Completion state of the node</returns>
        public abstract BehaviourReturnCode Visit(long elapsedTime, object dataContext);

        /// <summary>
        /// Gets the state data of the node.
        /// </summary>
        /// <returns>Node state data</returns>
        public virtual IBehaviourTreeState GetState()
        {
            return new BehaviourTreeNodeState(Name, CurrentState);
        }

        /// <summary>
        /// Resets the node state to ready.
        /// </summary>
        public virtual void ResetState()
        {
            CurrentState = BehaviourReturnCode.Ready;
        }

        /// <summary>
        /// Sets the node state to Success, returns same.
        /// </summary>
        /// <returns>Success</returns>
        protected BehaviourReturnCode Succeed()
        {
            CurrentState = BehaviourReturnCode.Success;
            return CurrentState;
        }

        /// <summary>
        /// Sets the node state to Failure, returns same.
        /// </summary>
        /// <returns>Failure</returns>
        protected BehaviourReturnCode Failed()
        {
            CurrentState = BehaviourReturnCode.Failure;
            return CurrentState;
        }

        /// <summary>
        /// Sets the node state to Running, returns same.
        /// </summary>
        /// <returns>Running</returns>
        protected BehaviourReturnCode Running()
        {
            CurrentState = BehaviourReturnCode.Running;
            return CurrentState;
        }

        /// <summary>
        /// Sets the node state to Error, returns same.
        /// </summary>
        /// <returns>Error</returns>
        protected BehaviourReturnCode Error()
        {
            CurrentState = BehaviourReturnCode.Error;
            return CurrentState;
        }

        /// <summary>
        /// Sets and returns the state of the node, based on the status given.
        /// </summary>
        /// <param name="status">State to set node to</param>
        /// <returns>Node state</returns>
        protected BehaviourReturnCode Return(BehaviourReturnCode status)
        {
            switch (status)
            {
                case BehaviourReturnCode.Success:
                    return Succeed();
                case BehaviourReturnCode.Failure:
                    return Failed();
                case BehaviourReturnCode.Running:
                    return Running();
                default:
                    return Error();
            }
        }
    }
}
