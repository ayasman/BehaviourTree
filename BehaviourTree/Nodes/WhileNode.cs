using System;

namespace C4i.BehaviourTree
{
    public class WhileNode : BehaviourNode
    {
        private readonly BehaviourNode conditionChildNode;
        private readonly BehaviourNode actionChildNode;
        private BehaviourNode runningNode = null;
        private bool actionSuccess = false;

        public WhileNode(string nodeName, ActionNode conditionNode, ActionNode childNode)
            : base(nodeName)
        {
            conditionChildNode = conditionNode;
            actionChildNode = childNode;
        }

        public override BehaviourReturnCode Visit(long elapsedTime, object dataContext)
        {
            if (runningNode == null)
                actionSuccess = false;

            bool again = true;
            while(again)
            {
                BehaviourReturnCode conditionStatus = BehaviourReturnCode.Ready;
                if (runningNode == actionChildNode || BehaviourReturnCode.Success == (conditionStatus = conditionChildNode.Visit(elapsedTime, dataContext)))
                {
                    var actionStatus = actionChildNode.Visit(elapsedTime, dataContext);

                    if (actionStatus == BehaviourReturnCode.Running)
                    {
                        runningNode = actionChildNode;
                        again = false;
                    }
                    else if (actionStatus == BehaviourReturnCode.Success)
                    {
                        actionSuccess = true;
                        runningNode = null;
                    }
                    else
                    {
                        actionSuccess = false;
                        runningNode = null;
                        again = false;
                    }
                }
                else if (conditionStatus == BehaviourReturnCode.Running)
                {
                    runningNode = conditionChildNode;
                    again = false;
                }
                else if (conditionStatus == BehaviourReturnCode.Failure || conditionStatus == BehaviourReturnCode.Error)
                {
                    runningNode = null;
                    again = false;
                }
            }

            if (runningNode != null)
                return Running();

            if (actionSuccess)
                return Succeed();

            return Failed();
        }

        public override IBehaviourTreeState GetState()
        {
            var state = base.GetState();
            state.Children.Add(conditionChildNode.GetState());
            state.Children.Add(actionChildNode.GetState());
            return state;
        }

        public override void ResetState()
        {
            base.ResetState();
            conditionChildNode?.ResetState();
            actionChildNode?.ResetState();
        }
    }
}
