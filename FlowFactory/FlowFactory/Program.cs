using FlowFactory.Config;
using FlowFactory.Container;
using System;

namespace FlowFactory
{
    /// <summary>
    /// Example context model
    /// </summary>
    public class MyContextModel
    {
        public object SomeObject => new object();

        public object AnotherObject => new object();
    }

    /// <summary>
    /// Example config
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MyConfig<T> : ContainerConfig<T>
    {
        public MyConfig(T model) : base(model) {  }

        /// <summary>
        /// Your own implementation
        /// </summary>
        public override void Preprocessing()
        {
            var model = ContextModel;
            //TODO
        }

        /// <summary>
        /// Your own implementation
        /// </summary>
        public override void Postprocessing()
        {
            var model = ContextModel;
            //TODO
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var config = new MyConfig<MyContextModel>(new MyContextModel());
            config.AddItem
            (
                new ControllerConfigItem
                {
                    ControllerType = typeof(ArgumentException),
                },
                new ControllerConfigItem
                {
                    ControllerType = typeof(AnotherExampleController),
                }
            ); 

            var container = new Container.Container(config);
            container.OnContainerStatusChange += Container_OnContainerStatusChange;
            container.OnControllerRunFailed += Container_OnControllerRunFailed;
            container.OnControllerInitFailed += Container_OnControllerInitFailed;
            container.OnControllerRegistered += Container_OnControllerRegistered;

            container.Init();
            container.Run();
        }

        private static void Container_OnControllerRegistered(object sender, System.EventArgs e)
        {
            Console.WriteLine("controller registered!!!!!!!!!!!!!!");
        }

        private static void Container_OnControllerInitFailed(object sender, ContainerExceptionEventArgs e)
        {
            Console.WriteLine("controller failed!!!!!!!!!!!!!!");
        }

        private static void Container_OnControllerRunFailed(object sender, ContainerExceptionEventArgs e)
        {
            e.Cancel = true;
        }

        private static void Container_OnContainerStatusChange(object sender, ContainerStatusEventArgs e)
        {
            //Console.WriteLine(e.Status);
        }
    }
}
