using AYLib.BehaviourTree;
using System;
using Xunit;

namespace BehaviourTreeTests
{
    public class WhileNodeTests
    {
        [Fact]
        public void TestSuccess()
        {
            int condCounter = 0;
            int actCounter = 0;

            ActionNode<long, object> conditionNode = new ActionNode<long, object>("Condition", (t, o) =>
            {
                condCounter++;
                if (condCounter > 1)
                    return BehaviourReturnCode.Failure;
                return BehaviourReturnCode.Success;
            });
            ActionNode<long, object> actionNode = new ActionNode<long, object>("Action", (t, o) =>
            {
                actCounter++;
                return BehaviourReturnCode.Success;
            });

            WhileNode<long, object> testNode = new WhileNode<long, object>("WhileNode", conditionNode, actionNode);
            var status = testNode.Visit(1, null);

            Assert.Equal(BehaviourReturnCode.Success, status);
            Assert.Equal(BehaviourReturnCode.Success, testNode.CurrentState);
            Assert.Equal(1, actCounter);
            Assert.Equal(2, condCounter);
        }

        [Fact]
        public void TestSuccessMoreChecks()
        {
            int condCounter = 0;
            int actCounter = 0;

            ActionNode<long, object> conditionNode = new ActionNode<long, object>("Condition", (t, o) =>
            {
                condCounter++;
                if (condCounter > 3)
                    return BehaviourReturnCode.Failure;
                return BehaviourReturnCode.Success;
            });
            ActionNode<long, object> actionNode = new ActionNode<long, object>("Action", (t, o) =>
            {
                actCounter++;
                return BehaviourReturnCode.Success;
            });

            WhileNode<long, object> testNode = new WhileNode<long, object>("WhileNode", conditionNode, actionNode);
            var status = testNode.Visit(1, null);

            Assert.Equal(BehaviourReturnCode.Success, status);
            Assert.Equal(BehaviourReturnCode.Success, testNode.CurrentState);
            Assert.Equal(3, actCounter);
            Assert.Equal(4, condCounter);
        }

        [Fact]
        public void TestConditionFailedNoAction()
        {
            int condCounter = 0;
            int actCounter = 0;

            ActionNode<long, object> conditionNode = new ActionNode<long, object>("Condition", (t, o) =>
            {
                condCounter++;
                return BehaviourReturnCode.Failure;
            });
            ActionNode<long, object> actionNode = new ActionNode<long, object>("Action", (t, o) =>
            {
                actCounter++;
                return BehaviourReturnCode.Success;
            });

            WhileNode<long, object> testNode = new WhileNode<long, object>("WhileNode", conditionNode, actionNode);
            var status = testNode.Visit(1, null);

            Assert.Equal(BehaviourReturnCode.Failure, status);
            Assert.Equal(BehaviourReturnCode.Failure, testNode.CurrentState);
            Assert.Equal(0, actCounter);
            Assert.Equal(1, condCounter);
        }

        [Fact]
        public void TesActionFailedFirstTry()
        {
            int condCounter = 0;
            int actCounter = 0;

            ActionNode<long, object> conditionNode = new ActionNode<long, object>("Condition", (t, o) =>
            {
                condCounter++;
                return BehaviourReturnCode.Success;
            });
            ActionNode<long, object> actionNode = new ActionNode<long, object>("Action", (t, o) =>
            {
                actCounter++;
                return BehaviourReturnCode.Failure;
            });

            WhileNode<long, object> testNode = new WhileNode<long, object>("WhileNode", conditionNode, actionNode);
            var status = testNode.Visit(1, null);

            Assert.Equal(BehaviourReturnCode.Failure, status);
            Assert.Equal(BehaviourReturnCode.Failure, testNode.CurrentState);
            Assert.Equal(1, actCounter);
            Assert.Equal(1, condCounter);
        }

        [Fact]
        public void TesActionFailedSecondTry()
        {
            int condCounter = 0;
            int actCounter = 0;

            ActionNode<long, object> conditionNode = new ActionNode<long, object>("Condition", (t, o) =>
            {
                condCounter++;
                return BehaviourReturnCode.Success;
            });
            ActionNode<long, object> actionNode = new ActionNode<long, object>("Action", (t, o) =>
            {
                actCounter++;
                if (actCounter > 1)
                    return BehaviourReturnCode.Failure;
                return BehaviourReturnCode.Success;
            });

            WhileNode<long, object> testNode = new WhileNode<long, object>("WhileNode", conditionNode, actionNode);
            var status = testNode.Visit(1, null);

            Assert.Equal(BehaviourReturnCode.Failure, status);
            Assert.Equal(BehaviourReturnCode.Failure, testNode.CurrentState);
            Assert.Equal(2, actCounter);
            Assert.Equal(2, condCounter);
        }

