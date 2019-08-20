using System;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("BehaviourTreeTests")]

namespace C4i.BehaviourTree
{
    public class BehaviourTree : BehaviourNode, IBehaviourTreeParentNode
    {
        private BehaviourNode childNode;

        internal BehaviourNode ChildNode => childNode;

        public BehaviourTree(string nodeName)
            : base(nodeName)
        {

        }

        public BehaviourTree(string nodeName, BehaviourNode childNode)
            : base(nodeName)
        {
            AddChild(childNode);
        }

        public void AddChild(BehaviourNode childNode)
        {
            this.childNode = childNode;
        }

        public override BehaviourReturnCode Visit(long elapsedTime, object dataContext)
        {
            if (childNode == null)
                throw new ApplicationException("Root node cannot have a null child.");

            if (CurrentState != BehaviourReturnCode.Success)
                ResetState();

            return Return(childNode.Visit(elapsedTime, dataContext));
        }

        public override IBehaviourTreeState GetState()
        {
            var state = base.GetState();
            state.Children.Add(childNode?.GetState());
            return state;
        }

        public override void ResetState()
        {
            base.ResetState();
            childNode?.ResetState();
        }
    }
}
