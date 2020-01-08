using System;
using System.Collections.Generic;
using System.Text;

namespace AYLib.BehaviourTree
{
    /// <summary>
    /// Defines a node that can contain child nodes (non-leaf).
    /// </summary>
    public interface IBehaviourTreeParentNode
    {
        void AddChild(BehaviourNode childNode);
    }
}
