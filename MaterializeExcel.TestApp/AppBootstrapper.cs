using Autofac;
using MaterializeClient;
using MaterializeExcel.View;
using MaterializeExcel.View.Nodes;
using MaterializeExcelViewModel;
using MaterializeExcelViewModel.Nodes;
using MaterializeExcelViewModel.Services;
using ReactiveUI;
using Splat;
using Splat.Autofac;

namespace MaterializeExcel.TestApp
{
    public class AppBootstrapper
    {
        public void Run()
        {
            var builder = new ContainerBuilder();

            
            // Register the Adapter to Splat.
            // Creates and sets the Autofac resolver as the Locator.            
            var autofacResolver = builder.UseAutofacDependencyResolver();

            // Register the resolver in Autofac so it can be later resolved
            builder.RegisterInstance(autofacResolver);
            
            // Initialize ReactiveUI components
            autofacResolver.InitializeReactiveUI();
            
            // If you need to override any service (such as the ViewLocator), register it after InitializeReactiveUI.
            // https://autofaccn.readthedocs.io/en/latest/register/registration.html#default-registrations
            // builder.RegisterType<MyCustomViewLocator>().As<IViewLocator>().SingleInstance();

            var mzClient = new MzClient("Marks-MacBook-Pro.local", 6875,
                "materialize", "materialize");
            builder.RegisterInstance(mzClient);
            builder.RegisterType<CatalogService>().AsSelf();
            
            // https://github.com/reactiveui/splat/blob/main/src/Splat.Autofac/README.md
            builder.RegisterType<MainControl>().As<IViewFor<MainControlViewModel>>();
            builder.RegisterType<MainWindow>().As<IViewFor<MainWindowViewModel>>();
            builder.RegisterType<DatabaseNodeView>().As<IViewFor<DatabaseNodeViewModel>>();
            builder.RegisterType<SchemaNodeView>().As<IViewFor<SchemaNodeViewModel>>();
            builder.RegisterType<ObjectNodeView>().As<IViewFor<ObjectNodeViewModel>>();
            builder.RegisterType<ColumnNodeView>().As<IViewFor<ColumnNodeViewModel>>();
            builder.RegisterType<MainWindowViewModel>().AsSelf();
            builder.RegisterType<MainControlViewModel>().AsSelf();
            builder.RegisterType<CatalogViewModel>().AsSelf();
            builder.RegisterType<ObjectNodeViewModel>().AsSelf();
            
            var container = builder.Build();

            // autofacResolver = container.Resolve<AutofacDependencyResolver>();

            // Set a lifetime scope (either the root or any of the child ones) to Autofac resolver.
            // This is needed because Autofac became immutable since version 5+.
            // https://github.com/autofac/Autofac/issues/811
            autofacResolver.SetLifetimeScope(container);

            //
            // RxAppAutofacExtension.UseAutofacDependencyResolver(builder.Build());
            var view = (MainWindow)Locator.Current.GetService(typeof (IViewFor<MainWindowViewModel>));
            view.Show();
        }

    }
}