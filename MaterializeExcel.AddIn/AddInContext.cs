using System.Threading;
using Autofac;
using MaterializeClient;
using MaterializeExcel.AddIn.Controller;
using Microsoft.Office.Interop.Excel;

namespace MaterializeExcel.AddIn
{
    internal static class AddInContext
    {
        private static MainController _mainController;
        private static MzClient _mzClient;

        public static CancellationTokenSource TokenCancellationSource { get; set; }
        public static IContainer Container { get; set; }
        public static Application ExcelApp { get; set; }

        public static MainController MainController =>
            _mainController ?? (_mainController = Container.Resolve<MainController>());

        public static MzClient MzClient =>
            _mzClient ?? (_mzClient = Container.Resolve<MzClient>());
    }
}