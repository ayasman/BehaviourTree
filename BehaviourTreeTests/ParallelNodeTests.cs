using AYLib.BehaviourTree;
using System;
using Xunit;

namespace BehaviourTreeTests
{
    public class ParallelNodeTests
    {
        [Fact]
        public void TestAllSuccess()
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

            ParallelNode<long, object> sequence = new ParallelNode<long, object>("ParallelNode", new System.Collections.Generic.List<BehaviourNode<long, object>>() { successNode1, successNode2, successNode3 });
            var status = sequence.Visit(1, null);

            Assert.Equal(BehaviourReturnCode.Success, sequence.CurrentState);
            Assert.Equal(BehaviourReturnCode.Success, status);
        }

        [Fact]
        public void TestOneRunning()
        {
            ActionNode<long, object> successNode1 = new ActionNode<long, object>("SuccessNode", (t, o) =>
            {
                return BehaviourReturnCode.Success;
            });
            ActionNode<long, object> successNode2 = new ActionNode<long, object>("SuccessNode", (t, o) =>
            {
                return BehaviourReturnCode.Success;
            });
            ActionNode<long, object> runningNode = new ActionNode<long, object>("RunningNode", (t, o) =>
            {
                return BehaviourReturnCode.Running;
            });

            ParallelNode<long, object> sequence = new ParallelNode<long, object>("ParallelNode", new System.Collections.Generic.List<BehaviourNode<long, object>>() { successNode1, successNode2, runningNode });
            var status = sequence.Visit(1, null);

            Assert.Equal(BehaviourReturnCode.Running, sequence.CurrentState);
            Assert.Equal(BehaviourReturnCode.Running, status);
        }

        [Fact]
        public void TestOneRunningReentrance()
        {
            int visitCount = 0;

            ActionNode<long, object> successNode1 = new ActionNode<long, object>("SuccessNode", (t, o) =>
            {
                return BehaviourReturnCode.Success;
            });
            ActionNode<long, object> successNode2 = new ActionNode<long, object>("SuccessNode", (t, o) =>
            {
                return BehaviourReturnCode.Success;
            });
            ActionNode<long, object> runningNode = new ActionNode<long, object>("RunningNode", (t, o) =>
            {
                visitCount++;
                if (visitCount == 2)
                    return BehaviourReturnCode.Success;
                return BehaviourReturnCode.Running;
            });

            ParallelNode<long, object> sequence = new ParallelNode<long, object>("ParallelNode", new System.Collections.Generic.List<BehaviourNode<long, object>>() { successNode1, successNode2, runningNode });

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

            ActionNode<long, object> successNode1 = new ActionNode<long, object>("SuccessNode", (t, o) =>
            {
                return BehaviourReturnCode.Success;
            });
            ActionNode<long, object> successNode2 = new ActionNode<long, object>("SuccessNode", (t, o) =>
            {
                return BehaviourReturnCode.Success;
            });
            ActionNode<long, object> runningNode = new ActionNode<long, object>("RunningNode", (t, o) =>
            {
                visitCount++;
                if (visitCount == 2)
                    return BehaviourReturnCode.Failure;
                return BehaviourReturnCode.Running;
            });

            ParallelNode<long, object> sequence = new ParallelNode<long, object>("ParallelNode", new System.Collections.Generic.List<BehaviourNode<long, object>>() { successNode1, successNode2, runningNode });

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
            ActionNode<long, object> successNode1 = new ActionNode<long, object>("SuccessNode", (t, o) =>
            {
                return BehaviourReturnCode.Success;
            });
            ActionNode<long, object> failNode = new ActionNode<long, object>("FailNode", (t, o) =>
            {
                return BehaviourReturnCode.Failure;
            });
            ActionNode<long, object> successNode3 = new ActionNode<long, object>("SuccessNode", (t, o) =>
            {

                return BehaviourReturnCode.Success;
            });

            ParallelNode<long, object> sequence = new ParallelNode<long, object>("ParallelNode", new System.Collections.Generic.List<BehaviourNode<long, object>>() { successNode1, failNode, successNode3 });
            var status = sequence.Visit(1, null);

            Assert.Equal(BehaviourReturnCode.Failure, sequence.CurrentState);
            Assert.Equal(BehaviourReturnCode.Failure, status);
        }

        [Fact]
        public void TestFailureWithRunning()
        {
            int visitCount = 0;
            ActionNode<long, object> successNode1 = new ActionNode<long, object>("SuccessNode", (t, o) =>
            {
                return BehaviourReturnCode.Success;
            });
            ActionNode<long, object> failNode = new ActionNode<long, object>("FailNode", (t, o) =>
            {
                return BehaviourReturnCode.Failure;
            });
            ActionNode<long, object> runningNode = new ActionNode<long, object>("RunningNode", (t, o) =>
            {
                visitCount++;
                if (visitCount == 2)
                    return BehaviourReturnCode.Success;
                return BehaviourReturnCode.Running;
            });

            ParallelNode<long, object> sequence = new ParallelNode<long, object>("ParallelNode", new System.Collections.Generic.List<BehaviourNode<long, object>>() { successNode1, failNode, runningNode });

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
            ActionNode<long, object> successNode = new ActionNode<long, object>("SuccessNode", (t, o) =>
            {
                visited = true;
                return BehaviourReturnCode.Success;
            });

            ParallelNode<long, object> sequence = new ParallelNode<long, object>("ParallelNode");
            sequence.AddChild(successNode);

            var status = sequence.Visit(1, null);
            Assert.Equal(BehaviourReturnCode.Success, sequence.CurrentState);
            Assert.Equal(BehaviourReturnCode.Success, status);
            Assert.True(visited);
        }

        [Fact]
        public void TestGetState()
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

            ParallelNode<long, object> sequence = new ParallelNode<long, object>("ParallelNode", new System.Collections.Generic.List<BehaviourNode<long, object>>() { successNode1, successNode2, successNode3 });
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
