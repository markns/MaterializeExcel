using Autofac;
using MaterializeExcel.AddIn.Startup.Contract;
using MaterializeExcel.View;
using MaterializeExcel.View.Nodes;
using MaterializeExcel.ViewModel;
using MaterializeExcel.ViewModel;
using MaterializeExcel.ViewModel.Nodes;
using ReactiveUI;
using Splat.Autofac;

namespace MaterializeExcel.AddIn.Startup
{
    // ReSharper disable once UnusedType.Global
    // ReSharper disable once InconsistentNaming
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
            autofacResolver?.InitializeReactiveUI();

            // If you need to override any service (such as the ViewLocator), register it after InitializeReactiveUI.
            // https://autofaccn.readthedocs.io/en/latest/register/registration.html#default-registrations
            // bootstrapper?.Builder.RegisterType<MyCustomViewLocator>().As<IViewLocator>().SingleInstance();
            
            // https://github.com/reactiveui/splat/blob/main/src/Splat.Autofac/README.md
            bootstrapper?.Builder.RegisterType<MainControl>().As<IViewFor<MainControlViewModel>>();
            bootstrapper?.Builder.RegisterType<DatabaseNodeView>().As<IViewFor<DatabaseNodeViewModel>>();
            bootstrapper?.Builder.RegisterType<SchemaNodeView>().As<IViewFor<SchemaNodeViewModel>>();
            bootstrapper?.Builder.RegisterType<ObjectNodeView>().As<IViewFor<TableNodeViewModel>>();
            bootstrapper?.Builder.RegisterType<ColumnNodeView>().As<IViewFor<ColumnNodeViewModel>>();
            bootstrapper?.Builder.RegisterType<MainControlViewModel>().AsSelf();
            bootstrapper?.Builder.RegisterType<CatalogViewModel>().AsSelf();
            bootstrapper?.Builder.RegisterType<TableNodeViewModel>().AsSelf();

        }
    }
}