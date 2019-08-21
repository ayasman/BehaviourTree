using System;

namespace AYLib.BehaviourTree
{
    /// <summary>
    /// Returns the inverse of the result of the child action when visited.
    /// 
    /// Sucess -> Failure
    /// Failure -> Sucess
    /// Error -> Error
    /// Running -> Running
    /// </summary>
    public class InverterNode : BehaviourNode
    {
        private readonly ActionNode childNode;

        public InverterNode(string nodeName, ActionNode childNode)
            : base(nodeName)
        {
            this.childNode = childNode;
        }

        public override BehaviourReturnCode Visit(long elapsedTime, object dataContext)
        {
            var status = childNode.Visit(elapsedTime, dataContext);
            switch (status)
            {
                case BehaviourReturnCode.Success:
                    return Failed();
                case BehaviourReturnCode.Failure:
                    return Succeed();
                default:
                    return Return(status);
            }
        }

        public override IBehaviourTreeState GetState()
        {
            var state = base.GetState();
            state.Children.Add(childNode.GetState());
            return state;
        }

        public override void ResetState()
        {
            base.ResetState();
            childNode?.ResetState();
        }
    }
}
