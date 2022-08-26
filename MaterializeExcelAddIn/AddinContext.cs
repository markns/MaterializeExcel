using System.Threading;
using Autofac;
using MaterializeExcelAddIn.Controller;
using NetOffice.ExcelApi;

namespace MaterializeExcelAddIn
{
    internal static class AddinContext
    {
        private static MainController ctrls;

        public static CancellationTokenSource TokenCancellationSource { get; set; }

        public static IContainer Container { get; set; }
        public static Application ExcelApp { get; set; }
        public static MainController MainController => ctrls ?? (ctrls = Container.Resolve<MainController>());
    }
}