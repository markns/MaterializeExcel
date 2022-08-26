using System;
using System.Data.Common;
using System.Linq;
using System.Reactive.Linq;
using NLog;

namespace MaterializeClient;

public static class MzClientExtensions
{
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

    public static IObservable<MultiSet<MzData>> ProgressBatch(this IObservable<IMzUpdate> source)
    {
        return source
            .Do(update => Logger.Trace($"update {update}"))
            // group by progress markers
            .Buffer(() => source.Where(u => u is MzProgress))
            // filter out empty batches. There will always be one MzProgress
            .Do(updates => Logger.Trace($"batch size {updates.Count}"))
            .Scan(new MultiSet<MzData>(), (acc, current) =>
            {
                foreach (var mzUpdate in current)
                {
                    if (mzUpdate is MzDiff mzDiff)
                    {
                        acc[mzDiff.Data] += mzDiff.Multiplicity;
                    }
                }

                return acc;
            });
    }

    public static IObservable<object[,]> MultiSetTo2DArray(this IObservable<MultiSet<MzData>> source)
    {
        // todo: try and reuse existing array here. For now, just create new each batch

        return source.Select(ms =>
            {
                var rowCount = ms.Values.Sum();
                var colCount = ms.First().Key.Values.Length;

                var arr = new object[rowCount + 1, colCount];

                // set column names as first row in result array                 
                var dbColumns = ms.First().Key.DbColumns;
                var columns = dbColumns as DbColumn[] ?? dbColumns.ToArray();
                for (var i = 0; i < columns.Length; i++)
                {
                    arr[0, i] = columns[i].ColumnName;
                }
                
                var row = 1;
                foreach (var pair in ms)
                {
                    for (var i = 0; i < pair.Value; i++)
                    {
                        // Buffer.BlockCopy(f, 0, g, 0, f.Length*sizeof(float)); https://stackoverflow.com/a/33030421
                        // Buffer.BlockCopy seems only to work for arrays based on primitive types, not e.g. object[].
                        // https://stackoverflow.com/a/33182069
                        for (var j = 0; j < pair.Key.Values.Length; j++)
                        {
                            arr[row, j] = pair.Key.Values[j];
                        }

                        row++;
                    }
                }

                return arr;
            }
        );
    }
}