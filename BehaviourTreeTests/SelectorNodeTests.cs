using AYLib.BehaviourTree;
using System;
using Xunit;

namespace BehaviourTreeTests
{
    public class SelectorNodeTests
    {
        [Fact]
        public void TestFirstSuccess()
        {
            int lastvisited = 0;

            ActionNode<long, object> successNode = new ActionNode<long, object>("SuccessNode", (t, o) =>
                {
                    lastvisited = 1;
                    return BehaviourReturnCode.Success;
                });
            ActionNode<long, object> failNode1 = new ActionNode<long, object>("FailureNode", (t, o) =>
                {
                    lastvisited = 2;
                    return BehaviourReturnCode.Failure;
                });
            ActionNode<long, object> failNode2 = new ActionNode<long, object>("FailureNode", (t, o) =>
                {
                    lastvisited = 3;
                    return BehaviourReturnCode.Failure;
                });

            SelectorNode<long, object> sequence = new SelectorNode<long, object>("SelectorNode", new System.Collections.Generic.List<BehaviourNode<long, object>>() { successNode, failNode1, failNode2 });
            var status = sequence.Visit(1, null);

            Assert.Equal(BehaviourReturnCode.Success, sequence.CurrentState);
            Assert.Equal(BehaviourReturnCode.Success, status);
            Assert.Equal(1, lastvisited);
        }

        [Fact]
        public void TestLastSuccess()
        {
            int lastvisited = 0;

            ActionNode<long, object> failNode1 = new ActionNode<long, object>("FailureNode", (t, o) =>
            {
                lastvisited = 1;
                return BehaviourReturnCode.Failure;
            });
            ActionNode<long, object> failNode2 = new ActionNode<long, object>("FailureNode", (t, o) =>
            {
                lastvisited = 2;
                return BehaviourReturnCode.Failure;
            });
            ActionNode<long, object> successNode = new ActionNode<long, object>("SuccessNode", (t, o) =>
            {
                lastvisited = 3;
                return BehaviourReturnCode.Success;
            });

            SelectorNode<long, object> sequence = new SelectorNode<long, object>("SelectorNode", new System.Collections.Generic.List<BehaviourNode<long, object>>() { failNode1, failNode2, successNode });
            var status = sequence.Visit(1, null);

            Assert.Equal(BehaviourReturnCode.Success, sequence.CurrentState);
            Assert.Equal(BehaviourReturnCode.Success, status);
            Assert.Equal(3, lastvisited);
        }

        [Fact]
        public void TestNoSuccess()
        {
            int lastvisited = 0;

            ActionNode<long, object> failNode1 = new ActionNode<long, object>("FailureNode", (t, o) =>
            {
                lastvisited = 1;
                return BehaviourReturnCode.Failure;
            });
            ActionNode<long, object> failNode2 = new ActionNode<long, object>("FailureNode", (t, o) =>
            {
                lastvisited = 2;
                return BehaviourReturnCode.Failure;
            });

            SelectorNode<long, object> sequence = new SelectorNode<long, object>("SelectorNode", new System.Collections.Generic.List<BehaviourNode<long, object>>() { failNode1, failNode2 });
            var status = sequence.Visit(1, null);

            Assert.Equal(BehaviourReturnCode.Failure, sequence.CurrentState);
            Assert.Equal(BehaviourReturnCode.Failure, status);
            Assert.Equal(2, lastvisited);
        }

        [Fact]
        public void TestError()
        {
            int lastvisited = 0;

            ActionNode<long, object> failNode1 = new ActionNode<long, object>("FailureNode", (t, o) =>
            {
                lastvisited = 1;
                return BehaviourReturnCode.Failure;
            });
            ActionNode<long, object> errorNode = new ActionNode<long, object>("ErrorNode", (t, o) =>
            {
                lastvisited = 2;
                return BehaviourReturnCode.Error;
            });
            ActionNode<long, object> successNode = new ActionNode<long, object>("SuccessNode", (t, o) =>
            {
                lastvisited = 3;
                return BehaviourReturnCode.Success;
            });

            SelectorNode<long, object> sequence = new SelectorNode<long, object>("SelectorNode", new System.Collections.Generic.List<BehaviourNode<long, object>>() { failNode1, errorNode, successNode });
            var status = sequence.Visit(1, null);

            Assert.Equal(BehaviourReturnCode.Error, sequence.CurrentState);
            Assert.Equal(BehaviourReturnCode.Error, status);
            Assert.Equal(2, lastvisited);
        }

