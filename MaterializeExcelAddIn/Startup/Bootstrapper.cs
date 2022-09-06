using System;
using System.Threading;
using Autofac;
using ExcelDna.Integration;
using MaterializeExcelAddIn.Startup.Autofac;
using Splat.Autofac;

namespace MaterializeExcelAddIn.Startup
{
    public class Bootstrapper : AutofacRunnerMain
    {
        public Bootstrapper(CancellationToken token)
            : base(token)
        {
        }

        public override void Start()
        {
            ExcelAsyncUtil.QueueAsMacro(() => base.Start());
        }

        public override void ExecuteAll()
        {
            base.ExecuteAll();
            AddInContext.Container = GetContainer();

            var autofacResolver = AddInContext.Container.Resolve<AutofacDependencyResolver>();

            // Set a lifetime scope (either the root or any of the child ones) to Autofac resolver.
            // This is needed because Autofac became immutable since version 5+.
            // https://github.com/autofac/Autofac/issues/811
            autofacResolver.SetLifetimeScope(AddInContext.Container);
        }
    }
}