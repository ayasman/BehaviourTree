using AYLib.BehaviourTree;
using System;
using Xunit;

namespace BehaviourTreeTests
{
    public class RaceNodeTests
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

            RaceNode<long, object> sequence = new RaceNode<long, object>("RaceNode", new System.Collections.Generic.List<BehaviourNode<long, object>>() { successNode1, successNode2, successNode3 });
            var status = sequence.Visit(1, null);

            Assert.Equal(BehaviourReturnCode.Success, sequence.CurrentState);
            Assert.Equal(BehaviourReturnCode.Success, status);
        }

        [Fact]
        public void TestOneSuccess()
        {
            ActionNode<long, object> successNode1 = new ActionNode<long, object>("SuccessNode", (t, o) =>
            {
                return BehaviourReturnCode.Success;
            });
            ActionNode<long, object> failNode1 = new ActionNode<long, object>("FailNode", (t, o) =>
            {
                return BehaviourReturnCode.Failure;
            });
            ActionNode<long, object> failNode2 = new ActionNode<long, object>("FailNode", (t, o) =>
            {
                return BehaviourReturnCode.Failure;
            });

            RaceNode<long, object> sequence = new RaceNode<long, object>("RaceNode", new System.Collections.Generic.List<BehaviourNode<long, object>>() { failNode1, successNode1, failNode2 });
            var status = sequence.Visit(1, null);

            Assert.Equal(BehaviourReturnCode.Success, sequence.CurrentState);
            Assert.Equal(BehaviourReturnCode.Success, status);
        }

        [Fact]
        public void TestNoSuccess()
        {
            ActionNode<long, object> failNode1 = new ActionNode<long, object>("FailNode", (t, o) =>
            {
                return BehaviourReturnCode.Failure;
            });
            ActionNode<long, object> failNode2 = new ActionNode<long, object>("FailNode", (t, o) =>
            {
                return BehaviourReturnCode.Failure;
            });
            ActionNode<long, object> failNode3 = new ActionNode<long, object>("FailNode", (t, o) =>
            {
                return BehaviourReturnCode.Failure;
            });

            RaceNode<long, object> sequence = new RaceNode<long, object>("RaceNode", new System.Collections.Generic.List<BehaviourNode<long, object>>() { failNode1, failNode2, failNode3 });
            var status = sequence.Visit(1, null);

            Assert.Equal(BehaviourReturnCode.Failure, sequence.CurrentState);
            Assert.Equal(BehaviourReturnCode.Failure, status);
        }

        [Fact]
        public void TestRunningFailedWithSuccess()
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

            RaceNode<long, object> sequence = new RaceNode<long, object>("RaceNode", new System.Collections.Generic.List<BehaviourNode<long, object>>() { successNode1, successNode2, runningNode });

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

            ActionNode<long, object> failNode1 = new ActionNode<long, object>("FailNode", (t, o) =>
            {
                return BehaviourReturnCode.Failure;
            });
            ActionNode<long, object> failNode2 = new ActionNode<long, object>("FailNode", (t, o) =>
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

            RaceNode<long, object> sequence = new RaceNode<long, object>("RaceNode", new System.Collections.Generic.List<BehaviourNode<long, object>>() { failNode1, failNode2, runningNode });

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

            ActionNode<long, object> failNode1 = new ActionNode<long, object>("FailNode", (t, o) =>
            {
                return BehaviourReturnCode.Failure;
            });
            ActionNode<long, object> failNode2 = new ActionNode<long, object>("FailNode", (t, o) =>
            {
                return BehaviourReturnCode.Failure;
            });
            ActionNode<long, object> runningNode = new ActionNode<long, object>("RunningNode", (t, o) =>
            {
                visitCount++;
                if (visitCount == 2)
                    return BehaviourReturnCode.Failure;
                return BehaviourReturnCode.Running;
            });

            RaceNode<long, object> sequence = new RaceNode<long, object>("RaceNode", new System.Collections.Generic.List<BehaviourNode<long, object>>() { failNode1, failNode2, runningNode });

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
            ActionNode<long, object> successNode = new ActionNode<long, object>("SuccessNode", (t, o) =>
            {
                visited = true;
                return BehaviourReturnCode.Success;
            });

            RaceNode<long, object> sequence = new RaceNode<long, object>("RaceNode");
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

            RaceNode<long, object> sequence = new RaceNode<long, object>("RaceNode", new System.Collections.Generic.List<BehaviourNode<long, object>>() { successNode1, successNode2, successNode3 });
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