        [Fact]
        public void TestRunning()
        {
            int lastvisited = 0;

            ActionNode<long, object> failNode1 = new ActionNode<long, object>("FailureNode", (t, o) =>
            {
                lastvisited = 1;
                return BehaviourReturnCode.Failure;
            });
            ActionNode<long, object> runningNode = new ActionNode<long, object>("RunningNode", (t, o) =>
            {
                lastvisited = 2;
                return BehaviourReturnCode.Running;
            });
            ActionNode<long, object> failNode2 = new ActionNode<long, object>("FailureNode", (t, o) =>
            {
                lastvisited = 3;
                return BehaviourReturnCode.Failure;
            });

            SelectorNode<long, object> sequence = new SelectorNode<long, object>("SelectorNode", new System.Collections.Generic.List<BehaviourNode<long, object>>() { failNode1, runningNode, failNode2 });
            var status = sequence.Visit(1, null);

            Assert.Equal(BehaviourReturnCode.Running, sequence.CurrentState);
            Assert.Equal(BehaviourReturnCode.Running, status);
            Assert.Equal(2, lastvisited);
        }

        [Fact]
        public void TestRunningReentrancySuccess()
        {
            int lastvisited = 0;
            int runCount = 3;

            ActionNode<long, object> failNode1 = new ActionNode<long, object>("FailureNode", (t, o) =>
            {
                lastvisited = 1;
                return BehaviourReturnCode.Failure;
            });
            ActionNode<long, object> runningNode = new ActionNode<long, object>("RunningNode", (t, o) =>
            {
                lastvisited = 2;
                runCount--;
                if (runCount == 0)
                    return BehaviourReturnCode.Success;
                return BehaviourReturnCode.Running;
            });
            ActionNode<long, object> failNode2 = new ActionNode<long, object>("FailureNode", (t, o) =>
            {
                lastvisited = 3;
                return BehaviourReturnCode.Failure;
            });

            SelectorNode<long, object> sequence = new SelectorNode<long, object>("SelectorNode", new System.Collections.Generic.List<BehaviourNode<long, object>>() { failNode1, runningNode, failNode2 });

            var status = sequence.Visit(1, null);
            Assert.Equal(BehaviourReturnCode.Running, sequence.CurrentState);
            Assert.Equal(BehaviourReturnCode.Running, status);
            Assert.Equal(2, lastvisited);
            Assert.Equal(2, runCount);

            status = sequence.Visit(1, null);
            Assert.Equal(BehaviourReturnCode.Running, sequence.CurrentState);
            Assert.Equal(BehaviourReturnCode.Running, status);
            Assert.Equal(2, lastvisited);
            Assert.Equal(1, runCount);

            status = sequence.Visit(1, null);
            Assert.Equal(BehaviourReturnCode.Success, sequence.CurrentState);
            Assert.Equal(BehaviourReturnCode.Success, status);
            Assert.Equal(2, lastvisited);
            Assert.Equal(0, runCount);
        }

        [Fact]
        public void TestRunningReentrancyWithFail()
        {
            int lastvisited = 0;
            int runCount = 3;

            ActionNode<long, object> failNode1 = new ActionNode<long, object>("FailureNode", (t, o) =>
            {
                lastvisited = 1;
                return BehaviourReturnCode.Failure;
            });
            ActionNode<long, object> runningNode = new ActionNode<long, object>("RunningNode", (t, o) =>
            {
                lastvisited = 2;
                runCount--;
                if (runCount == 0)
                    return BehaviourReturnCode.Failure;
                return BehaviourReturnCode.Running;
            });
            ActionNode<long, object> failNode2 = new ActionNode<long, object>("FailureNode", (t, o) =>
            {
                lastvisited = 3;
                return BehaviourReturnCode.Failure;
            });

            SelectorNode<long, object> sequence = new SelectorNode<long, object>("SelectorNode", new System.Collections.Generic.List<BehaviourNode<long, object>>() { failNode1, runningNode, failNode2 });

            var status = sequence.Visit(1, null);
            Assert.Equal(BehaviourReturnCode.Running, sequence.CurrentState);
            Assert.Equal(BehaviourReturnCode.Running, status);
            Assert.Equal(2, lastvisited);
            Assert.Equal(2, runCount);

            status = sequence.Visit(1, null);
            Assert.Equal(BehaviourReturnCode.Running, sequence.CurrentState);
            Assert.Equal(BehaviourReturnCode.Running, status);
            Assert.Equal(2, lastvisited);
            Assert.Equal(1, runCount);

            status = sequence.Visit(1, null);
            Assert.Equal(BehaviourReturnCode.Failure, sequence.CurrentState);
            Assert.Equal(BehaviourReturnCode.Failure, status);
            Assert.Equal(3, lastvisited);
            Assert.Equal(0, runCount);
        }

