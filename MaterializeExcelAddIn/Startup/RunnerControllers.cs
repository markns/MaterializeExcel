using System.Reflection;
using AddinX.Bootstrap.Contract;
using Autofac;

namespace MaterializeExcelAddIn.Startup
{
    internal class RunnerControllers : IRunner
    {
        public void Execute(IRunnerMain bootstrap)
        {
            var thisAssembly = Assembly.GetExecutingAssembly();

            var bootstrapper = (Bootstrapper)bootstrap;
            bootstrapper?.Builder.RegisterAssemblyTypes(thisAssembly)
                .Where(t => t.Name.ToLower().EndsWith("controller"));
        }
    }
}