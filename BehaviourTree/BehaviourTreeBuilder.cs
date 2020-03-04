using System;
using System.Collections.Generic;
using System.Text;

namespace AYLib.BehaviourTree
{
    /// <summary>
    /// Fluent API class to help build behaviour trees.
    /// 
    /// Non-leaf nodes must be ended with a call to End(), including the root node.
    /// 
    /// Calling Build() will verify tree integrity and give an interface to use for visiting the tree.
    /// 
    /// Example use:
    /// 
    /// BehaviourTreeBuilder builder = new BehaviourTreeBuilder("Main");
    /// var treeRoot = 
    ///     builder
    ///         .Sequence("Node 1")
    ///             .Selector("Node 2")
    ///                 .Action("Do 2", (x, y) => BehaviourReturnCode.Failure)
    ///                 .Action("Do 3", (x, y) => BehaviourReturnCode.Failure)
    ///             .End()
    ///             .Action("Do 1", (x, y) => BehaviourReturnCode.Success)
    ///             .Action("Do 4", (x, y) => BehaviourReturnCode.Success)
    ///         .End()
    ///     .End()
    ///     .Build()
    /// </summary>
    public class BehaviourTreeBuilder<TTime, TContext>
    {
        private BehaviourTree<TTime, TContext> rootNode = null;
        private Stack<IBehaviourTreeParentNode<TTime, TContext>> parentNodeStack = new Stack<IBehaviourTreeParentNode<TTime, TContext>>();

        /// <summary>
        /// Constructor. Creates the root node to build on.
        /// </summary>
        /// <param name="rootName">User friendly name of the node</param>
        public BehaviourTreeBuilder(string rootName)
        {
            rootNode = new BehaviourTree<TTime, TContext>(rootName);
            parentNodeStack.Push(rootNode);
        }

        /// <summary>
        /// Adds an action node to the current node. Leaf node.
        /// </summary>
        /// <param name="nodeName">User friendly name of the node</param>
        /// <param name="action">The action the node will perform (given elapsedTime and dataContext)</param>
        /// <returns>Current tree builder object</returns>
        public BehaviourTreeBuilder<TTime, TContext> Action(string nodeName, Func<TTime, TContext, BehaviourReturnCode> action)
        {
            var node = new ActionNode<TTime, TContext>(nodeName, action);
            parentNodeStack.Peek().AddChild(node);
            return this;
        }

        /// <summary>
        /// Adds an inverter node to the current node. Leaf node.
        /// </summary>
        /// <param name="nodeName">User friendly name of the node</param>
        /// <param name="action">The action the node will perform (given elapsedTime and dataContext)</param>
        /// <returns>Current tree builder object</returns>
        public BehaviourTreeBuilder<TTime, TContext> Invert(string nodeName, Func<TTime, TContext, BehaviourReturnCode> action)
        {
            var invert = new InverterNode<TTime, TContext>(nodeName, new ActionNode<TTime, TContext>(nodeName, action));
            parentNodeStack.Peek().AddChild(invert);
            return this;
        }

        /// <summary>
        /// Adds a repeating node to the current node. Leaf node.
        /// </summary>
        /// <param name="nodeName">User friendly name of the node</param>
        /// <param name="action">The action the node will perform (given elapsedTime and dataContext)</param>
        /// <param name="repeatCount">The number of times to repeat the action</param>
        /// <returns>Current tree builder object</returns>
        public BehaviourTreeBuilder<TTime, TContext> Repeat(string nodeName, Func<TTime, TContext, BehaviourReturnCode> action, uint repeatCount)
        {
            var repeat = new RepeatNode<TTime, TContext>(nodeName, new ActionNode<TTime, TContext>(nodeName, action), repeatCount);
            parentNodeStack.Peek().AddChild(repeat);
            return this;
        }

        /// <summary>
        /// Adds a succeed node to the current node. Leaf node.
        /// </summary>
        /// <param name="nodeName">User friendly name of the node</param>
        /// <param name="action">The action the node will perform (given elapsedTime and dataContext)</param>
        /// <returns>Current tree builder object</returns>
        public BehaviourTreeBuilder<TTime, TContext> Succeed(string nodeName, Func<TTime, TContext, BehaviourReturnCode> action)
        {
            var succeed = new SucceedNode<TTime, TContext>(nodeName, new ActionNode<TTime, TContext>(nodeName, action));
            parentNodeStack.Peek().AddChild(succeed);
            return this;
        }

