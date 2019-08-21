using System;
using System.Collections.Generic;
using System.Linq;

namespace AYLib.BehaviourTree
{
    /// <summary>
    /// Visits all of the children during every call to the Visit method. Returns a success result when the first one
    /// succeeds, a failure result if they all failed, or running if any are still working.
    /// </summary>
    public class RaceNode : BehaviourNode, IBehaviourTreeParentNode
    {
        private readonly List<BehaviourNode> childNodes = null;
        private readonly List<BehaviourNode> runningNodes = new List<BehaviourNode>();

        public RaceNode(string nodeName)
            : base(nodeName)
        {
            childNodes = new List<BehaviourNode>();
        }

        public RaceNode(string nodeName, List<BehaviourNode> childNodes)
            : base(nodeName)
        {
            this.childNodes = childNodes;
        }

        public override BehaviourReturnCode Visit(long elapsedTime, object dataContext)
        {
            bool anySuceeded = false;

            var processingNodes = runningNodes.Count == 0
                                    ? childNodes
                                    : runningNodes.ToList();

            foreach (var node in processingNodes)
            {
                var status = node.Visit(elapsedTime, dataContext);

                if (status == BehaviourReturnCode.Running && !runningNodes.Contains(node))
                    runningNodes.Add(node);
                if (status == BehaviourReturnCode.Success)
                    anySuceeded = true;
                if (status == BehaviourReturnCode.Failure && runningNodes.Contains(node))
                    runningNodes.Remove(node);
            }

            if (anySuceeded)
            {
                runningNodes.Clear();
                return Succeed();
            }
            if (runningNodes.Count > 0)
                return Running();
            return Failed();
        }

        public void AddChild(BehaviourNode childNode)
        {
            childNodes.Add(childNode);
        }

        public override IBehaviourTreeState GetState()
        {
            var state = base.GetState();
            foreach (var node in childNodes)
                state.Children.Add(node.GetState());
            return state;
        }

        public override void ResetState()
        {
            base.ResetState();
            foreach (var node in childNodes)
                node.ResetState();
        }
    }
}
