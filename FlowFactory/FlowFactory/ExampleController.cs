using FlowFactory.Controllers;
using System;
using System.Linq;

namespace FlowFactory
{
    /// <summary>
    /// Some controller 
    /// </summary>
    public class ExampleController : ControllerBase<MyContextModel>, IContainerController, IDisposable
    {
        public ExampleController(MyContextModel model) : base(model)
        {
        }

        /// <inheritdoc />
        /// <summary>
        /// Interface implementation
        /// </summary>
        /// <returns></returns>
        public bool IsValid()
        {
            return true;
        }

        /// <inheritdoc />
        /// <summary>
        /// Interface implementation
        /// </summary>
        /// <returns></returns>
        public bool IsActive()
        {
            return Active;
        }

        /// <inheritdoc />
        public void Start()
        {
            if (!IsValid()) return;

            ControllerConfigItem.First(x => x.ControllerType == typeof(AnotherExampleController)).Active = false;
        }

        /// <inheritdoc />
        /// <summary>
        /// IDisposable inherit
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Interface implementation
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposing) return;
        }

        /// <summary>
        /// Class finalizer
        /// </summary>
        ~ExampleController()
        {
            Dispose(false);
        }
    }
}
