using AddinX.Bootstrap.Contract;
using AddinX.Wpf.Contract;
using AddinX.Wpf.Implementation;
using Autofac;
using MaterializeExcelAddIn.Manipulation;

namespace MaterializeExcelAddIn.Startup
{
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