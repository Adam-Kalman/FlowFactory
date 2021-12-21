using System;

namespace FlowFactory.Container
{
    /// <summary>
    /// Exception EventArgs with cancelation 
    /// </summary>
    public class ContainerExceptionEventArgs : Exception
    {
        /// <summary>
        /// Terminate container process and exit
        /// </summary>
        public bool Cancel { get; set; }

        /// <summary>
        /// Exception handler
        /// </summary>
        public Exception Exception { get; }

        public ContainerExceptionEventArgs(Exception e) 
        {
            Exception = e;
        }
    }
}
