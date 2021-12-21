using System;
using FlowFactory.Config;

namespace FlowFactory.Container
{
    public partial class Container
    {
        /// <summary>
        /// Invoke on every <see cref="Enums.FactoryStatus"></see> change provided list of <see cref="ControllerConfigItem"></see>
        /// </summary>
        public event EventHandler<ContainerStatusEventArgs> OnContainerStatusChange;
        /// <summary>
        /// Invoke before controller Start method will be called
        /// </summary>
        public event EventHandler OnBeforeControllerStart;
        /// <summary>
        /// Invoke after controller Start method was finished successfully, otherwise OnControllerStartFailed will be calling.
        /// </summary>
        public event EventHandler OnAfterControllerStart;
        /// <summary>
        /// Invoke when controller Start method thrown an exception
        /// </summary>
        public event EventHandler<ContainerExceptionEventArgs> OnControllerRunFailed;
        /// <summary>
        /// Invoke when controller successfully run its constructor and was attached to module list
        /// </summary>
        public event EventHandler OnControllerRegistered;
        /// <summary>
        /// Invoke when controller thrown exception while running constructor
        /// </summary>
        public event EventHandler<ContainerExceptionEventArgs> OnControllerInitFailed;
        /// <summary>
        /// Invoke when Container write console output when Debug is set to true
        /// </summary>
        public event EventHandler<string> OnConsoleOutput;
    }
}
