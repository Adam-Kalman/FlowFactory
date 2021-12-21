using FlowFactory.Enums;
using System;

namespace FlowFactory.Container
{
    /// <summary>
    /// Provide current container status
    /// </summary>
    public class ContainerStatusEventArgs : EventArgs
    {
        /// <summary>
        /// Status handler
        /// </summary>
        public FactoryStatus Status { get; }

        public ContainerStatusEventArgs(FactoryStatus status)
        {
            Status = status;
        }
    }
}
