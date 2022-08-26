using System;
using ExcelDna.Integration;
using MaterializeClient;
using NLog;

namespace MaterializeExcelAddIn
{
    
    // Initialization [Error] Method not registered - unsupported signature, abstract or generic: 'MaterializeExcelFunctions.MZ_TAIL'

    public static class MaterializeExcelFunctions
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        
        [ExcelFunction(Description = "Materialize consumer")]
        public static IObservable<object> MZ_TAIL(string query)
        {
            Logger.Info($"Creating new materialize client");
            
            var mzClient = new MzClient("192.168.178.254", 6875,
                "materialize", "materialize");

            // const string query = "SELECT * FROM profile_views_enriched where owner_id = 25 order by received_at desc";

            return mzClient.Tail(query)
                .ProgressBatch()
                .MultiSetTo2DArray();
        }
    }
}