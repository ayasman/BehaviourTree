using System;

namespace C4i.BehaviourTree
{
    public class SucceedNode : BehaviourNode
    {
        private readonly ActionNode childNode;

        public SucceedNode(string nodeName, ActionNode childNode)
            : base(nodeName)
        {
            this.childNode = childNode;
        }

        public override BehaviourReturnCode Visit(long elapsedTime, object dataContext)
        {
            var status = childNode.Visit(elapsedTime, dataContext);
            return status == BehaviourReturnCode.Failure ? Succeed() : Return(status);
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
