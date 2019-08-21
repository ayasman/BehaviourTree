using AYLib.BehaviourTree;
using System;
using Xunit;

namespace BehaviourTreeTests
{
    public class RepeatNodeTests
    {
        [Fact]
        public void TestSuccess()
        {
            int visitCount = 0;

            ActionNode successNode = new ActionNode("SuccessNode", (t, o) =>
                {
                    visitCount++;
                    return BehaviourReturnCode.Success;
                });

            RepeatNode sequence = new RepeatNode("RepeatNode", successNode, 3);

            var status = sequence.Visit(1, null);
            Assert.Equal(BehaviourReturnCode.Running, sequence.CurrentState);
            Assert.Equal(BehaviourReturnCode.Running, status);
            Assert.Equal(1, visitCount);

            status = sequence.Visit(1, null);
            Assert.Equal(BehaviourReturnCode.Running, sequence.CurrentState);
            Assert.Equal(BehaviourReturnCode.Running, status);
            Assert.Equal(2, visitCount);

            status = sequence.Visit(1, null);
            Assert.Equal(BehaviourReturnCode.Success, sequence.CurrentState);
            Assert.Equal(BehaviourReturnCode.Success, status);
            Assert.Equal(3, visitCount);
        }

        [Fact]
        public void TestFailAnReenter()
        {
            int visitCount = 0;

            ActionNode successNode = new ActionNode("SuccessNode", (t, o) =>
            {
                visitCount++;
                if (visitCount == 2)
                    return BehaviourReturnCode.Failure;
                return BehaviourReturnCode.Success;
            });

            RepeatNode sequence = new RepeatNode("RepeatNode", successNode, 3);

            var status = sequence.Visit(1, null);
            Assert.Equal(BehaviourReturnCode.Running, sequence.CurrentState);
            Assert.Equal(BehaviourReturnCode.Running, status);
            Assert.Equal(1, visitCount);

            status = sequence.Visit(1, null);
            Assert.Equal(BehaviourReturnCode.Failure, sequence.CurrentState);
            Assert.Equal(BehaviourReturnCode.Failure, status);
            Assert.Equal(2, visitCount);

            status = sequence.Visit(1, null);
            Assert.Equal(BehaviourReturnCode.Running, sequence.CurrentState);
            Assert.Equal(BehaviourReturnCode.Running, status);
            Assert.Equal(3, visitCount);
        }

        [Fact]
        public void TestError()
        {
            int visitCount = 0;

            ActionNode successNode = new ActionNode("SuccessNode", (t, o) =>
            {
                visitCount++;
                if (visitCount == 2)
                    return BehaviourReturnCode.Failure;
                return BehaviourReturnCode.Success;
            });

            RepeatNode sequence = new RepeatNode("RepeatNode", successNode, 0);

            var status = sequence.Visit(1, null);
            Assert.Equal(BehaviourReturnCode.Error, sequence.CurrentState);
            Assert.Equal(BehaviourReturnCode.Error, status);
            Assert.Equal(0, visitCount);
        }

        [Fact]
        public void TestGetState()
        {
            ActionNode successNode = new ActionNode("SuccessNode", (t, o) =>
            {
                return BehaviourReturnCode.Success;
            });

            RepeatNode sequence = new RepeatNode("RepeatNode", successNode, 3);
            var status = sequence.Visit(1, null);

            var state = sequence.GetState();

            Assert.NotNull(state);
            Assert.Equal("RepeatNode", state.NodeName);
            Assert.Equal(BehaviourReturnCode.Running, state.CurrentState);
            Assert.Empty(state.Children);
        }
    }
}
