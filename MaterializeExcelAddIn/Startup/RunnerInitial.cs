
using AddinX.Bootstrap.Contract;
using Autofac;
using NLog;

namespace MaterializeExcelAddIn.Startup
{
    internal class RunnerInitial : IRunner
    {
        public void Execute(IRunnerMain bootstrap)
        {
            var bootstrapper = (Bootstrapper)bootstrap;

            // Excel Application
            bootstrapper?.Builder.RegisterInstance(AddinContext.ExcelApp).ExternallyOwned();

            // Ribbon
            bootstrapper?.Builder.RegisterInstance(new AddinRibbon());

            // ILogger
            bootstrapper?.Builder.RegisterInstance<ILogger>(LogManager.GetLogger("MaterializeExcel"));

            // Event Aggregator
            // bootstrapper?.Builder.RegisterInstance<IEventAggregator>(new EventAggregator());

        }
    }
}