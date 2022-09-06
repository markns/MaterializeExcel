using System.Reflection;
using Autofac;
using MaterializeExcelAddIn.Startup.Contract;

namespace MaterializeExcelAddIn.Startup
{
    // ReSharper disable once UnusedType.Global
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