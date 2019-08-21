using System;

namespace AYLib.BehaviourTree
{
    /// <summary>
    /// Base class for all of the nodes in a behaviour tree.
    /// </summary>
    public abstract class BehaviourNode : IBehaviourTreeNode
    {
        public BehaviourNode(string nodeName)
        {
            Name = nodeName;
        }

        public BehaviourReturnCode CurrentState { get; internal set; } = BehaviourReturnCode.Ready;

        public string Name { get; internal set; } = string.Empty;

        public abstract BehaviourReturnCode Visit(long elapsedTime, object dataContext);

        public virtual IBehaviourTreeState GetState()
        {
            return new BehaviourTreeNodeState(Name, CurrentState);
        }

        public virtual void ResetState()
        {
            CurrentState = BehaviourReturnCode.Ready;
        }

        protected BehaviourReturnCode Succeed()
        {
            CurrentState = BehaviourReturnCode.Success;
            return CurrentState;
        }

        protected BehaviourReturnCode Failed()
        {
            CurrentState = BehaviourReturnCode.Failure;
            return CurrentState;
        }

        protected BehaviourReturnCode Running()
        {
            CurrentState = BehaviourReturnCode.Running;
            return CurrentState;
        }

        protected BehaviourReturnCode Error()
        {
            CurrentState = BehaviourReturnCode.Error;
            return CurrentState;
        }

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
