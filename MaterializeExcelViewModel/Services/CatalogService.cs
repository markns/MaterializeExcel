using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using DynamicData;
using MaterializeClient;
using NLog;
using ReactiveUI;

namespace MaterializeExcelViewModel.Services
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
                // .SubscribeOn(RxApp.TaskpoolScheduler)
                .ProgressBatch()
                .SelectMany(z => z)
                .Subscribe(diff =>
                    {
                        // TODO: also need to handle multiple databases
                        var schemaData = new CatalogResultRow(diff.Data);

                        if (diff.Multiplicity > 0)
                        {
                            // Logger.Info($"adding catalog node {schemaData.DatabaseNode.Name}|" +
                                        // $"{schemaData.SchemaNode.Name}|{schemaData.ObjectNode.Name}|" +
                                        // $"{schemaData.ColumnNode.Name}");

                            var additions = new List<ICatalogNode>();
                            additions.AddRange(new ICatalogNode[]
                                {
                                    schemaData.DatabaseNode,
                                    schemaData.SchemaNode,
                                    schemaData.ObjectNode,
                                    schemaData.ColumnNode,
                                }
                                .Where(node => !_catalogNodes.Lookup(node.Id).HasValue));
                            // Logger.Info($"new nodes {additions.Count}");

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

        // public void Promote(EmployeeDto promtedDto, int  newBoss)
        // {
        //     //in the real world, go to service then update the cache
        //
        //     //update the cache with the emploee, 
        //     _employees.AddOrUpdate(new EmployeeDto(promtedDto.Id,promtedDto.Name,newBoss));
        // }
        //
        //
        // public void Sack(EmployeeDto sackEmp)
        // {
        //     //in the real world, go to service then updated the cache
        //
        //     _employees.Edit(updater =>
        //     {
        //         //assign new boss to the workers of the sacked employee
        //         var workersWithNewBoss = updater.Items
        //                             .Where(emp => emp.BossId == sackEmp.Id)
        //                             .Select(dto => new EmployeeDto(dto.Id, dto.Name,  sackEmp.BossId))
        //                             .ToArray();
        //
        //         updater.AddOrUpdate(workersWithNewBoss);
        //
        //         //get rid of the existing person
        //         updater.Remove(sackEmp.Id);
        //     });
        //
        //
        // }

        // private IEnumerable<EmployeeDto> CreateEmployees(int numberToLoad)
        // {
        //     var random = new Random();
        //
        //     return Enumerable.Range(1, numberToLoad)
        //         .Select(i =>
        //         {
        //             var boss = i%1000 == 0 ? 0 : random.Next(0, i);
        //             return new EmployeeDto(i, $"Person {i}", boss);
        //         });
        // }
    }
}