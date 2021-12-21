namespace FlowFactory.Enums
{
    /// <summary>
    /// Container working statuses
    /// </summary>
    public enum FactoryStatus
    {
        None = 0,
        InitializedControllerItems,
        BeforePreprocessing,
        AfterPreprocessing,
        Running,
        BeforePostprocessing,
        AfterPostprocessing,
        Finished,
        CanceledByUser
    }
}
