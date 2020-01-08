using System;
using System.Collections.Generic;
using System.Text;

namespace AYLib.BehaviourTree
{
    /// <summary>
    /// Interface for a node in the tree. Should be the only thing an external library uses to visit the tree once created.
    /// </summary>
    public interface IBehaviourTreeNode
    {
        BehaviourReturnCode Visit(long elapsedTime, object dataContext);

        IBehaviourTreeState GetState();
    }
}
