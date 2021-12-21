using FlowFactory.Config;
using System.Collections.Generic;

namespace FlowFactory.Controllers
{
    /// <summary>
    /// Base abstract class for every container module controller
    /// </summary>
    public abstract class ControllerBase<T>
    {
        /// <summary>
        /// Is controller Active
        /// </summary>
        public bool Active { get; set; } = true;
        /// <summary>
        /// Handler for model passed to controller from Config
        /// </summary>
        public T ContextModel { get; }
        /// <summary>
        /// Controller items handler
        /// </summary>
        public List<ControllerConfigItem> ControllerConfigItem { get; set; }

        protected ControllerBase(T model)
        {
            ContextModel = model;
        }
    }
}