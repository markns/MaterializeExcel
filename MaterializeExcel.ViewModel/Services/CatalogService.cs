using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using DynamicData;
using MaterializeClient;
using MaterializeExcel.ViewModel.Services;
using NLog;
using ReactiveUI;

namespace MaterializeExcel.ViewModel.Services
{
    public class CatalogService
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly SourceCache<ICatalogNode, string> _catalogNodes =
            new SourceCache<ICatalogNode, string>(x => x.Id);

        public CatalogService(MzClient mzClient)
        {
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

            mzClient.Tail(query)
                .SubscribeOn(RxApp.TaskpoolScheduler)
                .ProgressBatch()
                .SelectMany(z => z)
                .Subscribe(diff =>
                    {
                        // TODO: also need to handle multiple databases
                        var schemaData = new CatalogResultRow(diff.Data);

                        if (diff.Multiplicity > 0)
                        {
                            var additions = new List<ICatalogNode>();
                            additions.AddRange(new ICatalogNode[]
                                {
                                    schemaData.DatabaseNode,
                                    schemaData.SchemaNode,
                                    schemaData.ObjectNode,
                                    schemaData.ColumnNode,
                                }
                                .Where(node => !_catalogNodes.Lookup(node.Id).HasValue));

                            _catalogNodes.AddOrUpdate(additions);
                        }
                        else if (diff.Multiplicity < 0)
                        {
                            // TODO: handle removal of database objects
                            // remove where count == 1
                            Logger.Warn($"Unhandled removal of database object {schemaData}");
                            // cache.Remove(schemaData);
                        }
                    }
                );
        }

        public IObservableCache<ICatalogNode, string> CatalogNodes => _catalogNodes.AsObservableCache();
    }
}