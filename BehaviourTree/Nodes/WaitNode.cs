using System;

namespace AYLib.BehaviourTree
{
    /// <summary>
    /// Waits a given amount of time (ticks), then returns a success.
    /// </summary>
    public class WaitNode : BehaviourNode
    {
        private long ticksToWait;
        private long currentTickCounter;

        public WaitNode(string nodeName, long ticksToWait)
            : base(nodeName)
        {
            this.ticksToWait = ticksToWait;
            currentTickCounter = ticksToWait;
        }

        public override BehaviourReturnCode Visit(long elapsedTime, object dataContext)
        {
            currentTickCounter -= elapsedTime;
            if (currentTickCounter <= 0)
            {
                currentTickCounter = ticksToWait;
                return Succeed();
            }

            return Running();
        }

        public override IBehaviourTreeState GetState()
        {
            var state = base.GetState();
            return state;
        }

        public override void ResetState()
        {
            base.ResetState();
        }
    }
}
