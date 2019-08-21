using System;
using System.Collections.Generic;
using System.Text;

namespace AYLib.BehaviourTree
{
    public interface IBehaviourTreeParentNode
    {
        void AddChild(BehaviourNode childNode);
    }
}
