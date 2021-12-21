namespace FlowFactory.Controllers
{
    /// <summary>
    /// Container controller module interface
    /// </summary>
    public interface IContainerController
    {
        /// <summary>
        /// Return controller data validation status
        /// </summary>
        /// <returns></returns>
        bool IsValid();

        /// <summary>
        /// Return controller active status
        /// </summary>
        /// <returns></returns>
        bool IsActive();

        /// <summary>
        /// Run controller
        /// </summary>
        void Start();
    }
}