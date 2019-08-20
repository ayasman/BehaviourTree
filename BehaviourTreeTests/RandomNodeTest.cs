using C4i.BehaviourTree;
using System;
using Xunit;

namespace BehaviourTreeTests
{
    public class RandomNodeTest
    {
        [Fact]
        public void TestSuccessOne()
        {
            ActionNode successNode = new ActionNode("SuccessNode", (t, o) =>
                {
                    return BehaviourReturnCode.Success;
                });

            RandomNode sequence = new RandomNode("RandomNode", new System.Collections.Generic.List<BehaviourNode>() { successNode });

            var status = sequence.Visit(1, null);
            Assert.Equal(BehaviourReturnCode.Success, sequence.CurrentState);
            Assert.Equal(BehaviourReturnCode.Success, status);
        }

        [Fact]
        public void TestSuccessMany()
        {
            ActionNode successNode1 = new ActionNode("SuccessNode", (t, o) =>
            {
                return BehaviourReturnCode.Success;
            });
            ActionNode successNode2 = new ActionNode("SuccessNode", (t, o) =>
            {
                return BehaviourReturnCode.Success;
            });
            ActionNode successNode3 = new ActionNode("SuccessNode", (t, o) =>
            {
                return BehaviourReturnCode.Success;
            });

            RandomNode sequence = new RandomNode("RandomNode", new System.Collections.Generic.List<BehaviourNode>() { successNode1, successNode2, successNode3 });

            var status = sequence.Visit(1, null);
            Assert.Equal(BehaviourReturnCode.Success, sequence.CurrentState);
            Assert.Equal(BehaviourReturnCode.Success, status);
        }

        [Fact]
        public void TestReenter()
        {
            bool hitVisit = false;

            ActionNode successNode1 = new ActionNode("SuccessNode", (t, o) =>
            {
                if (!hitVisit)
                {
                    hitVisit = true;
                    return BehaviourReturnCode.Running;
                }
                return BehaviourReturnCode.Success;
            });

            RandomNode sequence = new RandomNode("RandomNode", new System.Collections.Generic.List<BehaviourNode>() { successNode1 });

            var status = sequence.Visit(1, null);
            Assert.Equal(BehaviourReturnCode.Running, sequence.CurrentState);
            Assert.Equal(BehaviourReturnCode.Running, status);

            status = sequence.Visit(1, null);
            Assert.Equal(BehaviourReturnCode.Success, sequence.CurrentState);
            Assert.Equal(BehaviourReturnCode.Success, status);
            Assert.True(hitVisit);
        }

        [Fact]
        public void TestFail()
        {
            ActionNode failNode = new ActionNode("FailNode", (t, o) =>
            {
                return BehaviourReturnCode.Failure;
            });

            RandomNode sequence = new RandomNode("RandomNode", new System.Collections.Generic.List<BehaviourNode>() { failNode });

            var status = sequence.Visit(1, null);
            Assert.Equal(BehaviourReturnCode.Failure, sequence.CurrentState);
            Assert.Equal(BehaviourReturnCode.Failure, status);
        }

        [Fact]
        public void TestError()
        {
            RandomNode sequence = new RandomNode("RandomNode");
            var status = sequence.Visit(1, null);
            Assert.Equal(BehaviourReturnCode.Error, sequence.CurrentState);
            Assert.Equal(BehaviourReturnCode.Error, status);
        }

        [Fact]
        public void TestAddChild()
        {
            bool visited = false;
            ActionNode successNode = new ActionNode("SuccessNode", (t, o) =>
            {
                visited = true;
                return BehaviourReturnCode.Success;
            });

            RandomNode sequence = new RandomNode("RandomNode");
            sequence.AddChild(successNode);

            var status = sequence.Visit(1, null);
            Assert.Equal(BehaviourReturnCode.Success, sequence.CurrentState);
            Assert.Equal(BehaviourReturnCode.Success, status);
            Assert.True(visited);
        }

        [Fact]
        public void TestGetState()
        {
            ActionNode successNode = new ActionNode("SuccessNode", (t, o) =>
            {
                return BehaviourReturnCode.Success;
            });

            RandomNode sequence = new RandomNode("RandomNode", new System.Collections.Generic.List<BehaviourNode>() { successNode });
            var status = sequence.Visit(1, null);

            var state = sequence.GetState();

            Assert.NotNull(state);
            Assert.Equal("RandomNode", state.NodeName);
            Assert.Equal(BehaviourReturnCode.Success, state.CurrentState);
            Assert.NotEmpty(state.Children);
            Assert.Single(state.Children);
        }
    }
}
