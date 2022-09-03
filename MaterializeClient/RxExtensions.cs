using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reactive.Linq;
using NLog;

namespace MaterializeClient;

public static class MzClientExtensions
{
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

    public static IObservable<IEnumerable<MzDiff>> ProgressBatch(this IObservable<IMzUpdate> source)
    {
        // use publish/refcount otherwise the buffer marker observable continually resubscribes
        var sourceRef = source.Publish().RefCount();
        return sourceRef
            // group by progress markers
            .Buffer(() => sourceRef.Where(u => u is MzProgress))
            // Remove MzProgress marker, and cast to MzDiff
            .Select(batch =>
            {
                batch.RemoveAt(batch.Count - 1);
                return batch.Cast<MzDiff>();
            })
            // filter out empty batches. 
            .Where(c => c.Any());
    }

    public static IObservable<MultiSet<MzData>> ScanToMultiSet(this IObservable<IEnumerable<IMzUpdate>> source)
    {
        return source.Scan(new MultiSet<MzData>(), (acc, current) =>
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