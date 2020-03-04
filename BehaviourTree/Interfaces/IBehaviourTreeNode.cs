using System;
using System.Collections.Generic;
using System.Text;

namespace AYLib.BehaviourTree
{
    /// <summary>
    /// Interface for a node in the tree. Should be the only thing an external library uses to visit the tree once created.
    /// </summary>
    public interface IBehaviourTreeNode<TTime, TContext>
    {
        /// <summary>
        /// Visits the node.
        /// </summary>
        /// <param name="elapsedTime">The time since last visit</param>
        /// <param name="dataContext">The data context to run against</param>
        /// <returns>Completion state of the node</returns>
        BehaviourReturnCode Visit(TTime elapsedTime, TContext dataContext);

        /// <summary>
        /// Gets the state data of the node.
        /// </summary>
        /// <returns>Node state data</returns>
        IBehaviourTreeState GetState();
    }
}
