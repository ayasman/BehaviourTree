using System;
using System.Collections.Generic;
using System.Text;

namespace C4i.BehaviourTree
{
    public interface IBehaviourTreeNode
    {
        BehaviourReturnCode Visit(long elapsedTime, object dataContext);

        IBehaviourTreeState GetState();
    }
}
