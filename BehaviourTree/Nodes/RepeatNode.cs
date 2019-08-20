using System;

namespace C4i.BehaviourTree
{
    public class RepeatNode : BehaviourNode
    {
        private readonly ActionNode childNode;
        private readonly uint repeatCount;
        private uint currentCount = 0;

        public RepeatNode(string nodeName, ActionNode childAction, uint repeatCount)
            : base(nodeName)
        {
            this.childNode = childAction;
            this.repeatCount = currentCount = repeatCount;
        }

        public override BehaviourReturnCode Visit(long elapsedTime, object dataContext)
        {
            if (currentCount > 0)
            {
                var status = childNode.Visit(elapsedTime, dataContext);

                if (status == BehaviourReturnCode.Failure)
                {
                    currentCount = repeatCount;
                    return Failed();
                }
                if (status == BehaviourReturnCode.Success)
                {
                    currentCount--;
                    if (currentCount == 0)
                    {
                        currentCount = repeatCount;
                        return Succeed();
                    }
                }
                return Running();
            }
            return Error();
        }

        public override void ResetState()
        {
            base.ResetState();
            childNode?.ResetState();
        }
    }
}
