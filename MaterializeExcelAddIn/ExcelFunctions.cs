using System;
using ExcelDna.Integration;
using MaterializeClient;
using MaterializeExcelAddIn.Properties;
using NLog;
using NLog.Fluent;

namespace MaterializeExcelAddIn
{
    // Initialization [Error] Method not registered - unsupported signature, abstract or generic: 'MaterializeExcelFunctions.MZ_TAIL'

    public static class MaterializeExcelFunctions
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        [ExcelFunction(Description = "Stream updates from a materialize view or select statement")]
        public static IObservable<object> MZ_TAIL(
            [ExcelArgument(Name = "query",
                Description = "view or select statement", AllowReference = true)
            ] string query)
        {
            Logger.Info($"Requesting tail for query {query}");
            var mzClient = GetMzClient();
            return mzClient.Tail(query)
                .ProgressBatch()
                .ScanToMultiSet()
                .MultiSetTo2DArray();
        }

        private static MzClient GetMzClient()
        {
            return new MzClient(Settings.Default.Host,
                Settings.Default.Port,
                Settings.Default.Database,
                Settings.Default.User);
        }
    }
}