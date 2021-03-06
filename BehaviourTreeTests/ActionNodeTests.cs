using AYLib.BehaviourTree;
using System;
using Xunit;

namespace BehaviourTreeTests
{
    public class ActionNodeTests
    {
        [Fact]
        public void TestSuccess()
        {
            ActionNode<long, object> testNode = new ActionNode<long, object>("TestNode", (t, o) => BehaviourReturnCode.Success);
            Assert.Equal(BehaviourReturnCode.Success, testNode.Visit(1, null));
            Assert.Equal(BehaviourReturnCode.Success, testNode.CurrentState);
        }

        [Fact]
        public void TestFailure()
        {
            ActionNode<long, object> testNode = new ActionNode<long, object>("TestNode", (t, o) => BehaviourReturnCode.Failure);
            Assert.Equal(BehaviourReturnCode.Failure, testNode.Visit(1, null));
            Assert.Equal(BehaviourReturnCode.Failure, testNode.CurrentState);
        }

        [Fact]
        public void TestRunning()
        {
            ActionNode<long, object> testNode = new ActionNode<long, object>("TestNode", (t, o) => BehaviourReturnCode.Running);
            Assert.Equal(BehaviourReturnCode.Running, testNode.Visit(1, null));
            Assert.Equal(BehaviourReturnCode.Running, testNode.CurrentState);
        }

        [Fact]
        public void TestError()
        {
            ActionNode<long, object> testNode = new ActionNode<long, object>("TestNode", (t, o) => BehaviourReturnCode.Error);
            Assert.Equal(BehaviourReturnCode.Error, testNode.Visit(1, null));
            Assert.Equal(BehaviourReturnCode.Error, testNode.CurrentState);
        }

        [Fact]
        public void TestErrorNoFunction()
        {
            ActionNode<long, object> testNode = new ActionNode<long, object>("TestNode", null);
            Assert.Equal(BehaviourReturnCode.Error, testNode.Visit(1, null));
            Assert.Equal(BehaviourReturnCode.Error, testNode.CurrentState);
        }

        [Fact]
        public void TestGetState()
        {
            ActionNode<long, object> testNode = new ActionNode<long, object>("TestNode", (t, o) => BehaviourReturnCode.Success);
            testNode.Visit(1, null);

            var state = testNode.GetState();

            Assert.NotNull(state);
            Assert.Equal("TestNode", state.NodeName);
            Assert.Equal(BehaviourReturnCode.Success, state.CurrentState);
            Assert.Empty(state.Children);
        }

        [Fact]
        public void TestStateReset()
        {
            ActionNode<long, object> testNode = new ActionNode<long, object>("TestNode", (t, o) => BehaviourReturnCode.Success);
            testNode.Visit(1, null);
            testNode.ResetState();

            Assert.Equal(BehaviourReturnCode.Ready, testNode.CurrentState); 
        }
    }
}
