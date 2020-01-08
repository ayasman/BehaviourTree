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
        /// <summary>
        /// Sets the root node of the tree.
        /// </summary>
        /// <param name="childNode">The new root node</param>
        void AddChild(BehaviourNode childNode);
    }
}
