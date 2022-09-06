using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using DynamicData;
using MaterializeClient;
using NLog;

namespace MaterializeExcelConsole
{
    internal class Program
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static void Main(string[] args)
        {
            var mzClient = new MzClient("Marks-MacBook-Pro.local", 6875, "materialize", "materialize");

            const string query = "SELECT * FROM t";

            // batches.

            var d = mzClient.Tail(query)
                .ProgressBatch()
                .Subscribe(
                    changeSet => Console.WriteLine($"next: {string.Join("|", changeSet)}"),
                    ex => Console.WriteLine($"error: {ex.Message}"),
                    () => Console.WriteLine("finished")
                );
            Console.ReadLine();

            Console.WriteLine("disposing subscription");
            d.Dispose();
            Console.WriteLine("disposed subscription");
            
            Console.ReadLine();
        }
    }

}