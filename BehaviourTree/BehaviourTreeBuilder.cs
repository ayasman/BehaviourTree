using System;
using System.Collections.Generic;
using System.Text;

namespace C4i.BehaviourTree
{
    public class BehaviourTreeBuilder
    {
        private BehaviourTree rootNode = null;
        private Stack<IBehaviourTreeParentNode> parentNodeStack = new Stack<IBehaviourTreeParentNode>();

        public BehaviourTreeBuilder(string rootName)
        {
            rootNode = new BehaviourTree(rootName);
            parentNodeStack.Push(rootNode);
        }

        public BehaviourTreeBuilder Action(string nodeName, Func<double, object, BehaviourReturnCode> action)
        {
            var node = new ActionNode(nodeName, action);
            parentNodeStack.Peek().AddChild(node);
            return this;
        }

        public BehaviourTreeBuilder Invert(string nodeName, Func<double, object, BehaviourReturnCode> action)
        {
            var invert = new InverterNode(nodeName, new ActionNode(nodeName, action));
            parentNodeStack.Peek().AddChild(invert);
            return this;
        }

        public BehaviourTreeBuilder Repeat(string nodeName, Func<double, object, BehaviourReturnCode> action, uint repeatCount)
        {
            var repeat = new RepeatNode(nodeName, new ActionNode(nodeName, action), repeatCount);
            parentNodeStack.Peek().AddChild(repeat);
            return this;
        }

        public BehaviourTreeBuilder Succeed(string nodeName, Func<double, object, BehaviourReturnCode> action)
        {
            var succeed = new SucceedNode(nodeName, new ActionNode(nodeName, action));
            parentNodeStack.Peek().AddChild(succeed);
            return this;
        }

        public BehaviourTreeBuilder While(string nodeName, Func<double, object, BehaviourReturnCode> condition, Func<double, object, BehaviourReturnCode> action)
        {
            var succeed = new WhileNode(nodeName, new ActionNode(nodeName, condition), new ActionNode(nodeName, action));
            parentNodeStack.Peek().AddChild(succeed);
            return this;
        }

        public BehaviourTreeBuilder Selector(string nodeName)
        {
            var node = new SelectorNode(nodeName);
            parentNodeStack.Push(node);
            return this;
        }

        public BehaviourTreeBuilder Sequence(string nodeName)
        {
            var node = new SequenceNode(nodeName);
            parentNodeStack.Push(node);
            return this;
        }

        public BehaviourTreeBuilder Parallel(string nodeName)
        {
            var node = new ParallelNode(nodeName);
            parentNodeStack.Push(node);
            return this;
        }

        public BehaviourTreeBuilder Race(string nodeName)
        {
            var node = new RaceNode(nodeName);
            parentNodeStack.Push(node);
            return this;
        }

        public BehaviourTreeBuilder Random(string nodeName)
        {
            var node = new RandomNode(nodeName);
            parentNodeStack.Push(node);
            return this;
        }

        public BehaviourTreeBuilder Splice(IBehaviourTreeNode subTree)
        {
            parentNodeStack.Peek().AddChild(subTree as BehaviourNode);
            return this;
        }

        public IBehaviourTreeNode Build()
        {
            if (parentNodeStack.Count > 1)
                throw new ApplicationException("Sequence type nodes must be closed with and End().");
            if (rootNode.ChildNode == null)
                throw new ApplicationException("Root node cannot have a null child.");
            return rootNode;
        }

        public BehaviourTreeBuilder End()
        {
            var node = parentNodeStack.Pop();
            if (!(node is BehaviourTree))
                parentNodeStack.Peek().AddChild(node as BehaviourNode);
            return this;
        }
    }
}
