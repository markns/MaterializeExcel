using System;
using System.Threading;
using ExcelDna.Integration;
using ExcelDna.IntelliSense;
using ExcelDna.Logging;
using ExcelDna.Registration;
using MaterializeExcel.AddIn.Startup;
using Microsoft.Office.Interop.Excel;
using NLog;

namespace MaterializeExcel.AddIn
{
    // ReSharper disable once UnusedType.Global
    public class AddIn : IExcelAddIn
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        // todo: check https://github.com/markns/quandl-excel-windows/ for some inspiration

        public void AutoOpen()
        {
            try
            {
                // Token cancellation is useful to close all existing Tasks<> before leaving the application
                AddInContext.TokenCancellationSource = new CancellationTokenSource();

                // The Excel Application object
                AddInContext.ExcelApp = (Application)ExcelDnaUtil.Application;

                // Start the bootstrapper now
                new Bootstrapper(AddInContext.TokenCancellationSource.Token).Start();

                ExcelRegistration.GetExcelFunctions()
                    .ProcessAsyncRegistrations()
                    // .ProcessParamsRegistrations()
                    .RegisterFunctions();

                IntelliSenseServer.Install();

                // setup error handler
                ExcelIntegration.RegisterUnhandledExceptionHandler(ex => $"Unhandled Error: {ex}");
            }
            catch (Exception e)
            {
                LogDisplay.RecordLine(e.Message);
                LogDisplay.RecordLine(e.StackTrace);
                LogDisplay.Show();
            }
        }

        public void AutoClose()
        {
            IntelliSenseServer.Uninstall();
        }
    }
}