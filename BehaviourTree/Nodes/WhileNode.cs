using System;

namespace AYLib.BehaviourTree
{
    internal class WhileNode<TTime, TContext> : BehaviourNode<TTime, TContext>
    {
        private readonly BehaviourNode<TTime, TContext> conditionChildNode;
        private readonly BehaviourNode<TTime, TContext> actionChildNode;
        private BehaviourNode<TTime, TContext> runningNode = null;
        private bool actionSuccess = false;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="nodeName">User friendly name of the node</param>
        /// <param name="conditionNode">The condition node that is checked to perform loop</param>
        /// <param name="childNode">The action node that is visited</param>
        public WhileNode(string nodeName, ActionNode<TTime, TContext> conditionNode, ActionNode<TTime, TContext> childNode)
            : base(nodeName)
        {
            conditionChildNode = conditionNode;
            actionChildNode = childNode;
        }

        /// <summary>
        /// Calls the condition node and, if passes, the child action node.
        /// </summary>
        /// <param name="elapsedTime">The time since last visit</param>
        /// <param name="dataContext">The data context to run against</param>
        /// <returns>Completion state of the node</returns>
        public override BehaviourReturnCode Visit(TTime elapsedTime, TContext dataContext)
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

        /// <summary>
        /// Gets the state data of the node.
        /// </summary>
        /// <returns>Node state data</returns>
        public override IBehaviourTreeState GetState()
        {
            var state = base.GetState();
            state.Children.Add(conditionChildNode.GetState());
            state.Children.Add(actionChildNode.GetState());
            return state;
        }

        /// <summary>
        /// Resets the node state to ready.
        /// </summary>
        public override void ResetState()
        {
            base.ResetState();
            conditionChildNode?.ResetState();
            actionChildNode?.ResetState();
        }
    }
}
