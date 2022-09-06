using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;
using NLog;
using Npgsql;

namespace MaterializeClient;

public class MzClient
{
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

    public MzClient(string host, int port, string database, string user)
    {
        Host = host;
        Port = port;
        Database = database;
        User = user;
    }

    private string Host { get; }
    private int Port { get; }
    private string Database { get; }
    private string User { get; }

    private async Task<NpgsqlConnection> OpenConnection()
    {
        Logger.Info($"Opening connection to {Host}:{Port}");

        // todo: connection pooling?

        var conn = new NpgsqlConnection($"host={Host};port={Port};database={Database};username={User}");
        await conn.OpenAsync();
        return conn;
    }

    public IObservable<IMzUpdate> Tail(
        string query,
        bool withProgress = true,
        bool withSnapshot = true,
        int? batchSize = null,
        int? timeout = null)
    {
        return Observable.Create<IMzUpdate>(async (observer, cancellationToken) =>
            {
                await using var conn = await OpenConnection();
                await using var txn = conn.BeginTransaction();

                var cmdString = $"DECLARE c CURSOR FOR TAIL {WrapSelectStmt(query)} WITH (" +
                                $"PROGRESS = {withProgress.ToString()}, " +
                                $"SNAPSHOT = {withSnapshot.ToString()})";

                await new NpgsqlCommand(cmdString, conn).ExecuteNonQueryAsync(cancellationToken);

                var fetch = batchSize.HasValue ? batchSize.ToString() : "ALL";
                var timeoutClause = timeout.HasValue ? $"WITH(timeout = '{timeout}s)" : "";

                using var fetchCmd = new NpgsqlCommand($"FETCH {fetch} c {timeoutClause}", conn, txn);

                
                while (!cancellationToken.IsCancellationRequested)
                {
                    await using var reader = await fetchCmd.ExecuteReaderAsync(cancellationToken);
                        
                    // todo: shouldn't execute this command every loop
                    IEnumerable<DbColumn> columns = await reader.GetColumnSchemaAsync(cancellationToken);                
                    while (!cancellationToken.IsCancellationRequested && await reader.ReadAsync(cancellationToken))
                    {
                        var mzUpdate = ReadMzUpdate(reader, columns, withProgress);
                        observer.OnNext(mzUpdate);
                    }
                }

                observer.OnCompleted();

                return Disposable.Create(() =>
                {
                    Logger.Info($"Observer has unsubscribed");
                });
            } 
        );
    }

    private static IMzUpdate ReadMzUpdate(IDataRecord reader, IEnumerable<DbColumn> dbColumns, bool withProgress)
    {
        var ordinal = 0;

        var mzTimestamp = reader.GetInt64(ordinal++);
        if (withProgress && reader.GetBoolean(ordinal++))
        {
            return new MzProgress(mzTimestamp);
        }

        var array = new object[reader.FieldCount];
        reader.GetValues(array);

        var mzDiff = reader.GetInt32(ordinal++);

        return new MzDiff(mzTimestamp,
            mzDiff,
            dbColumns.Skip(ordinal),
            // TODO: investigate ArraySegment or https://github.com/henon/SliceAndDice to prevent copy
            SliceMe(array, ordinal, reader.FieldCount - ordinal)
        );
    }

    private static object[] SliceMe(object[] source, int offset, int length)
    {
        var copy = new object[length];
        Array.Copy(source, offset, copy, 0, length);
        return copy;
    }

    private static string WrapSelectStmt(string query)
    {
        // TAIL accepts either an object name (source, table, view) or a SELECT statement.
        // If using a SELECT statement it is required to wrap this in parentheses.
        return query.ToUpper().Trim().StartsWith("SELECT") ? $"({query})" : query;
    }
}