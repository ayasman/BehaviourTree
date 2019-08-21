using System;
using System.Collections.Generic;
using System.Linq;

namespace AYLib.BehaviourTree
{
    /// <summary>
    /// Visits all of the child nodes in order until one succeeds or they all fail.
    /// </summary>
    public class SelectorNode : BehaviourNode, IBehaviourTreeParentNode
    {
        private readonly List<BehaviourNode> childNodes = null;
        private BehaviourNode runningNode = null;

        public SelectorNode(string nodeName)
            : base(nodeName)
        {
            childNodes = new List<BehaviourNode>();
        }

        public SelectorNode(string nodeName, List<BehaviourNode> childNodes)
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

                if (status == BehaviourReturnCode.Success ||
                    status == BehaviourReturnCode.Running ||
                    status == BehaviourReturnCode.Error)
                    return Return(status);
            }
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
