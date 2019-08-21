using System;

namespace AYLib.BehaviourTree
{
    /// <summary>
    /// Return codes for a node status.
    /// </summary>
    public enum BehaviourReturnCode
    {
        Ready,
        Running,
        Success,
        Failure,
        Error
    }
}
