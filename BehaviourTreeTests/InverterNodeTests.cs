using C4i.BehaviourTree;
using System;
using Xunit;

namespace BehaviourTreeTests
{
    public class InverterNodeTests
    {
        [Fact]
        public void TestSuccess()
        {
            ActionNode testNode = new ActionNode("TestNode", (t, o) => BehaviourReturnCode.Success);
            InverterNode inverter = new InverterNode("Invert", testNode);

            Assert.Equal(BehaviourReturnCode.Failure, inverter.Visit(1, null));
            Assert.Equal(BehaviourReturnCode.Failure, inverter.CurrentState);
        }

        [Fact]
        public void TestFailure()
        {
            ActionNode testNode = new ActionNode("TestNode", (t, o) => BehaviourReturnCode.Failure);
            InverterNode inverter = new InverterNode("Invert", testNode);

            Assert.Equal(BehaviourReturnCode.Success, inverter.Visit(1, null));
            Assert.Equal(BehaviourReturnCode.Success, inverter.CurrentState);
        }

        [Fact]
        public void TestRunning()
        {
            ActionNode testNode = new ActionNode("TestNode", (t, o) => BehaviourReturnCode.Running);
            InverterNode inverter = new InverterNode("Invert", testNode);

            Assert.Equal(BehaviourReturnCode.Running, inverter.Visit(1, null));
            Assert.Equal(BehaviourReturnCode.Running, inverter.CurrentState);
        }

        [Fact]
        public void TestError()
        {
            ActionNode testNode = new ActionNode("TestNode", (t, o) => BehaviourReturnCode.Error);
            InverterNode inverter = new InverterNode("Invert", testNode);

            Assert.Equal(BehaviourReturnCode.Error, inverter.Visit(1, null));
            Assert.Equal(BehaviourReturnCode.Error, inverter.CurrentState);
        }

        [Fact]
        public void TestGetState()
        {
            ActionNode testNode = new ActionNode("TestNode", (t, o) => BehaviourReturnCode.Failure);
            InverterNode inverter = new InverterNode("Invert", testNode);
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
            ActionNode testNode = new ActionNode("TestNode", (t, o) => BehaviourReturnCode.Failure);
            InverterNode inverter = new InverterNode("Invert", testNode);
            inverter.Visit(1, null);

            inverter.ResetState();

            Assert.Equal(BehaviourReturnCode.Ready, testNode.CurrentState);
            Assert.Equal(BehaviourReturnCode.Ready, inverter.CurrentState);
        }
    }
}
