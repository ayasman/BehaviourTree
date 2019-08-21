using System;
using System.Collections.Generic;
using System.Linq;

namespace AYLib.BehaviourTree
{
    /// <summary>
    /// Visits all of the child nodes in order until one fails or they all succeed.
    /// </summary>
    public class SequenceNode : BehaviourNode, IBehaviourTreeParentNode
    {
        private readonly List<BehaviourNode> childNodes = null;
        private BehaviourNode runningNode = null;

        public SequenceNode(string nodeName) 
            : base(nodeName)
        {
            childNodes = new List<BehaviourNode>();
        }

        public SequenceNode(string nodeName, List<BehaviourNode> childNodes)
            : base(nodeName)
        {
            this.childNodes = childNodes;
        }

        public override BehaviourReturnCode Visit(long elapsedTime, object dataContext)
        {
            var processingNodes = runningNode == null
                                    ? childNodes
                                    : childNodes.SkipWhile(node => node != runningNode);

            foreach (var currentNode in processingNodes)
            {
                var status = currentNode.Visit(elapsedTime, dataContext);
                if (status == BehaviourReturnCode.Running)
                    runningNode = currentNode;
                else
                    runningNode = null;

                if (status != BehaviourReturnCode.Success)
                    return Return(status);
            }

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
