using System;
using System.Threading;
using ExcelDna.Integration;
using ExcelDna.IntelliSense;
using ExcelDna.Logging;
using ExcelDna.Registration;
using MaterializeExcelAddIn.Startup;
using NetOffice.ExcelApi;
using NLog;

namespace MaterializeExcelAddIn
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
                // Locator.CurrentMutable.InitializeReactiveUI();
                // Locator.CurrentMutable.InitializeSplat();
                
                // A helper method that will register all classes that derive off IViewFor 
                // into our dependency injection container. ReactiveUI uses Splat for it's 
                // dependency injection by default, but you can override this if you like.
                // Locator.CurrentMutable.RegisterViewsForViewModels(Assembly.GetCallingAssembly());                
                
                // Token cancellation is useful to close all existing Tasks<> before leaving the application
                AddInContext.TokenCancellationSource = new CancellationTokenSource();

                // The Excel Application object
                AddInContext.ExcelApp = new Application(null, ExcelDnaUtil.Application);

                // Start the bootstrapper now
                new Bootstrapper(AddInContext.TokenCancellationSource.Token).Start();
                
                ExcelRegistration.GetExcelFunctions()
                    .ProcessAsyncRegistrations()
                    // .ProcessParamsRegistrations()
                    .RegisterFunctions();

                IntelliSenseServer.Install();

                // setup error handler
                ExcelIntegration.RegisterUnhandledExceptionHandler(ex => "Unhandled Error: " + ex.ToString());
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