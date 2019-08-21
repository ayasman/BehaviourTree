using System;
using System.Collections.Generic;

namespace AYLib.BehaviourTree
{
    /// <summary>
    /// Visits one of the child nodes at random, and continues visiting that child until it finishes running.
    /// </summary>
    public class RandomNode : BehaviourNode, IBehaviourTreeParentNode
    {
        private readonly List<BehaviourNode> childNodes = null;
        private BehaviourNode runningNode = null;
        private Random random = new Random();

        public RandomNode(string nodeName)
            : base(nodeName)
        {
            childNodes = new List<BehaviourNode>();
        }

        public RandomNode(string nodeName, List<BehaviourNode> childNodes)
            : base(nodeName)
        {
            this.childNodes = childNodes;
        }

        public override BehaviourReturnCode Visit(long elapsedTime, object dataContext)
        {
            if (childNodes.Count == 0)
                return Error();

            var processingNode = runningNode ?? childNodes[random.Next(childNodes.Count)];

            var status = processingNode.Visit(elapsedTime, dataContext);
            if (status == BehaviourReturnCode.Running)
                runningNode = processingNode;
            else
                runningNode = null;
            return Return(status);
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
