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

            // const string query = "SELECT * FROM t";

            const string query =
                @"select coalesce(d.id, 0)          as database_id,
                         coalesce(d.oid, 0)         as database_oid,
                         coalesce(d.name, 'system') as database,
                         s.id                       as schema_id,
                         s.oid                      as schema_oid,
                         s.name                     as schema,
                         o.id                       as object_id,
                         o.oid                      as object_oid,
                         o.name                     as object_name,
                         c.id                       as column_id,
                         c.name                     as column,
                         c.position,
                         c.nullable,
                         c.type
                  from mz_databases d
                           right join mz_schemas s on d.id = s.database_id
                           join mz_objects o on s.id = o.schema_id
                           join mz_columns c on o.id = c.id
                  order by database, schema, object_id, position";

            var cache = new SourceCache<ICatalogNode, string>(x => x.Id);

            // batches.

            mzClient.Tail(query)
                .ProgressBatch()
                .SelectMany(z => z)
                .Subscribe(diff =>
                    {
                        // TODO: also need to handle multiple databases
                        var schemaData = new CatalogResultRow(diff.Data);

                        if (diff.Multiplicity > 0)
                        {
                            cache.AddOrUpdate(new ICatalogNode[]
                            {
                                schemaData.DatabaseNode,
                                schemaData.SchemaNode,
                                schemaData.ObjectNode,
                                schemaData.ColumnNode,
                            });
                        }
                        else if (diff.Multiplicity < 0)
                        {
                            Logger.Info($"Removing schema data {schemaData}");
                            // cache.Remove(schemaData);
                        }
                    }
                );


            bool DefaultPredicate(Node<ICatalogNode, string> node) => node.IsRoot;

            cache.Connect()
                .TransformToTree(node => node.OwnerId,
                    Observable.Return((Func<Node<ICatalogNode, string>, bool>)DefaultPredicate))
                .Subscribe(
                    changeSet => Console.WriteLine($"next: {changeSet}"),
                    ex => Console.WriteLine($"error: {ex.Message}"),
                    () => Console.WriteLine("finished")
                );
            Console.ReadLine();
        }
    }

    interface ICatalogNode
    {
        string Id { get; }
        string OwnerId { get; }
    }

    abstract class CatalogNode : ICatalogNode
    {
        protected CatalogNode(string id, string ownerId, string name)
        {
            Id = id;
            OwnerId = ownerId;
            Name = name;
        }

        public string Id { get; }
        public string OwnerId { get; }
        public string Name { get; }
    }

    class DatabaseNode : CatalogNode
    {
        public DatabaseNode(string id, string ownerId, string name) : base(id, ownerId, name)
        {
        }
    }

    class SchemaNode : CatalogNode
    {
        public SchemaNode(string id, string ownerId, string name) : base(id, ownerId, name)
        {
        }
    }

    class ObjectNode : CatalogNode
    {
        public ObjectNode(string id, string ownerId, string name) : base(id, ownerId, name)
        {
        }
    }

    internal class ColumnNode : CatalogNode
    {
        public long Position { get; }
        public bool Nullable { get; }
        public string Type { get; }

        public ColumnNode(string id, string ownerId, string name, long position, bool nullable, string type) : base(id,
            ownerId, name)
        {
            Position = position;
            Nullable = nullable;
            Type = type;
        }
    }

    class CatalogResultRow
    {
        private readonly Dictionary<string, object> _resultRow;

        public CatalogResultRow(MzData data)
        {
            _resultRow = data.DbColumns.Zip(data.Values, (column, o) => new { column, o })
                .ToDictionary(x => x.column.ColumnName, x => x.o);
        }

        public DatabaseNode DatabaseNode =>
            new DatabaseNode(_resultRow["database_id"].ToString(), "-1", (string)_resultRow["database"]);

        public SchemaNode SchemaNode =>
            new SchemaNode(_resultRow["schema_id"].ToString(), _resultRow["database_id"].ToString(),
                (string)_resultRow["schema"]);

        public ObjectNode ObjectNode =>
            new ObjectNode(_resultRow["object_id"].ToString(), _resultRow["schema_id"].ToString(),
                (string)_resultRow["object_name"]);

        public ColumnNode ColumnNode =>
            new ColumnNode($"{_resultRow["object_id"]}:{_resultRow["column"]}", _resultRow["object_id"].ToString(),
                (string)_resultRow["column"], (long)_resultRow["position"], (bool)_resultRow["nullable"],
                (string)_resultRow["type"]);
    }
}