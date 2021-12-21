using FlowFactory.Controllers;
using System;

namespace FlowFactory
{
    /// <summary>
    /// Some controller
    /// </summary>
    public class AnotherExampleController : ControllerBase<MyContextModel>, IContainerController, IDisposable
    {
        public AnotherExampleController(MyContextModel model) : base(model)
        {
            
        }

        /// <summary>
        /// Get validation status 
        /// </summary>
        /// <returns></returns>
        public bool IsValid()
        {
            //some validation here
            return true;
        }
        /// <summary>
        /// Get Active status
        /// </summary>
        /// <returns></returns>
        public bool IsActive()
        {
            return Active;
        }
        /// <summary>
        /// Run controller
        /// </summary>
        public void Start()
        {
            if (!IsValid()) return;


        }

        /// <summary>
        /// IDisposable inheritance
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposing) return;
        }

        /// <summary>
        /// Object finalizer
        /// </summary>
        ~AnotherExampleController()
        {
            Dispose(false);
        }
    }
}
