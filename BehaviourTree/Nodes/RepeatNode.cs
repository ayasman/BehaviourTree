using System;

namespace AYLib.BehaviourTree
{
    /// <summary>
    /// Repeats the an action a given number of times. Succeeds if all calls to the action succeed, and fails
    /// if any call fails. Returns running otherwise. An error is returned if the count is in an invalid state.
    /// </summary>
    public class RepeatNode : BehaviourNode
    {
        private readonly ActionNode childNode;
        private readonly uint repeatCount;
        private uint currentCount = 0;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="nodeName">User friendly name of the node</param>
        /// <param name="childAction">The action to take when visiting the node</param>
        /// <param name="repeatCount">The number of times to repeat the action</param>
        public RepeatNode(string nodeName, ActionNode childAction, uint repeatCount)
            : base(nodeName)
        {
            this.childNode = childAction;
            this.repeatCount = currentCount = repeatCount;
        }

        /// <summary>
        /// Calls the action for the node.
        /// </summary>
        /// <param name="elapsedTime">The time since last visit</param>
        /// <param name="dataContext">The data context to run against</param>
        /// <returns>Completion state of the node</returns>
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
