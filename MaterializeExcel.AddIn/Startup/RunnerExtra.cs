using Autofac;
using MaterializeClient;
using MaterializeExcel.AddIn.Manipulation;
using MaterializeExcel.AddIn.Properties;
using MaterializeExcel.AddIn.Startup.Contract;
using MaterializeExcel.ViewModel.Services;

namespace MaterializeExcel.AddIn.Startup
{
    // ReSharper disable once UnusedType.Global
    internal class RunnerExtra : IRunner
    {
        public void Execute(IRunnerMain bootstrap)
        {
            var bootstrapper = (Bootstrapper)bootstrap;
            bootstrapper?.Builder.RegisterType<ExcelInteraction>();
            
            bootstrapper?.Builder.RegisterInstance(GetMzClient());
            bootstrapper?.Builder.RegisterType<CatalogService>().SingleInstance();
            
        }

        private static MzClient GetMzClient()
        {
            return new MzClient(Settings.Default.Host,
                Settings.Default.Port,
                Settings.Default.Database,
                Settings.Default.User);
        }
    }
}