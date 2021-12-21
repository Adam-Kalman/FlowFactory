using FlowFactory.Config;
using System;
using System.Collections.Generic;

namespace FlowFactory.Container
{
    /// <summary>
    /// Container config interface
    /// </summary>
    public interface IContainerConfig
    {
        /// <summary>
        /// List of currently added controller items
        /// </summary>
        /// <returns></returns>
        List<ControllerConfigItem> GetControllerItemsList();

        /// <summary>
        /// Register attached modules for Container process
        /// </summary>
        void ControllerItemRegister();

        /// <summary>
        /// Set controllers visible and active due to implemented logic
        /// </summary>
        void SetActiveControllers();

        /// <summary>
        /// Execute implementation BEFORE module controllers run
        /// </summary>
        void Preprocessing();

        /// <summary>
        /// Execute implementation AFTER module controllers run 
        /// </summary>
        void Postprocessing();

        /// <summary>
        /// Invoke when controller successfully run its constructor and was attached to module list
        /// </summary>
        event EventHandler OnControllerRegistered;

        /// <summary>
        /// Invoke when controller thrown exception while running constructor
        /// </summary>
        event EventHandler<ContainerExceptionEventArgs> OnControllerInitFailed;
    }
}