        [Fact]
        public void TesActionSucceedRentryCondition()
        {
            int condCounter = 0;
            int actCounter = 0;

            ActionNode<long, object> conditionNode = new ActionNode<long, object>("Condition", (t, o) =>
            {
                condCounter++;
                if (condCounter == 3 || condCounter == 7)
                    return BehaviourReturnCode.Success;
                if (condCounter > 7)
                    return BehaviourReturnCode.Failure;
                return BehaviourReturnCode.Running;
            });
            ActionNode<long, object> actionNode = new ActionNode<long, object>("Action", (t, o) =>
            {
                actCounter++;
                return BehaviourReturnCode.Success;
            });

            WhileNode<long, object> testNode = new WhileNode<long, object>("WhileNode", conditionNode, actionNode);

            var status = testNode.Visit(1, null);
            Assert.Equal(BehaviourReturnCode.Running, status);
            Assert.Equal(BehaviourReturnCode.Running, testNode.CurrentState);

            status = testNode.Visit(1, null);
            status = testNode.Visit(1, null);
            status = testNode.Visit(1, null);
            status = testNode.Visit(1, null);
            status = testNode.Visit(1, null);

            Assert.Equal(BehaviourReturnCode.Success, status);
            Assert.Equal(BehaviourReturnCode.Success, testNode.CurrentState);
            Assert.Equal(2, actCounter);
            Assert.Equal(8, condCounter);
        }

        [Fact]
        public void TesActionSucceedRentryAction()
        {
            int condCounter = 0;
            int actCounter = 0;

            ActionNode<long, object> conditionNode = new ActionNode<long, object>("Condition", (t, o) =>
            {
                condCounter++;
                if (condCounter == 3)
                    return BehaviourReturnCode.Success;
                if (condCounter > 3)
                    return BehaviourReturnCode.Failure;
                return BehaviourReturnCode.Running;
            });
            ActionNode<long, object> actionNode = new ActionNode<long, object>("Action", (t, o) =>
            {
                actCounter++;
                if (actCounter == 3)
                    return BehaviourReturnCode.Success;
                return BehaviourReturnCode.Running;
            });

            WhileNode<long, object> testNode = new WhileNode<long, object>("WhileNode", conditionNode, actionNode);

            var status = testNode.Visit(1, null);
            Assert.Equal(BehaviourReturnCode.Running, status);
            Assert.Equal(BehaviourReturnCode.Running, testNode.CurrentState);

            status = testNode.Visit(1, null);
            status = testNode.Visit(1, null);
            status = testNode.Visit(1, null);
            status = testNode.Visit(1, null);

            Assert.Equal(BehaviourReturnCode.Success, status);
            Assert.Equal(BehaviourReturnCode.Success, testNode.CurrentState);
            Assert.Equal(3, actCounter);
            Assert.Equal(4, condCounter);
        }

        [Fact]
        public void TesActionFailRentry()
        {
            int condCounter = 0;
            int actCounter = 0;

            ActionNode<long, object> conditionNode = new ActionNode<long, object>("Condition", (t, o) =>
            {
                condCounter++;
                if (condCounter == 3 || condCounter == 7)
                    return BehaviourReturnCode.Success;
                if (condCounter > 7)
                    return BehaviourReturnCode.Failure;
                return BehaviourReturnCode.Running;
            });
            ActionNode<long, object> actionNode = new ActionNode<long, object>("Action", (t, o) =>
            {
                actCounter++;
                if (actCounter == 3)
                    return BehaviourReturnCode.Failure;
                return BehaviourReturnCode.Running;
            });

            WhileNode<long, object> testNode = new WhileNode<long, object>("WhileNode", conditionNode, actionNode);

            var status = testNode.Visit(1, null);
            Assert.Equal(BehaviourReturnCode.Running, status);
            Assert.Equal(BehaviourReturnCode.Running, testNode.CurrentState);

            status = testNode.Visit(1, null);
            status = testNode.Visit(1, null);
            status = testNode.Visit(1, null);
            status = testNode.Visit(1, null);

            Assert.Equal(BehaviourReturnCode.Failure, status);
            Assert.Equal(BehaviourReturnCode.Failure, testNode.CurrentState);
            Assert.Equal(3, actCounter);
            Assert.Equal(3, condCounter);
        }

        [Fact]
        public void TestGetState()
        {
            ActionNode<long, object> conditionNode = new ActionNode<long, object>("Condition", (t, o) => BehaviourReturnCode.Success);
            ActionNode<long, object> actionNode = new ActionNode<long, object>("Action", (t, o) => BehaviourReturnCode.Failure);
            WhileNode<long, object> testNode = new WhileNode<long, object>("WhileNode", conditionNode, actionNode);
            testNode.Visit(1, null);

            var state = testNode.GetState();

            Assert.NotNull(state);
            Assert.Equal("WhileNode", state.NodeName);
            Assert.Equal(BehaviourReturnCode.Failure, state.CurrentState);
            Assert.Equal(2, state.Children.Count);
        }

        [Fact]
        public void TestStateReset()
        {
            ActionNode<long, object> conditionNode = new ActionNode<long, object>("Condition", (t, o) => BehaviourReturnCode.Success);
            ActionNode<long, object> actionNode = new ActionNode<long, object>("Action", (t, o) => BehaviourReturnCode.Failure);
            WhileNode<long, object> testNode = new WhileNode<long, object>("WhileNode", conditionNode, actionNode);
            testNode.Visit(1, null);

            testNode.ResetState();

            Assert.Equal(BehaviourReturnCode.Ready, testNode.CurrentState);
            Assert.Equal(BehaviourReturnCode.Ready, conditionNode.CurrentState);
            Assert.Equal(BehaviourReturnCode.Ready, actionNode.CurrentState);
        }
    }
}
