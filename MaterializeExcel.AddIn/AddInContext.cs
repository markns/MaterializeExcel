using System.Threading;
using Autofac;
using MaterializeExcel.AddIn.Controller;
using NetOffice.ExcelApi;

namespace MaterializeExcel.AddIn
{
    internal static class AddInContext
    {
        private static MainController _mainController;

        public static CancellationTokenSource TokenCancellationSource { get; set; }
        public static IContainer Container { get; set; }
        public static Application ExcelApp { get; set; }

        public static MainController MainController =>
            _mainController ?? (_mainController = Container.Resolve<MainController>());
    }
}