        /// <summary>
        /// Adds a while node to the current node. Leaf node.
        /// </summary>
        /// <param name="nodeName">User friendly name of the node</param>
        /// <param name="condition">The condition action the node will check (given elapsedTime and dataContext)</param>
        /// <param name="action">The action the node will perform (given elapsedTime and dataContext)</param>
        /// <returns>Current tree builder object</returns>
        public BehaviourTreeBuilder<TTime, TContext> While(string nodeName, Func<TTime, TContext, BehaviourReturnCode> condition, Func<TTime, TContext, BehaviourReturnCode> action)
        {
            var succeed = new WhileNode<TTime, TContext>(nodeName, new ActionNode<TTime, TContext>(nodeName, condition), new ActionNode<TTime, TContext>(nodeName, action));
            parentNodeStack.Peek().AddChild(succeed);
            return this;
        }

        /// <summary>
        /// Adds a wait node to the current node. Leaf node.
        /// </summary>
        /// <param name="nodeName">User friendly name of the node</param>
        /// <param name="ticks">The time to wait</param>
        /// <returns>Current tree builder object</returns>
        public BehaviourTreeBuilder<TTime, TContext> Wait(string nodeName, long ticks)
        {
            var waitNode = new WaitNode<TTime, TContext>(nodeName, ticks);
            parentNodeStack.Peek().AddChild(waitNode);
            return this;
        }

        /// <summary>
        /// Adds a selector node to the current node. Children will attach until End is called.
        /// </summary>
        /// <param name="nodeName">User friendly name of the node</param>
        /// <returns>Current tree builder object</returns>
        public BehaviourTreeBuilder<TTime, TContext> Selector(string nodeName)
        {
            var node = new SelectorNode<TTime, TContext>(nodeName);
            parentNodeStack.Push(node);
            return this;
        }

        /// <summary>
        /// Adds a sequence node to the current node. Children will attach until End is called.
        /// </summary>
        /// <param name="nodeName">User friendly name of the node</param>
        /// <returns>Current tree builder object</returns>
        public BehaviourTreeBuilder<TTime, TContext> Sequence(string nodeName)
        {
            var node = new SequenceNode<TTime, TContext>(nodeName);
            parentNodeStack.Push(node);
            return this;
        }

        /// <summary>
        /// Adds a parallel node to the current node. Children will attach until End is called.
        /// </summary>
        /// <param name="nodeName">User friendly name of the node</param>
        /// <returns>Current tree builder object</returns>
        public BehaviourTreeBuilder<TTime, TContext> Parallel(string nodeName)
        {
            var node = new ParallelNode<TTime, TContext>(nodeName);
            parentNodeStack.Push(node);
            return this;
        }

        /// <summary>
        /// Adds a race node to the current node. Children will attach until End is called.
        /// </summary>
        /// <param name="nodeName">User friendly name of the node</param>
        /// <returns>Current tree builder object</returns>
        public BehaviourTreeBuilder<TTime, TContext> Race(string nodeName)
        {
            var node = new RaceNode<TTime, TContext>(nodeName);
            parentNodeStack.Push(node);
            return this;
        }

        /// <summary>
        /// Adds a random node to the current node. Children will attach until End is called.
        /// </summary>
        /// <param name="nodeName">User friendly name of the node</param>
        /// <returns>Current tree builder object</returns>
        public BehaviourTreeBuilder<TTime, TContext> Random(string nodeName)
        {
            var node = new RandomNode<TTime, TContext>(nodeName);
            parentNodeStack.Push(node);
            return this;
        }

        /// <summary>
        /// Splices a given tree into the current node.
        /// </summary>
        /// <param name="subTree">The pre-built subtree that will be attached to the current node</param>
        /// <returns>Current tree builder object</returns>
        public BehaviourTreeBuilder<TTime, TContext> Splice(IBehaviourTreeNode<TTime, TContext> subTree)
        {
            parentNodeStack.Peek().AddChild(subTree as BehaviourNode<TTime, TContext>);
            return this;
        }

        /// <summary>
        /// Validates the structure of the current tree, and returns an interface to interact with.
        /// </summary>
        /// <returns>Current tree builder object</returns>
        public IBehaviourTreeNode<TTime, TContext> Build()
        {
            if (parentNodeStack.Count > 1)
                throw new ApplicationException("Sequence type nodes must be closed with and End().");
            if (rootNode.ChildNode == null)
                throw new ApplicationException("Root node cannot have a null child.");
            return rootNode;
        }

        /// <summary>
        /// Closes a non-leaf node of the tree.
        /// </summary>
        /// <returns>Current tree builder object</returns>
        public BehaviourTreeBuilder<TTime, TContext> End()
        {
            var node = parentNodeStack.Pop();
            if (!(node is BehaviourTree<TTime, TContext>))
                parentNodeStack.Peek().AddChild(node as BehaviourNode<TTime, TContext>);
            return this;
        }
    }
}
