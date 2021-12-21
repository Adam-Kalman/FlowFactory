using FlowFactory.Config;
using FlowFactory.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace FlowFactory.Container
{
    /// <summary>
    /// Controller factory schema class
    /// </summary>
    public partial class Container
    {
        /// <summary>
        /// Is internal debug active
        /// </summary>
        public static bool Debug { get; set; } = true;
        /// <summary>
        /// Container config handler
        /// </summary>
        protected IContainerConfig Config { get; set; }
        /// <summary>
        /// Will be true, when initialize process ends
        /// </summary>
        private bool controllersHandlerActive;
        /// <summary>
        /// Stopwatch handler
        /// </summary>
        private readonly Stopwatch stopwatch = new Stopwatch();

        private FactoryStatus status;
        /// <summary>
        /// Container working status
        /// </summary>
        public FactoryStatus Status 
        {
            get => status;
            set
            {
                status = value;
                OnContainerStatusChange?.Invoke(Config.GetControllerItemsList(), new ContainerStatusEventArgs(status));
            }
        }

        public Container(IContainerConfig c)
        {
            Config = c;
            Config.OnControllerInitFailed += (sender, e) => OnControllerInitFailed?.Invoke(sender, e);
            Config.OnControllerRegistered += (sender, e) => OnControllerRegistered?.Invoke(sender, e);
            Config.SetActiveControllers();
        }

        /// <summary>
        /// Container initializer
        /// </summary>
        public void Init()
        {
            stopwatch.Start();
            WriteDebugMessage(string.Empty);
            WriteDebugMessage("Container: (START)");
            InitControllerItems();
        }

        /// <summary>
        /// Initialize module controllers registered in config
        /// </summary>
        /// <returns></returns>
        private void InitControllerItems()
        {
            try
            {
                Config.ControllerItemRegister();
            }
            catch(OperationCanceledException)
            {
                WriteDebugMessage($"Container: Initialization failed! Canceled by user.");
                Status = FactoryStatus.CanceledByUser;
                return;
            }

            if (!Config.GetControllerItemsList().Any())
            {
                WriteDebugMessage("No modules loaded");
                return;
            }

            
            WriteDebugMessage("Registered modules: " + Config.GetControllerItemsList().Count);
            Status = FactoryStatus.InitializedControllerItems;
        }

        /// <summary>
        /// Run active and visible controllers or events
        /// </summary>
        public void Run()
        {
            if (status != FactoryStatus.InitializedControllerItems)
            {
                WriteDebugMessage($"Container: Wrong status! Expected {FactoryStatus.InitializedControllerItems} but was {status}");
                return;
            }

            Status = FactoryStatus.BeforePreprocessing;
            WriteDebugMessage($"Container: Config.Preprocessing() (START) [ {stopwatch.Elapsed.TotalSeconds:0.0000000} ]");
            Config.Preprocessing();
            Status = FactoryStatus.AfterPreprocessing;
            WriteDebugMessage($"Container: Config.Preprocessing() (END) [ {stopwatch.Elapsed.TotalSeconds:0.0000000} ]");

            Status = FactoryStatus.Running;
            foreach (var item in Config.GetControllerItemsList())
            {
                try
                {
                    if (item.Active)
                    {
                        if (item.Instance.IsValid())
                        {
                            if (item.Instance.IsActive())
                            {
                                var microtime = Stopwatch.StartNew();
                                WriteDebugMessage($"Controller module '{item.Instance.GetType()}' (START)");

                                try
                                {
                                    OnBeforeControllerStart?.Invoke(item, EventArgs.Empty);
                                    item.Instance.Start();
                                    item.IsExecuted = true;
                                    OnAfterControllerStart?.Invoke(item, EventArgs.Empty);
                                }
                                catch (Exception e)
                                {
                                    WriteDebugMessage($"Controller '{item.Instance.GetType()}' Start method thrown {e.GetType().Name}: {e.Message}.");
                                    var ea = new ContainerExceptionEventArgs(e);
                                    OnControllerRunFailed?.Invoke(item, ea);
                                    if (ea.Cancel)
                                    {
                                        Status = FactoryStatus.CanceledByUser;
                                        return;
                                    }
                                }

                                WriteDebugMessage($"Controller module '{item.Instance.GetType()}' (END) in {microtime.Elapsed.TotalSeconds:0.0000000} sec.");
                            }
                            else
                            {
                                WriteDebugMessage($"Controller module '{item.Instance.GetType()}' omitted due to ACTIVE status");
                            }
                        }
                        else
                        {
                            WriteDebugMessage($"Controller module '{item.Instance.GetType()}' omitted due to VALID status");
                        }
                    }
                    else
                    {
                        WriteDebugMessage($"Controller module '{item.ControllerType}' omitted due to ACCESSIBILITY status or failed to start.");
                    }
                }
                catch(Exception e)
                {
                    WriteDebugMessage($"Controller '{item.Instance.GetType()}' thrown {e.GetType().Name}: {e.Message}.");
                }
            }

            controllersHandlerActive = true;

            Status = FactoryStatus.BeforePostprocessing;
            WriteDebugMessage($"Container: Config.Postprocessing() (START) [ {stopwatch.Elapsed.TotalSeconds:0.0000000} ]");
            Config.Postprocessing();
            Status = FactoryStatus.AfterPostprocessing;
            WriteDebugMessage($"Container: Config.Postprocessing() (END) [ {stopwatch.Elapsed.TotalSeconds:0.0000000} ]");
            WriteDebugMessage($"Container: (END) [ TOTAL: {stopwatch.Elapsed.TotalSeconds:0.0000000} ]");
            Status = FactoryStatus.Finished;
        }

        /// <summary>
        /// Handler to registered modules
        /// </summary>
        /// <returns></returns>
        public IReadOnlyCollection<ControllerConfigItem> ControllersHandler()
        {
            return controllersHandlerActive ? Config.GetControllerItemsList() : new List<ControllerConfigItem>();
        }

        /// <summary>
        /// Console debug output
        /// </summary>
        /// <param name="message"></param>
        protected void WriteDebugMessage(string message)
        {
            if (!Debug) return;
            OnConsoleOutput?.Invoke(this, message);
            Console.WriteLine(message);
        }
    }
}