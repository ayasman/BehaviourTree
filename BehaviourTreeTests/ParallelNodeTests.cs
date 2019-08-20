using C4i.BehaviourTree;
using System;
using Xunit;

namespace BehaviourTreeTests
{
    public class ParallelNodeTests
    {
        [Fact]
        public void TestAllSuccess()
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

            ParallelNode sequence = new ParallelNode("ParallelNode", new System.Collections.Generic.List<BehaviourNode>() { successNode1, successNode2, successNode3 });
            var status = sequence.Visit(1, null);

            Assert.Equal(BehaviourReturnCode.Success, sequence.CurrentState);
            Assert.Equal(BehaviourReturnCode.Success, status);
        }

        [Fact]
        public void TestOneRunning()
        {
            ActionNode successNode1 = new ActionNode("SuccessNode", (t, o) =>
            {
                return BehaviourReturnCode.Success;
            });
            ActionNode successNode2 = new ActionNode("SuccessNode", (t, o) =>
            {
                return BehaviourReturnCode.Success;
            });
            ActionNode runningNode = new ActionNode("RunningNode", (t, o) =>
            {
                return BehaviourReturnCode.Running;
            });

            ParallelNode sequence = new ParallelNode("ParallelNode", new System.Collections.Generic.List<BehaviourNode>() { successNode1, successNode2, runningNode });
            var status = sequence.Visit(1, null);

            Assert.Equal(BehaviourReturnCode.Running, sequence.CurrentState);
            Assert.Equal(BehaviourReturnCode.Running, status);
        }

        [Fact]
        public void TestOneRunningReentrance()
        {
            int visitCount = 0;

            ActionNode successNode1 = new ActionNode("SuccessNode", (t, o) =>
            {
                return BehaviourReturnCode.Success;
            });
            ActionNode successNode2 = new ActionNode("SuccessNode", (t, o) =>
            {
                return BehaviourReturnCode.Success;
            });
            ActionNode runningNode = new ActionNode("RunningNode", (t, o) =>
            {
                visitCount++;
                if (visitCount == 2)
                    return BehaviourReturnCode.Success;
                return BehaviourReturnCode.Running;
            });

            ParallelNode sequence = new ParallelNode("ParallelNode", new System.Collections.Generic.List<BehaviourNode>() { successNode1, successNode2, runningNode });

            var status = sequence.Visit(1, null);
            Assert.Equal(BehaviourReturnCode.Running, sequence.CurrentState);
            Assert.Equal(BehaviourReturnCode.Running, status);

            status = sequence.Visit(1, null);
            Assert.Equal(BehaviourReturnCode.Success, sequence.CurrentState);
            Assert.Equal(BehaviourReturnCode.Success, status);
        }

        [Fact]
        public void TestOneRunningFailed()
        {
            int visitCount = 0;

            ActionNode successNode1 = new ActionNode("SuccessNode", (t, o) =>
            {
                return BehaviourReturnCode.Success;
            });
            ActionNode successNode2 = new ActionNode("SuccessNode", (t, o) =>
            {
                return BehaviourReturnCode.Success;
            });
            ActionNode runningNode = new ActionNode("RunningNode", (t, o) =>
            {
                visitCount++;
                if (visitCount == 2)
                    return BehaviourReturnCode.Failure;
                return BehaviourReturnCode.Running;
            });

            ParallelNode sequence = new ParallelNode("ParallelNode", new System.Collections.Generic.List<BehaviourNode>() { successNode1, successNode2, runningNode });

            var status = sequence.Visit(1, null);
            Assert.Equal(BehaviourReturnCode.Running, sequence.CurrentState);
            Assert.Equal(BehaviourReturnCode.Running, status);

            status = sequence.Visit(1, null);
            Assert.Equal(BehaviourReturnCode.Failure, sequence.CurrentState);
            Assert.Equal(BehaviourReturnCode.Failure, status);
        }

        [Fact]
        public void TestFailure()
        {
            ActionNode successNode1 = new ActionNode("SuccessNode", (t, o) =>
            {
                return BehaviourReturnCode.Success;
            });
            ActionNode failNode = new ActionNode("FailNode", (t, o) =>
            {
                return BehaviourReturnCode.Failure;
            });
            ActionNode successNode3 = new ActionNode("SuccessNode", (t, o) =>
            {

                return BehaviourReturnCode.Success;
            });

            ParallelNode sequence = new ParallelNode("ParallelNode", new System.Collections.Generic.List<BehaviourNode>() { successNode1, failNode, successNode3 });
            var status = sequence.Visit(1, null);

            Assert.Equal(BehaviourReturnCode.Failure, sequence.CurrentState);
            Assert.Equal(BehaviourReturnCode.Failure, status);
        }

        [Fact]
        public void TestFailureWithRunning()
        {
            int visitCount = 0;
            ActionNode successNode1 = new ActionNode("SuccessNode", (t, o) =>
            {
                return BehaviourReturnCode.Success;
            });
            ActionNode failNode = new ActionNode("FailNode", (t, o) =>
            {
                return BehaviourReturnCode.Failure;
            });
            ActionNode runningNode = new ActionNode("RunningNode", (t, o) =>
            {
                visitCount++;
                if (visitCount == 2)
                    return BehaviourReturnCode.Success;
                return BehaviourReturnCode.Running;
            });

            ParallelNode sequence = new ParallelNode("ParallelNode", new System.Collections.Generic.List<BehaviourNode>() { successNode1, failNode, runningNode });

            var status = sequence.Visit(1, null);
            Assert.Equal(BehaviourReturnCode.Failure, sequence.CurrentState);
            Assert.Equal(BehaviourReturnCode.Failure, status);

            status = sequence.Visit(1, null);
            Assert.Equal(BehaviourReturnCode.Failure, sequence.CurrentState);
            Assert.Equal(BehaviourReturnCode.Failure, status);
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

            ParallelNode sequence = new ParallelNode("ParallelNode");
            sequence.AddChild(successNode);

            var status = sequence.Visit(1, null);
            Assert.Equal(BehaviourReturnCode.Success, sequence.CurrentState);
            Assert.Equal(BehaviourReturnCode.Success, status);
            Assert.True(visited);
        }

        [Fact]
        public void TestGetState()
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

            ParallelNode sequence = new ParallelNode("ParallelNode", new System.Collections.Generic.List<BehaviourNode>() { successNode1, successNode2, successNode3 });
            var status = sequence.Visit(1, null);

            var state = sequence.GetState();

            Assert.NotNull(state);
            Assert.Equal("ParallelNode", state.NodeName);
            Assert.Equal(BehaviourReturnCode.Success, state.CurrentState);
            Assert.NotEmpty(state.Children);
            Assert.Equal(3, state.Children.Count);
        }
    }
}
