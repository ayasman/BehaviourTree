using System;

namespace AYLib.BehaviourTree
{
    /// <summary>
    /// Waits a given amount of time (ticks), then returns a success.
    /// </summary>
    internal class WaitNode : BehaviourNode
    {
        private long elapsedTimeToWait;
        private long currentTickCounter;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="nodeName">User friendly name of the node</param>
        /// <param name="elapsedTimeToWait">Amount of time to wait</param>
        public WaitNode(string nodeName, long elapsedTimeToWait)
            : base(nodeName)
        {
            this.elapsedTimeToWait = elapsedTimeToWait;
            currentTickCounter = elapsedTimeToWait;
        }

        /// <summary>
        /// Counts down from initial time to wait, returns Succeed when done.
        /// </summary>
        /// <param name="elapsedTime">The time since last visit</param>
        /// <param name="dataContext">The data context to run against</param>
        /// <returns>Completion state of the node</returns>
        public override BehaviourReturnCode Visit(long elapsedTime, object dataContext)
        {
            currentTickCounter -= elapsedTime;
            if (currentTickCounter <= 0)
            {
                currentTickCounter = elapsedTimeToWait;
                return Succeed();
            }

            return Running();
        }

        /// <summary>
        /// Gets the state data of the node.
        /// </summary>
        /// <returns>Node state data</returns>
        public override IBehaviourTreeState GetState()
        {
            var state = base.GetState();
            return state;
        }

        /// <summary>
        /// Resets the node state to ready.
        /// </summary>
        public override void ResetState()
        {
            base.ResetState();
        }
    }
}
