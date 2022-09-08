using Autofac;
using MaterializeExcel.AddIn.Startup.Contract;
using ReactiveUI;

namespace MaterializeExcel.AddIn.Startup
{
    // ReSharper disable once UnusedType.Global
    internal class RunnerInitial : IRunner
    {
        public void Execute(IRunnerMain bootstrap)
        {
            var bootstrapper = (Bootstrapper)bootstrap;

            // Excel Application
            // todo: this breaks autofac somehow.
            // bootstrapper?.Builder.RegisterInstance(AddInContext.ExcelApp).ExternallyOwned();

            // Ribbon
            bootstrapper?.Builder.RegisterInstance(new AddInRibbon());

            // ILogger
            // todo: inject logger?
            // bootstrapper?.Builder.RegisterInstance<ILogger>(new SerilogLogger());

            // Event Aggregator
            bootstrapper?.Builder.RegisterInstance(MessageBus.Current);
        }
    }
}