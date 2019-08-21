using System;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("BehaviourTreeTests")]

namespace AYLib.BehaviourTree
{
    /// <summary>
    /// Top level of a behaviour tree, and contains a single child node, which can be of any type.
    /// While not totally necessary to use this as the top level, the Visit method will be used
    /// to reset the state in the tree after a completed run.
    /// </summary>
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

            if (CurrentState != BehaviourReturnCode.Running)
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
