
using Autofac;
using MaterializeExcel.AddIn.Startup.Contract;

namespace MaterializeExcel.AddIn.Startup
{
    // ReSharper disable once UnusedType.Global
    internal class RunnerInitial : IRunner
    {
        public void Execute(IRunnerMain bootstrap)
        {
            var bootstrapper = (Bootstrapper)bootstrap;

            // Excel Application
            bootstrapper?.Builder.RegisterInstance(AddInContext.ExcelApp).ExternallyOwned();

            // Ribbon
            bootstrapper?.Builder.RegisterInstance(new AddinRibbon());

            // ILogger
            // bootstrapper?.Builder.RegisterInstance<ILogger>(new SerilogLogger());

            // Event Aggregator
            // bootstrapper?.Builder.RegisterInstance<IEventAggregator>(new EventAggregator());

        }
    }
}