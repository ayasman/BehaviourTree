using AYLib.BehaviourTree;
using System;
using Xunit;

namespace BehaviourTreeTests
{
    public class RandomNodeTest
    {
        [Fact]
        public void TestSuccessOne()
        {
            ActionNode<long, object> successNode = new ActionNode<long, object>("SuccessNode", (t, o) =>
                {
                    return BehaviourReturnCode.Success;
                });

            RandomNode<long, object> sequence = new RandomNode<long, object>("RandomNode", new System.Collections.Generic.List<BehaviourNode<long, object>>() { successNode });

            var status = sequence.Visit(1, null);
            Assert.Equal(BehaviourReturnCode.Success, sequence.CurrentState);
            Assert.Equal(BehaviourReturnCode.Success, status);
        }

        [Fact]
        public void TestSuccessMany()
        {
            ActionNode<long, object> successNode1 = new ActionNode<long, object>("SuccessNode", (t, o) =>
            {
                return BehaviourReturnCode.Success;
            });
            ActionNode<long, object> successNode2 = new ActionNode<long, object>("SuccessNode", (t, o) =>
            {
                return BehaviourReturnCode.Success;
            });
            ActionNode<long, object> successNode3 = new ActionNode<long, object>("SuccessNode", (t, o) =>
            {
                return BehaviourReturnCode.Success;
            });

            RandomNode<long, object> sequence = new RandomNode<long, object>("RandomNode", new System.Collections.Generic.List<BehaviourNode<long, object>>() { successNode1, successNode2, successNode3 });

            var status = sequence.Visit(1, null);
            Assert.Equal(BehaviourReturnCode.Success, sequence.CurrentState);
            Assert.Equal(BehaviourReturnCode.Success, status);
        }

        [Fact]
        public void TestReenter()
        {
            bool hitVisit = false;

            ActionNode<long, object> successNode1 = new ActionNode<long, object>("SuccessNode", (t, o) =>
            {
                if (!hitVisit)
                {
                    hitVisit = true;
                    return BehaviourReturnCode.Running;
                }
                return BehaviourReturnCode.Success;
            });

            RandomNode<long, object> sequence = new RandomNode<long, object>("RandomNode", new System.Collections.Generic.List<BehaviourNode<long, object>>() { successNode1 });

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
            ActionNode<long, object> failNode = new ActionNode<long, object>("FailNode", (t, o) =>
            {
                return BehaviourReturnCode.Failure;
            });

            RandomNode<long, object> sequence = new RandomNode<long, object>("RandomNode", new System.Collections.Generic.List<BehaviourNode<long, object>>() { failNode });

            var status = sequence.Visit(1, null);
            Assert.Equal(BehaviourReturnCode.Failure, sequence.CurrentState);
            Assert.Equal(BehaviourReturnCode.Failure, status);
        }

        [Fact]
        public void TestError()
        {
            RandomNode<long, object> sequence = new RandomNode<long, object>("RandomNode");
            var status = sequence.Visit(1, null);
            Assert.Equal(BehaviourReturnCode.Error, sequence.CurrentState);
            Assert.Equal(BehaviourReturnCode.Error, status);
        }

        [Fact]
        public void TestAddChild()
        {
            bool visited = false;
            ActionNode<long, object> successNode = new ActionNode<long, object>("SuccessNode", (t, o) =>
            {
                visited = true;
                return BehaviourReturnCode.Success;
            });

            RandomNode<long, object> sequence = new RandomNode<long, object>("RandomNode");
            sequence.AddChild(successNode);

            var status = sequence.Visit(1, null);
            Assert.Equal(BehaviourReturnCode.Success, sequence.CurrentState);
            Assert.Equal(BehaviourReturnCode.Success, status);
            Assert.True(visited);
        }

        [Fact]
        public void TestGetState()
        {
            ActionNode<long, object> successNode = new ActionNode<long, object>("SuccessNode", (t, o) =>
            {
                return BehaviourReturnCode.Success;
            });

            RandomNode<long, object> sequence = new RandomNode<long, object>("RandomNode", new System.Collections.Generic.List<BehaviourNode<long, object>>() { successNode });
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
