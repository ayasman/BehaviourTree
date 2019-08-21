using AYLib.BehaviourTree;
using System;
using Xunit;

namespace BehaviourTreeTests
{
    public class RootNodeTests
    {
        [Fact]
        public void TestConstuctorWithAction()
        {
            ActionNode testNode = new ActionNode("TestNode", (t, o) => BehaviourReturnCode.Success);
            BehaviourTree testRoot = new BehaviourTree("Root", testNode);
            var state = testRoot.Visit(1, null);

            Assert.Equal(BehaviourReturnCode.Success, state);
            Assert.Equal(BehaviourReturnCode.Success, testRoot.CurrentState);
        }

        [Fact]
        public void TestConstuctorFail()
        {
            BehaviourTree testNode = new BehaviourTree("Root");
            Assert.Throws<ApplicationException>(() => testNode.Visit(1, null));
        }

        [Fact]
        public void TestGetState()
        {
            ActionNode testNode = new ActionNode("TestNode", (t, o) => BehaviourReturnCode.Success);
            BehaviourTree testRoot = new BehaviourTree("Root", testNode);
            testRoot.Visit(1, null);

            var state = testRoot.GetState();

            Assert.NotNull(state);
            Assert.Equal("Root", state.NodeName);
            Assert.Equal(BehaviourReturnCode.Success, state.CurrentState);
            Assert.Single(state.Children);
        }
    }
}
