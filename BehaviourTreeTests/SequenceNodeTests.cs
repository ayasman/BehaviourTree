using AYLib.BehaviourTree;
using System;
using Xunit;

namespace BehaviourTreeTests
{
    public class SequenceNodeTests
    {
        [Fact]
        public void TestFirstFail()
        {
            int lastvisited = 0;

            ActionNode failNode = new ActionNode("FailNode", (t, o) =>
                {
                    lastvisited = 1;
                    return BehaviourReturnCode.Failure;
                });
            ActionNode successNode1 = new ActionNode("SuccessNode", (t, o) =>
                {
                    lastvisited = 2;
                    return BehaviourReturnCode.Success;
                });
            ActionNode successNode2 = new ActionNode("SuccessNode", (t, o) =>
                {
                    lastvisited = 3;
                    return BehaviourReturnCode.Success;
                });

            SequenceNode sequence = new SequenceNode("SequenceNode", new System.Collections.Generic.List<BehaviourNode>() { failNode, successNode1, successNode2 });
            var status = sequence.Visit(1, null);

            Assert.Equal(BehaviourReturnCode.Failure, sequence.CurrentState);
            Assert.Equal(BehaviourReturnCode.Failure, status);
            Assert.Equal(1, lastvisited);
        }

        [Fact]
        public void TestLastFail()
        {
            int lastvisited = 0;

            ActionNode successNode1 = new ActionNode("SuccessNode", (t, o) =>
            {
                lastvisited = 1;
                return BehaviourReturnCode.Success;
            });
            ActionNode successNode2 = new ActionNode("SuccessNode", (t, o) =>
            {
                lastvisited = 2;
                return BehaviourReturnCode.Success;
            });
            ActionNode failNode = new ActionNode("FailNode", (t, o) =>
            {
                lastvisited = 3;
                return BehaviourReturnCode.Failure;
            });

            SequenceNode sequence = new SequenceNode("SequenceNode", new System.Collections.Generic.List<BehaviourNode>() { successNode1, successNode2, failNode });
            var status = sequence.Visit(1, null);

            Assert.Equal(BehaviourReturnCode.Failure, sequence.CurrentState);
            Assert.Equal(BehaviourReturnCode.Failure, status);
            Assert.Equal(3, lastvisited);
        }

        [Fact]
        public void TestAllSuccess()
        {
            int lastvisited = 0;

            ActionNode successNode1 = new ActionNode("SuccessNode", (t, o) =>
            {
                lastvisited = 1;
                return BehaviourReturnCode.Success;
            });
            ActionNode successNode2 = new ActionNode("SuccessNode", (t, o) =>
            {
                lastvisited = 2;
                return BehaviourReturnCode.Success;
            });

            SequenceNode sequence = new SequenceNode("SequenceNode", new System.Collections.Generic.List<BehaviourNode>() { successNode1, successNode2 });
            var status = sequence.Visit(1, null);

            Assert.Equal(BehaviourReturnCode.Success, sequence.CurrentState);
            Assert.Equal(BehaviourReturnCode.Success, status);
            Assert.Equal(2, lastvisited);
        }

        [Fact]
        public void TestError()
        {
            int lastvisited = 0;

            ActionNode successNode1 = new ActionNode("SuccessNode", (t, o) =>
            {
                lastvisited = 1;
                return BehaviourReturnCode.Success;
            });
            ActionNode errorNode = new ActionNode("ErrorNode", (t, o) =>
            {
                lastvisited = 2;
                return BehaviourReturnCode.Error;
            });
            ActionNode successNode2 = new ActionNode("SuccessNode", (t, o) =>
            {
                lastvisited = 3;
                return BehaviourReturnCode.Success;
            });

            SequenceNode sequence = new SequenceNode("SequenceNode", new System.Collections.Generic.List<BehaviourNode>() { successNode1, errorNode, successNode2 });
            var status = sequence.Visit(1, null);

            Assert.Equal(BehaviourReturnCode.Error, sequence.CurrentState);
            Assert.Equal(BehaviourReturnCode.Error, status);
            Assert.Equal(2, lastvisited);
        }

