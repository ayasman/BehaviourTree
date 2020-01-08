using AYLib.BehaviourTree;
using System;
using Xunit;

namespace BehaviourTreeTests
{
    public class BehaviourTreeBuilderTests
    {
        BehaviourTreeBuilder behaviourTreeNode = new BehaviourTreeBuilder("Tree");

        [Fact]
        public void TestNoChildrenCreated()
        { 
            Assert.Throws<ApplicationException>(() => behaviourTreeNode.Build());
        }

        [Fact]
        public void TestUnbalancedTree()
        {
            Assert.Throws<ApplicationException>(() => behaviourTreeNode
                .Sequence("SequenceNode")
                .Build());
        }

        [Fact]
        public void TestActionAddActionNode()
        {
            var tree = behaviourTreeNode
                    .Action("Action", (x, y) => BehaviourReturnCode.Success)
                .End()
                .Build();

            Assert.IsType<ActionNode>(((BehaviourTree)tree).ChildNode);
            Assert.Equal(BehaviourReturnCode.Success, tree.Visit(1, null));
        }

        [Fact]
        public void TestActionAddInverterNode()
        {
            var tree = behaviourTreeNode
                    .Invert("Invert", (x, y) => BehaviourReturnCode.Success)
                .End()
                .Build();

            Assert.IsType<InverterNode>(((BehaviourTree)tree).ChildNode);
            Assert.Equal(BehaviourReturnCode.Failure, tree.Visit(1, null));
        }

        [Fact]
        public void TestActionAddRepeatNode()
        {
            var tree = behaviourTreeNode
                    .Repeat("Repeat", (x, y) => BehaviourReturnCode.Success, 1)
                .End()
                .Build();

            Assert.IsType<RepeatNode>(((BehaviourTree)tree).ChildNode);
            Assert.Equal(BehaviourReturnCode.Success, tree.Visit(1, null));
        }

        [Fact]
        public void TestActionAddParallelNode()
        {
            var tree = behaviourTreeNode
                    .Parallel("Parallel")
                        .Action("Action", (x, y) => BehaviourReturnCode.Success)
                    .End()
                .End()
                .Build();

            Assert.IsType<ParallelNode>(((BehaviourTree)tree).ChildNode);
            Assert.Equal(BehaviourReturnCode.Success, tree.Visit(1, null));
        }

        [Fact]
        public void TestActionAddRaceNode()
        {
            var tree = behaviourTreeNode
                    .Race("Race")
                        .Action("Action", (x, y) => BehaviourReturnCode.Success)
                    .End()
                .End()
                .Build();

            Assert.IsType<RaceNode>(((BehaviourTree)tree).ChildNode);
            Assert.Equal(BehaviourReturnCode.Success, tree.Visit(1, null));
        }

        [Fact]
        public void TestActionAddRandomNode()
        {
            var tree = behaviourTreeNode
                    .Random("Random")
                        .Action("Action", (x, y) => BehaviourReturnCode.Success)
                    .End()
                .End()
                .Build();

            Assert.IsType<RandomNode>(((BehaviourTree)tree).ChildNode);
            Assert.Equal(BehaviourReturnCode.Success, tree.Visit(1, null));
        }

        [Fact]
        public void TestActionAddSelectorNode()
        {
            var tree = behaviourTreeNode
                    .Selector("Selector")
                        .Action("Action", (x, y) => BehaviourReturnCode.Failure)
                        .Action("Action", (x, y) => BehaviourReturnCode.Success)
                    .End()
                .End()
                .Build();

            Assert.IsType<SelectorNode>(((BehaviourTree)tree).ChildNode);
            Assert.Equal(BehaviourReturnCode.Success, tree.Visit(1, null));
        }

        [Fact]
        public void TestActionAddSequenceNode()
        {
            var tree = behaviourTreeNode
                    .Sequence("Sequence")
                        .Action("Action", (x, y) => BehaviourReturnCode.Success)
                        .Action("Action", (x, y) => BehaviourReturnCode.Success)
                    .End()
                .End()
                .Build();

            Assert.IsType<SequenceNode>(((BehaviourTree)tree).ChildNode);
            Assert.Equal(BehaviourReturnCode.Success, tree.Visit(1, null));
        }

        [Fact]
        public void TestActionAddSucceedNode()
        {
            var tree = behaviourTreeNode
                    .Succeed("Succeed", (x, y) => BehaviourReturnCode.Failure)
                .End()
                .Build();

            Assert.IsType<SucceedNode>(((BehaviourTree)tree).ChildNode);
            Assert.Equal(BehaviourReturnCode.Success, tree.Visit(1, null));
        }

        [Fact]
        public void TestActionSplice()
        {
            BehaviourTreeBuilder secondTree = new BehaviourTreeBuilder("Tree");
            var t2 = secondTree
                .Action("Action", (x, y) => BehaviourReturnCode.Failure)
                .Build();

            var tree = behaviourTreeNode
                    .Splice(t2)
                .End()
                .Build();

            Assert.IsType<ActionNode>(((BehaviourTree)t2).ChildNode);
            Assert.IsType<BehaviourTree>(((BehaviourTree)tree).ChildNode);
            Assert.Equal(BehaviourReturnCode.Failure, tree.Visit(1, null));
        }

        [Fact]
        public void TestActionAddWhileNode()
        {
            var tree = behaviourTreeNode
                    .While("While", (x, y) => BehaviourReturnCode.Success, (x, y) => BehaviourReturnCode.Failure)
                .End()
                .Build();

            Assert.IsType<WhileNode>(((BehaviourTree)tree).ChildNode);
            Assert.Equal(BehaviourReturnCode.Failure, tree.Visit(1, null));
        }

        [Fact]
        public void TestActionAddWaitNode()
        {
            var tree = behaviourTreeNode
                    .Wait("Wait", 1000)
                .End()
                .Build();

            Assert.IsType<WaitNode>(((BehaviourTree)tree).ChildNode);
            Assert.Equal(BehaviourReturnCode.Running, tree.Visit(1, null));
        }
    }
}
