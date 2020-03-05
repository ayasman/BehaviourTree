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
            ActionNode<long, object> testNode = new ActionNode<long, object>("TestNode", (t, o) => BehaviourReturnCode.Success);
            BehaviourTree<long, object> testRoot = new BehaviourTree<long, object>("Root", testNode);
            var state = testRoot.Visit(1, null);

            Assert.Equal(BehaviourReturnCode.Success, state);
            Assert.Equal(BehaviourReturnCode.Success, testRoot.CurrentState);
        }

        [Fact]
        public void TestConstuctorFail()
        {
            BehaviourTree<long, object> testNode = new BehaviourTree<long, object>("Root");
            Assert.Throws<ApplicationException>(() => testNode.Visit(1, null));
        }

        [Fact]
        public void TestGetState()
        {
            ActionNode<long, object> testNode = new ActionNode<long, object>("TestNode", (t, o) => BehaviourReturnCode.Success);
            BehaviourTree<long, object> testRoot = new BehaviourTree<long, object>("Root", testNode);
            testRoot.Visit(1, null);

            var state = testRoot.GetState();

            Assert.NotNull(state);
            Assert.Equal("Root", state.NodeName);
            Assert.Equal(BehaviourReturnCode.Success, state.CurrentState);
            Assert.Single(state.Children);
        }
    }
}
