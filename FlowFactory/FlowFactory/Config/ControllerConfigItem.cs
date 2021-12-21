using FlowFactory.Controllers;
using System;

namespace FlowFactory.Config
{
    /// <summary>
    /// One controller item model
    /// </summary>
    public class ControllerConfigItem
    {
        /// <summary>
        /// Define that item is ready to check valid status<see cref="IContainerController.IsValid"></see>, 
        /// active status <see cref="IContainerController.IsActive"></see> and run <see cref="IContainerController.Start"></see>
        /// <para>If set to false, <see cref="Container.Container"></see> will never check and run this controller.</para>
        /// </summary>
        public bool Active { get; set; } = true;
        /// <summary>
        /// Type of controller to instantiate
        /// </summary>
        public Type ControllerType { get; set; }
        /// <summary>
        /// Instance of controller
        /// </summary>
        public IContainerController Instance { get; set; }
        /// <summary>
        /// Set true if Controller successfully initialized
        /// </summary>
        public bool IsInitialized { get; set; }
        /// <summary>
        /// Set true if Controller was successfully executed
        /// </summary>
        public bool IsExecuted { get; set; }
    }
}