        [Fact]
        public void TestRunning()
        {
            int lastvisited = 0;

            ActionNode successNode1 = new ActionNode("SuccessNode", (t, o) =>
            {
                lastvisited = 1;
                return BehaviourReturnCode.Success;
            });
            ActionNode runningNode = new ActionNode("RunningNode", (t, o) =>
            {
                lastvisited = 2;
                return BehaviourReturnCode.Running;
            });
            ActionNode successNode2 = new ActionNode("SuccessNode", (t, o) =>
            {
                lastvisited = 3;
                return BehaviourReturnCode.Success;
            });

            SequenceNode sequence = new SequenceNode("SequenceNode", new System.Collections.Generic.List<BehaviourNode>() { successNode1, runningNode, successNode2 });
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

            ActionNode successNode1 = new ActionNode("SuccessNode", (t, o) =>
            {
                lastvisited = 1;
                return BehaviourReturnCode.Success;
            });
            ActionNode runningNode = new ActionNode("RunningNode", (t, o) =>
            {
                lastvisited = 2;
                runCount--;
                if (runCount == 0)
                    return BehaviourReturnCode.Success;
                return BehaviourReturnCode.Running;
            });
            ActionNode successNode2 = new ActionNode("SuccessNode", (t, o) =>
            {
                lastvisited = 3;
                return BehaviourReturnCode.Success;
            });

            SequenceNode sequence = new SequenceNode("SequenceNode", new System.Collections.Generic.List<BehaviourNode>() { successNode1, runningNode, successNode2 });

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
            Assert.Equal(3, lastvisited);
            Assert.Equal(0, runCount);
        }

        [Fact]
        public void TestRunningReentrancyWithFail()
        {
            int lastvisited = 0;
            int runCount = 3;

            ActionNode successNode1 = new ActionNode("SuccessNode", (t, o) =>
            {
                lastvisited = 1;
                return BehaviourReturnCode.Success;
            });
            ActionNode runningNode = new ActionNode("RunningNode", (t, o) =>
            {
                lastvisited = 2;
                runCount--;
                if (runCount == 0)
                    return BehaviourReturnCode.Failure;
                return BehaviourReturnCode.Running;
            });
            ActionNode successNode2 = new ActionNode("SuccessNode", (t, o) =>
            {
                lastvisited = 3;
                return BehaviourReturnCode.Success;
            });

            SequenceNode sequence = new SequenceNode("SequenceNode", new System.Collections.Generic.List<BehaviourNode>() { successNode1, runningNode, successNode2 });

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
            Assert.Equal(2, lastvisited);
            Assert.Equal(0, runCount);
        }

        [Fact]
        public void TestRunningReentrancyLoopbackToStart()
        {
            int lastvisited = 0;
            int runCount = 3;

            ActionNode successNode1 = new ActionNode("SuccessNode", (t, o) =>
            {
                lastvisited = 1;
                if (runCount == 0)
                    return BehaviourReturnCode.Failure;
                return BehaviourReturnCode.Success;
            });
            ActionNode runningNode = new ActionNode("RunningNode", (t, o) =>
            {
                lastvisited = 2;
                runCount--;
                if (runCount == 0)
                    return BehaviourReturnCode.Success;
                return BehaviourReturnCode.Running;
            });
            ActionNode successNode2 = new ActionNode("SuccessNode", (t, o) =>
            {
                lastvisited = 3;
                return BehaviourReturnCode.Success;
            });

            SequenceNode sequence = new SequenceNode("SequenceNode", new System.Collections.Generic.List<BehaviourNode>() { successNode1, runningNode, successNode2 });

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
            Assert.Equal(3, lastvisited);
            Assert.Equal(0, runCount);

            status = sequence.Visit(1, null);
            Assert.Equal(BehaviourReturnCode.Failure, sequence.CurrentState);
            Assert.Equal(BehaviourReturnCode.Failure, status);
            Assert.Equal(1, lastvisited);
            Assert.Equal(0, runCount);
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

            SequenceNode sequence = new SequenceNode("SequenceNode");
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

            SequenceNode sequence = new SequenceNode("SequenceNode", new System.Collections.Generic.List<BehaviourNode>() { successNode1, successNode2 });
            sequence.Visit(1, null);

            var state = sequence.GetState();

            Assert.NotNull(state);
            Assert.Equal("SequenceNode", state.NodeName);
            Assert.Equal(BehaviourReturnCode.Success, state.CurrentState);
            Assert.NotEmpty(state.Children);
            Assert.Equal(2, state.Children.Count);
        }
    }
}
