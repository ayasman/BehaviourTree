using AYLib.BehaviourTree;
using System;
using Xunit;

namespace BehaviourTreeTests
{
    public class SucceedNodeTests
    {
        [Fact]
        public void TestSuccess()
        {
            ActionNode<long, object> testNode = new ActionNode<long, object>("TestNode", (t, o) => BehaviourReturnCode.Success);
            SucceedNode<long, object> succeed = new SucceedNode<long, object>("Succeed", testNode);

            Assert.Equal(BehaviourReturnCode.Success, succeed.Visit(1, null));
            Assert.Equal(BehaviourReturnCode.Success, succeed.CurrentState);
        }

        [Fact]
        public void TestFailure()
        {
            ActionNode<long, object> testNode = new ActionNode<long, object>("TestNode", (t, o) => BehaviourReturnCode.Failure);
            SucceedNode<long, object> succeed = new SucceedNode<long, object>("Succeed", testNode);

            Assert.Equal(BehaviourReturnCode.Success, succeed.Visit(1, null));
            Assert.Equal(BehaviourReturnCode.Success, succeed.CurrentState);
        }

        [Fact]
        public void TestRunning()
        {
            ActionNode<long, object> testNode = new ActionNode<long, object>("TestNode", (t, o) => BehaviourReturnCode.Running);
            SucceedNode<long, object> succeed = new SucceedNode<long, object>("Succeed", testNode);

            Assert.Equal(BehaviourReturnCode.Running, succeed.Visit(1, null));
            Assert.Equal(BehaviourReturnCode.Running, succeed.CurrentState);
        }

        [Fact]
        public void TestError()
        {
            ActionNode<long, object> testNode = new ActionNode<long, object>("TestNode", (t, o) => BehaviourReturnCode.Error);
            SucceedNode<long, object> succeed = new SucceedNode<long, object>("Succeed", testNode);

            Assert.Equal(BehaviourReturnCode.Error, succeed.Visit(1, null));
            Assert.Equal(BehaviourReturnCode.Error, succeed.CurrentState);
        }

        [Fact]
        public void TestGetState()
        {
            ActionNode<long, object> testNode = new ActionNode<long, object>("TestNode", (t, o) => BehaviourReturnCode.Failure);
            SucceedNode<long, object> succeed = new SucceedNode<long, object>("Succeed", testNode);
            succeed.Visit(1, null);

            var state = succeed.GetState();

            Assert.NotNull(state);
            Assert.Equal("Succeed", state.NodeName);
            Assert.Equal(BehaviourReturnCode.Success, state.CurrentState);
            Assert.Single(state.Children);
        }

        [Fact]
        public void TestStateReset()
        {
            ActionNode<long, object> testNode = new ActionNode<long, object>("TestNode", (t, o) => BehaviourReturnCode.Failure);
            SucceedNode<long, object> succeed = new SucceedNode<long, object>("Succeed", testNode);
            succeed.Visit(1, null);

            succeed.ResetState();

            Assert.Equal(BehaviourReturnCode.Ready, testNode.CurrentState);
            Assert.Equal(BehaviourReturnCode.Ready, succeed.CurrentState);
        }
    }
}
