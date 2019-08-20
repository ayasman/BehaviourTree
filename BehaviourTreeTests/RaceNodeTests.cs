using C4i.BehaviourTree;
using System;
using Xunit;

namespace BehaviourTreeTests
{
    public class RaceNodeTests
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

            RaceNode sequence = new RaceNode("RaceNode", new System.Collections.Generic.List<BehaviourNode>() { successNode1, successNode2, successNode3 });
            var status = sequence.Visit(1, null);

            Assert.Equal(BehaviourReturnCode.Success, sequence.CurrentState);
            Assert.Equal(BehaviourReturnCode.Success, status);
        }

        [Fact]
        public void TestOneSuccess()
        {
            ActionNode successNode1 = new ActionNode("SuccessNode", (t, o) =>
            {
                return BehaviourReturnCode.Success;
            });
            ActionNode failNode1 = new ActionNode("FailNode", (t, o) =>
            {
                return BehaviourReturnCode.Failure;
            });
            ActionNode failNode2 = new ActionNode("FailNode", (t, o) =>
            {
                return BehaviourReturnCode.Failure;
            });

            RaceNode sequence = new RaceNode("RaceNode", new System.Collections.Generic.List<BehaviourNode>() { failNode1, successNode1, failNode2 });
            var status = sequence.Visit(1, null);

            Assert.Equal(BehaviourReturnCode.Success, sequence.CurrentState);
            Assert.Equal(BehaviourReturnCode.Success, status);
        }

        [Fact]
        public void TestNoSuccess()
        {
            ActionNode failNode1 = new ActionNode("FailNode", (t, o) =>
            {
                return BehaviourReturnCode.Failure;
            });
            ActionNode failNode2 = new ActionNode("FailNode", (t, o) =>
            {
                return BehaviourReturnCode.Failure;
            });
            ActionNode failNode3 = new ActionNode("FailNode", (t, o) =>
            {
                return BehaviourReturnCode.Failure;
            });

            RaceNode sequence = new RaceNode("RaceNode", new System.Collections.Generic.List<BehaviourNode>() { failNode1, failNode2, failNode3 });
            var status = sequence.Visit(1, null);

            Assert.Equal(BehaviourReturnCode.Failure, sequence.CurrentState);
            Assert.Equal(BehaviourReturnCode.Failure, status);
        }

        [Fact]
        public void TestRunningFailedWithSuccess()
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

            RaceNode sequence = new RaceNode("RaceNode", new System.Collections.Generic.List<BehaviourNode>() { successNode1, successNode2, runningNode });

            var status = sequence.Visit(1, null);
            Assert.Equal(BehaviourReturnCode.Success, sequence.CurrentState);
            Assert.Equal(BehaviourReturnCode.Success, status);

            status = sequence.Visit(1, null);
            Assert.Equal(BehaviourReturnCode.Success, sequence.CurrentState);
            Assert.Equal(BehaviourReturnCode.Success, status);
        }

        [Fact]
        public void TestOneRunningSuccess()
        {
            int visitCount = 0;

            ActionNode failNode1 = new ActionNode("FailNode", (t, o) =>
            {
                return BehaviourReturnCode.Failure;
            });
            ActionNode failNode2 = new ActionNode("FailNode", (t, o) =>
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

            RaceNode sequence = new RaceNode("RaceNode", new System.Collections.Generic.List<BehaviourNode>() { failNode1, failNode2, runningNode });

            var status = sequence.Visit(1, null);
            Assert.Equal(BehaviourReturnCode.Running, sequence.CurrentState);
            Assert.Equal(BehaviourReturnCode.Running, status);

            status = sequence.Visit(1, null);
            Assert.Equal(BehaviourReturnCode.Success, sequence.CurrentState);
            Assert.Equal(BehaviourReturnCode.Success, status);
        }

        [Fact]
        public void TestOneRunningFail()
        {
            int visitCount = 0;

            ActionNode failNode1 = new ActionNode("FailNode", (t, o) =>
            {
                return BehaviourReturnCode.Failure;
            });
            ActionNode failNode2 = new ActionNode("FailNode", (t, o) =>
            {
                return BehaviourReturnCode.Failure;
            });
            ActionNode runningNode = new ActionNode("RunningNode", (t, o) =>
            {
                visitCount++;
                if (visitCount == 2)
                    return BehaviourReturnCode.Failure;
                return BehaviourReturnCode.Running;
            });

            RaceNode sequence = new RaceNode("RaceNode", new System.Collections.Generic.List<BehaviourNode>() { failNode1, failNode2, runningNode });

            var status = sequence.Visit(1, null);
            Assert.Equal(BehaviourReturnCode.Running, sequence.CurrentState);
            Assert.Equal(BehaviourReturnCode.Running, status);

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

            RaceNode sequence = new RaceNode("RaceNode");
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

            RaceNode sequence = new RaceNode("RaceNode", new System.Collections.Generic.List<BehaviourNode>() { successNode1, successNode2, successNode3 });
            var status = sequence.Visit(1, null);

            var state = sequence.GetState();

            Assert.NotNull(state);
            Assert.Equal("RaceNode", state.NodeName);
            Assert.Equal(BehaviourReturnCode.Success, state.CurrentState);
            Assert.NotEmpty(state.Children);
            Assert.Equal(3, state.Children.Count);
        }
    }
}
