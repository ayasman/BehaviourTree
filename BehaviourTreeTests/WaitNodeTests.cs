using AYLib.BehaviourTree;
using System;
using Xunit;

namespace BehaviourTreeTests
{
    public class WaitNodeTests
    {
        [Fact]
        public void TestSuccess()
        {
            WaitNode waitNode = new WaitNode("Wait", 100);

            Assert.Equal(BehaviourReturnCode.Ready, waitNode.CurrentState);

            Assert.Equal(BehaviourReturnCode.Success, waitNode.Visit(100, null));
            Assert.Equal(BehaviourReturnCode.Success, waitNode.CurrentState);
        }

        [Fact]
        public void TestLongSuccess()
        {
            WaitNode waitNode = new WaitNode("Wait", 500);

            var state = waitNode.Visit(100, null);
            Assert.Equal(BehaviourReturnCode.Running, state);
            Assert.Equal(BehaviourReturnCode.Running, waitNode.CurrentState);

            state = waitNode.Visit(100, null);
            Assert.Equal(BehaviourReturnCode.Running, state);
            Assert.Equal(BehaviourReturnCode.Running, waitNode.CurrentState);

            state = waitNode.Visit(100, null);
            Assert.Equal(BehaviourReturnCode.Running, state);
            Assert.Equal(BehaviourReturnCode.Running, waitNode.CurrentState);

            state = waitNode.Visit(100, null);
            Assert.Equal(BehaviourReturnCode.Running, state);
            Assert.Equal(BehaviourReturnCode.Running, waitNode.CurrentState);

            state = waitNode.Visit(100, null);
            Assert.Equal(BehaviourReturnCode.Success, state);
            Assert.Equal(BehaviourReturnCode.Success, waitNode.CurrentState);

            state = waitNode.Visit(100, null);
            Assert.Equal(BehaviourReturnCode.Running, state);
            Assert.Equal(BehaviourReturnCode.Running, waitNode.CurrentState);
        }

        [Fact]
        public void TestGetState()
        {
            WaitNode waitNode = new WaitNode("Wait", 500);
            waitNode.Visit(1, null);

            var state = waitNode.GetState();

            Assert.NotNull(state);
            Assert.Equal("Wait", state.NodeName);
            Assert.Equal(BehaviourReturnCode.Running, state.CurrentState);
            Assert.Empty(state.Children);
        }

        [Fact]
        public void TestStateReset()
        {
            WaitNode waitNode = new WaitNode("Wait", 500);
            waitNode.Visit(1, null);

            waitNode.ResetState();

            Assert.Equal(BehaviourReturnCode.Ready, waitNode.CurrentState);
        }
    }
}
