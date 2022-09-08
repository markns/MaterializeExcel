using System;
using ExcelDna.Integration;
using MaterializeClient;
using NLog;

namespace MaterializeExcel.AddIn
{
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
            return AddInContext.MzClient.Tail(query)
                .ProgressBatch()
                .ScanToMultiSet()
                .MultiSetTo2DArray();
        }

    }
}