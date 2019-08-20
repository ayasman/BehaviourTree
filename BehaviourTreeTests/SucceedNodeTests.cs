using C4i.BehaviourTree;
using System;
using Xunit;

namespace BehaviourTreeTests
{
    public class SucceedNodeTests
    {
        [Fact]
        public void TestSuccess()
        {
            ActionNode testNode = new ActionNode("TestNode", (t, o) => BehaviourReturnCode.Success);
            SucceedNode succeed = new SucceedNode("Succeed", testNode);

            Assert.Equal(BehaviourReturnCode.Success, succeed.Visit(1, null));
            Assert.Equal(BehaviourReturnCode.Success, succeed.CurrentState);
        }

        [Fact]
        public void TestFailure()
        {
            ActionNode testNode = new ActionNode("TestNode", (t, o) => BehaviourReturnCode.Failure);
            SucceedNode succeed = new SucceedNode("Succeed", testNode);

            Assert.Equal(BehaviourReturnCode.Success, succeed.Visit(1, null));
            Assert.Equal(BehaviourReturnCode.Success, succeed.CurrentState);
        }

        [Fact]
        public void TestRunning()
        {
            ActionNode testNode = new ActionNode("TestNode", (t, o) => BehaviourReturnCode.Running);
            SucceedNode succeed = new SucceedNode("Succeed", testNode);

            Assert.Equal(BehaviourReturnCode.Running, succeed.Visit(1, null));
            Assert.Equal(BehaviourReturnCode.Running, succeed.CurrentState);
        }

        [Fact]
        public void TestError()
        {
            ActionNode testNode = new ActionNode("TestNode", (t, o) => BehaviourReturnCode.Error);
            SucceedNode succeed = new SucceedNode("Succeed", testNode);

            Assert.Equal(BehaviourReturnCode.Error, succeed.Visit(1, null));
            Assert.Equal(BehaviourReturnCode.Error, succeed.CurrentState);
        }

        [Fact]
        public void TestGetState()
        {
            ActionNode testNode = new ActionNode("TestNode", (t, o) => BehaviourReturnCode.Failure);
            SucceedNode succeed = new SucceedNode("Succeed", testNode);
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
            ActionNode testNode = new ActionNode("TestNode", (t, o) => BehaviourReturnCode.Failure);
            SucceedNode succeed = new SucceedNode("Succeed", testNode);
            succeed.Visit(1, null);

            succeed.ResetState();

            Assert.Equal(BehaviourReturnCode.Ready, testNode.CurrentState);
            Assert.Equal(BehaviourReturnCode.Ready, succeed.CurrentState);
        }
    }
}
