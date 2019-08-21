using System;
using System.Collections.Generic;
using System.Linq;

namespace AYLib.BehaviourTree
{
    /// <summary>
    /// Visits all of the children during every call to the Visit method. Returns a failure result when the first one
    /// fails, a success result if they all succeed, or running if any are still working.
    /// </summary>
    public class ParallelNode : BehaviourNode, IBehaviourTreeParentNode
    {
        private readonly List<BehaviourNode> childNodes = null;
        private readonly List<BehaviourNode> runningNodes = new List<BehaviourNode>();

        public ParallelNode(string nodeName)
            : base(nodeName)
        {
            childNodes = new List<BehaviourNode>();
        }

        public ParallelNode(string nodeName, List<BehaviourNode> childNodes)
            : base(nodeName)
        {
            this.childNodes = childNodes;
        }

        public override BehaviourReturnCode Visit(long elapsedTime, object dataContext)
        {
            bool anyFailed = false;

            var processingNodes = runningNodes.Count == 0
                                    ? childNodes
                                    : runningNodes.ToList();

            foreach (var node in processingNodes)
            {
                var status = node.Visit(elapsedTime, dataContext);

                if (status == BehaviourReturnCode.Running && !runningNodes.Contains(node))
                    runningNodes.Add(node);
                if (status == BehaviourReturnCode.Success && runningNodes.Contains(node))
                    runningNodes.Remove(node);
                if (status == BehaviourReturnCode.Failure)
                    anyFailed = true;
            }

            if (anyFailed)
            {
                runningNodes.Clear();
                return Failed();
            }
            if (runningNodes.Count > 0)
                return Running();
            return Succeed();
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
