using AYLib.BehaviourTree;
using System;
using Xunit;

namespace BehaviourTreeTests
{
    public class NodeStateTests
    {
        [Fact]
        public void TestSuccess()
        {
            BehaviourTreeBuilder<long, object> behaviourTreeNode = new BehaviourTreeBuilder<long, object>("Tree");
            var tree = behaviourTreeNode
                    .Sequence("Sequence")
                        .Action("Action", (x, y) => BehaviourReturnCode.Success)
                        .Action("Action", (x, y) => BehaviourReturnCode.Success)
                    .End()
                .End()
                .Build();

            BehaviourTreeBuilder<long, object> behaviourTreeNode2 = new BehaviourTreeBuilder<long, object>("Tree");
            var tree2 = behaviourTreeNode2
                    .Sequence("Sequence")
                        .Action("Action", (x, y) => BehaviourReturnCode.Success)
                        .Action("Action", (x, y) => BehaviourReturnCode.Success)
                    .End()
                .End()
                .Build();

            IBehaviourTreeState state1 = tree.GetState();
            IBehaviourTreeState state2 = tree2.GetState();

            Assert.True(state1.StateEqual(state2));

            tree.Visit(100, null);
            tree2.Visit(100, null);

            state1 = tree.GetState();
            state2 = tree2.GetState();

            Assert.True(state1.StateEqual(state2));
        }

        [Fact]
        public void TestFailureName()
        {
            BehaviourTreeBuilder<long, object> behaviourTreeNode = new BehaviourTreeBuilder<long, object>("Tree");
            var tree = behaviourTreeNode
                    .Sequence("Sequence")
                        .Action("Action", (x, y) => BehaviourReturnCode.Success)
                        .Action("Action", (x, y) => BehaviourReturnCode.Success)
                    .End()
                .End()
                .Build();

            BehaviourTreeBuilder<long, object> behaviourTreeNode2 = new BehaviourTreeBuilder<long, object>("Tree");
            var tree2 = behaviourTreeNode2
                    .Sequence("Sequence")
                        .Action("Noitca", (x, y) => BehaviourReturnCode.Success)
                        .Action("Action", (x, y) => BehaviourReturnCode.Success)
                    .End()
                .End()
                .Build();

            IBehaviourTreeState state1 = tree.GetState();
            IBehaviourTreeState state2 = tree2.GetState();

            Assert.False(state1.StateEqual(state2));
        }

        [Fact]
        public void TestFailureResult()
        {
            BehaviourTreeBuilder<long, object> behaviourTreeNode = new BehaviourTreeBuilder<long, object>("Tree");
            var tree = behaviourTreeNode
                    .Sequence("Sequence")
                        .Action("Action", (x, y) => BehaviourReturnCode.Failure)
                        .Action("Action", (x, y) => BehaviourReturnCode.Success)
                    .End()
                .End()
                .Build();

            BehaviourTreeBuilder<long, object> behaviourTreeNode2 = new BehaviourTreeBuilder<long, object>("Tree");
            var tree2 = behaviourTreeNode2
                    .Sequence("Sequence")
                        .Action("Action", (x, y) => BehaviourReturnCode.Success)
                        .Action("Action", (x, y) => BehaviourReturnCode.Success)
                    .End()
                .End()
                .Build();

            tree.Visit(100, null);
            tree2.Visit(100, null);

            IBehaviourTreeState state1 = tree.GetState();
            IBehaviourTreeState state2 = tree2.GetState();

            Assert.False(state1.StateEqual(state2));
        }

        [Fact]
        public void TestFailureSize()
        {
            BehaviourTreeBuilder<long, object> behaviourTreeNode = new BehaviourTreeBuilder<long, object>("Tree");
            var tree = behaviourTreeNode
                    .Sequence("Sequence")
                        .Action("Action", (x, y) => BehaviourReturnCode.Success)
                        .Action("Action", (x, y) => BehaviourReturnCode.Success)
                    .End()
                .End()
                .Build();

            BehaviourTreeBuilder<long, object> behaviourTreeNode2 = new BehaviourTreeBuilder<long, object>("Tree");
            var tree2 = behaviourTreeNode2
                    .Sequence("Sequence")
                        .Action("Action", (x, y) => BehaviourReturnCode.Success)
                        .Action("Action", (x, y) => BehaviourReturnCode.Success)
                        .Action("Action", (x, y) => BehaviourReturnCode.Success)
                    .End()
                .End()
                .Build();

            IBehaviourTreeState state1 = tree.GetState();
            IBehaviourTreeState state2 = tree2.GetState();

            Assert.False(state1.StateEqual(state2));
        }
    }
}
