using System;
using System.Threading;
using ExcelDna.Integration;
using ExcelDna.IntelliSense;
using ExcelDna.Logging;
using ExcelDna.Registration;
using MaterializeExcelAddIn.Startup;
// using MaterializeExcelAddIn.Startup;
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
                // Token cancellation is useful to close all existing Tasks<> before leaving the application
                AddinContext.TokenCancellationSource = new CancellationTokenSource();

                // The Excel Application object
                AddinContext.ExcelApp = new Application(null, ExcelDnaUtil.Application);
                
                // Start the bootstrapper now
                new Bootstrapper(AddinContext.TokenCancellationSource.Token).Start();
                
                ExcelRegistration.GetExcelFunctions()
                    .ProcessAsyncRegistrations()
                    // .ProcessParamsRegistrations()
                    .RegisterFunctions();

                IntelliSenseServer.Install();
                
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