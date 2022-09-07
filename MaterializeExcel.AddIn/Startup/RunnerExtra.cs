using AddinX.Wpf.Contract;
using AddinX.Wpf.Implementation;
using Autofac;
using MaterializeExcel.AddIn.Manipulation;
using MaterializeExcel.AddIn.Startup.Contract;

namespace MaterializeExcel.AddIn.Startup
{
    // ReSharper disable once UnusedType.Global
    internal class RunnerExtra : IRunner
    {
        public void Execute(IRunnerMain bootstrap)
        {
            var bootstrapper = (Bootstrapper)bootstrap;
            bootstrapper?.Builder.RegisterType<ExcelInteraction>();

            bootstrapper?.Builder.RegisterType<ExcelDnaWpfHelper>().As<IWpfHelper>();
        }
    }
}