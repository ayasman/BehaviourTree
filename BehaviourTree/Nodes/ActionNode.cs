using System;

namespace AYLib.BehaviourTree
{
    /// <summary>
    /// A leaf, or action, node within a tree. Contains no children, and does the action sent in when the Visit method is called.
    /// </summary>
    public class ActionNode : BehaviourNode
    {
        private Func<double, object, BehaviourReturnCode> actionFunction = null;

        public ActionNode(string nodeName, Func<double, object, BehaviourReturnCode> action)
            : base(nodeName)
        {
            actionFunction = action;
        }

        public override BehaviourReturnCode Visit(long elapsedTime, object dataContext)
        {
            if (actionFunction != null)
            {
                return Return(actionFunction.Invoke(elapsedTime, dataContext));
            }
            return Error();
        }
    }
}