        [Fact]
        public void TestRunningReentrancyLoopbackToStart()
        {
            int lastvisited = 0;
            int runCount = 3;

            ActionNode<long, object> failNode1 = new ActionNode<long, object>("SwitchNode", (t, o) =>
            {
                lastvisited = 1;
                if (runCount==0)
                    return BehaviourReturnCode.Success;
                return BehaviourReturnCode.Failure;
            });
            ActionNode<long, object> runningNode = new ActionNode<long, object>("RunningNode", (t, o) =>
            {
                lastvisited = 2;
                runCount--;
                if (runCount == 0)
                    return BehaviourReturnCode.Failure;
                return BehaviourReturnCode.Running;
            });
            ActionNode<long, object> failNode2 = new ActionNode<long, object>("FailureNode", (t, o) =>
            {
                lastvisited = 3;
                return BehaviourReturnCode.Failure;
            });

            SelectorNode<long, object> sequence = new SelectorNode<long, object>("SelectorNode", new System.Collections.Generic.List<BehaviourNode<long, object>>() { failNode1, runningNode, failNode2 });

            var status = sequence.Visit(1, null);
            Assert.Equal(BehaviourReturnCode.Running, sequence.CurrentState);
            Assert.Equal(BehaviourReturnCode.Running, status);
            Assert.Equal(2, lastvisited);
            Assert.Equal(2, runCount);

            status = sequence.Visit(1, null);
            Assert.Equal(BehaviourReturnCode.Running, sequence.CurrentState);
            Assert.Equal(BehaviourReturnCode.Running, status);
            Assert.Equal(2, lastvisited);
            Assert.Equal(1, runCount);

            status = sequence.Visit(1, null);
            Assert.Equal(BehaviourReturnCode.Failure, sequence.CurrentState);
            Assert.Equal(BehaviourReturnCode.Failure, status);
            Assert.Equal(3, lastvisited);
            Assert.Equal(0, runCount);

            status = sequence.Visit(1, null);
            Assert.Equal(BehaviourReturnCode.Success, sequence.CurrentState);
            Assert.Equal(BehaviourReturnCode.Success, status);
            Assert.Equal(1, lastvisited);
            Assert.Equal(0, runCount);
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

            SelectorNode<long, object> sequence = new SelectorNode<long, object>("RandomNode");
            sequence.AddChild(successNode);

            var status = sequence.Visit(1, null);
            Assert.Equal(BehaviourReturnCode.Success, sequence.CurrentState);
            Assert.Equal(BehaviourReturnCode.Success, status);
            Assert.True(visited);
        }

        [Fact]
        public void TestGetState()
        {
            ActionNode<long, object> failNode1 = new ActionNode<long, object>("FailureNode", (t, o) =>
            {
                return BehaviourReturnCode.Failure;
            });
            ActionNode<long, object> failNode2 = new ActionNode<long, object>("FailureNode", (t, o) =>
            {
                return BehaviourReturnCode.Failure;
            });

            SelectorNode<long, object> sequence = new SelectorNode<long, object>("SelectorNode", new System.Collections.Generic.List<BehaviourNode<long, object>>() { failNode1, failNode2 });
            var status = sequence.Visit(1, null);

            var state = sequence.GetState();

            Assert.NotNull(state);
            Assert.Equal("SelectorNode", state.NodeName);
            Assert.Equal(BehaviourReturnCode.Failure, state.CurrentState);
            Assert.NotEmpty(state.Children);
            Assert.Equal(2, state.Children.Count);
        }
    }
}
