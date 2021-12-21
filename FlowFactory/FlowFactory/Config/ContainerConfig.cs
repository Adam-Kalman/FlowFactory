using FlowFactory.Container;
using FlowFactory.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FlowFactory.Config
{
    /// <summary>
    /// Container configurator abstract class with model as argument that will be passed through all the controllers
    /// </summary>
    public abstract class ContainerConfig<T> : IContainerConfig
    {
        /// <summary>
        /// Invoke when controller successfully run its constructor and was attached to module list
        /// </summary>
        public event EventHandler OnControllerRegistered;
        /// <summary>
        /// Invoke when controller thrown exception while running constructor
        /// </summary>
        public event EventHandler<ContainerExceptionEventArgs> OnControllerInitFailed;
        /// <summary>
        /// Definition of container controllers ACCESSIBILITY
        /// Active : true - controller starts normally
        /// Active : false - controller doesn't start, view will not be generated
        /// Visible : true - if controller is Active, view will be generated
        /// Visible : false - even if controller is Active, view will not be generated
        /// </summary>
        private List<ControllerConfigItem> ControllerItems { get; } = new List<ControllerConfigItem>();

        /// <summary>
        /// Model that will be passed to each Controller
        /// </summary>
        protected T ContextModel { get; set; }

        /// <summary>
        /// Set true when ModulesRegister was finished to prevent another item adding
        /// </summary>
        private bool itemsAlreadyRegistered;

        protected ContainerConfig(T model)
        {
            ContextModel = model;
        }

        /// <summary>
        /// Controllers accessibility configuration
        /// Based on some conditions here you can disable any of controller
        /// </summary>
        public virtual void SetActiveControllers()
        {
            //user implementation
        }

        /// <summary>
        /// Modules configuration list getter
        /// </summary>
        /// <returns></returns>
        public List<ControllerConfigItem> GetControllerItemsList()
        {
            return ControllerItems;
        }

        /// <summary>
        /// Create instance of all Controller items
        /// </summary>
        public void ControllerItemRegister()
        {
            foreach(var item in ControllerItems)
            {
                if (!item.Active) continue;

                try
                {
                    item.Instance = Activator.CreateInstance(item.ControllerType, ContextModel) as IContainerController;
                    if (item.Instance == null) throw new ArgumentNullException(nameof(item.Instance), "Controller must implement IContainerController interface.");
                    if (!(item.Instance is ControllerBase<T> @base)) throw new InvalidCastException("Controller must derived from ControllerBase<T>.");

                    @base.ControllerConfigItem = GetControllerItemsList();
                    item.IsInitialized = true;
                    OnControllerRegistered?.Invoke(item, EventArgs.Empty);
                }
                catch(Exception e)
                {
                    item.Active = false;
                    var args = new ContainerExceptionEventArgs(e);
                    OnControllerInitFailed?.Invoke(item, args);
                    if(args.Cancel)
                    {
                        throw new OperationCanceledException();
                    }
                }
            }

            itemsAlreadyRegistered = true;
        }

        /// <summary>
        /// Processing aggregation before first controller starts
        /// </summary>
        public virtual void Preprocessing()
        {
            //user implementation
        }

        /// <summary>
        /// Processing aggregation after last controller ends
        /// </summary>
        public virtual void Postprocessing()
        {
            //user implementation
        }

        /// <summary>
        /// Add <see cref="ControllerConfigItem"/> to list. 
        /// It's a FIFO list, first added controller will be run first
        /// </summary>
        /// <param name="item"></param>
        public void AddItem(ControllerConfigItem item)
        {
            if (itemsAlreadyRegistered) throw new ArgumentException("You cannot add items after running Container.InitControllerItems method", nameof(itemsAlreadyRegistered));
            ControllerItems.Add(item);
        }

        /// <summary>
        /// Add range of <see cref="ControllerConfigItem"/> items
        /// </summary>
        /// <param name="items"></param>
        public void AddItem(params ControllerConfigItem[] items)
        {
            items.ToList().ForEach(AddItem);
        }
    }
}