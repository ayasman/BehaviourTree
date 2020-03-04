using AYLib.BehaviourTree;
using System;
using Xunit;

namespace BehaviourTreeTests
{
    public class InverterNodeTests
    {
        [Fact]
        public void TestSuccess()
        {
            ActionNode<long, object> testNode = new ActionNode<long, object>("TestNode", (t, o) => BehaviourReturnCode.Success);
            InverterNode<long, object> inverter = new InverterNode<long, object>("Invert", testNode);

            Assert.Equal(BehaviourReturnCode.Failure, inverter.Visit(1, null));
            Assert.Equal(BehaviourReturnCode.Failure, inverter.CurrentState);
        }

        [Fact]
        public void TestFailure()
        {
            ActionNode<long, object> testNode = new ActionNode<long, object>("TestNode", (t, o) => BehaviourReturnCode.Failure);
            InverterNode<long, object> inverter = new InverterNode<long, object>("Invert", testNode);

            Assert.Equal(BehaviourReturnCode.Success, inverter.Visit(1, null));
            Assert.Equal(BehaviourReturnCode.Success, inverter.CurrentState);
        }

        [Fact]
        public void TestRunning()
        {
            ActionNode<long, object> testNode = new ActionNode<long, object>("TestNode", (t, o) => BehaviourReturnCode.Running);
            InverterNode<long, object> inverter = new InverterNode<long, object>("Invert", testNode);

            Assert.Equal(BehaviourReturnCode.Running, inverter.Visit(1, null));
            Assert.Equal(BehaviourReturnCode.Running, inverter.CurrentState);
        }

        [Fact]
        public void TestError()
        {
            ActionNode<long, object> testNode = new ActionNode<long, object>("TestNode", (t, o) => BehaviourReturnCode.Error);
            InverterNode<long, object> inverter = new InverterNode<long, object>("Invert", testNode);

            Assert.Equal(BehaviourReturnCode.Error, inverter.Visit(1, null));
            Assert.Equal(BehaviourReturnCode.Error, inverter.CurrentState);
        }

        [Fact]
        public void TestGetState()
        {
            ActionNode<long, object> testNode = new ActionNode<long, object>("TestNode", (t, o) => BehaviourReturnCode.Failure);
            InverterNode<long, object> inverter = new InverterNode<long, object>("Invert", testNode);
            inverter.Visit(1, null);

            var state = inverter.GetState();

            Assert.NotNull(state);
            Assert.Equal("Invert", state.NodeName);
            Assert.Equal(BehaviourReturnCode.Success, state.CurrentState);
            Assert.Single(state.Children);
        }

        [Fact]
        public void TestStateReset()
        {
            ActionNode<long, object> testNode = new ActionNode<long, object>("TestNode", (t, o) => BehaviourReturnCode.Failure);
            InverterNode<long, object> inverter = new InverterNode<long, object>("Invert", testNode);
            inverter.Visit(1, null);

            inverter.ResetState();

            Assert.Equal(BehaviourReturnCode.Ready, testNode.CurrentState);
            Assert.Equal(BehaviourReturnCode.Ready, inverter.CurrentState);
        }
    }
}
