using Autofac;
using MaterializeExcel.View;
using MaterializeExcel.ViewModel;
using MaterializeExcelAddIn.Startup.Contract;
using MaterializeExcel.ViewModel;
using ReactiveUI;
using Splat.Autofac;

namespace MaterializeExcelAddIn.Startup
{
    internal class RunnerUI : IRunner
    {
        public void Execute(IRunnerMain bootstrap)
        {
            var bootstrapper = (Bootstrapper)bootstrap;

            // Register the Adapter to Splat.
            // Creates and sets the Autofac resolver as the Locator.            
            var autofacResolver = bootstrapper?.Builder.UseAutofacDependencyResolver();

            // Register the resolver in Autofac so it can be later resolved
            bootstrapper?.Builder.RegisterInstance(autofacResolver);
            //
            // Initialize ReactiveUI components
            autofacResolver.InitializeReactiveUI();
            
            // If you need to override any service (such as the ViewLocator), register it after InitializeReactiveUI.
            // https://autofaccn.readthedocs.io/en/latest/register/registration.html#default-registrations
            // builder.RegisterType<MyCustomViewLocator>().As<IViewLocator>().SingleInstance();
            
            // https://github.com/reactiveui/splat/blob/main/src/Splat.Autofac/README.md
            bootstrapper?.Builder.RegisterType<MainControl>().As<IViewFor<MainControlViewModel>>();
            
            // Ribbon
            // bootstrapper?.Builder.RegisterInstance(new AddinRibbon());

            // ILogger
            // bootstrapper?.Builder.RegisterInstance<ILogger>(new SerilogLogger());

            // Event Aggregator
            // bootstrapper?.Builder.RegisterInstance<IEventAggregator>(new EventAggregator());
        }
    }
}