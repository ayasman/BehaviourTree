using System;
using System.Collections.Generic;
using System.Text;

namespace C4i.BehaviourTree
{
    public interface IBehaviourTreeParentNode
    {
        void AddChild(BehaviourNode childNode);
    }
}
