using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using MaterializeExcel.AddIn.Startup.Contract;

namespace MaterializeExcel.AddIn.Startup.Autofac
{
    public class AutofacRunnerMain : IRunnerMain
    {
        private readonly CancellationToken _token;
        public ContainerBuilder Builder { get; }

        private IContainer _container;

        protected AutofacRunnerMain(CancellationToken token)
        {
            _token = token;
            Builder = new ContainerBuilder();
        }

        public virtual void Start()
        {
            Task.Factory.StartNew(ExecuteAll, _token);
        }

        public virtual void ExecuteAll()
        {
            var runnerInterface = typeof(IRunner);

            var types = Assembly.GetCallingAssembly().GetTypes()
                .Where(p => runnerInterface.IsAssignableFrom(p)
                            && p != runnerInterface);

            foreach (var instance in types.Select(
                         t => (IRunner)Activator.CreateInstance(t)))
            {
                instance.Execute(this);
            }

            _container = Builder.Build();
        }

        protected IContainer GetContainer()
        {
            return _container;
        }
    }
}