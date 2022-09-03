using System;
using System.Linq;
using MaterializeClient;

namespace MaterializeExcelViewModel.Services
{
    public class CatalogResultRow
    {
        public CatalogResultRow(MzData data)
        {
            var resultRow = data.DbColumns.Zip(data.Values, (column, o) => new { column, o })
                .ToDictionary(x => x.column.ColumnName, x => x.o);

            DatabaseNode =
                new DatabaseNode(resultRow["database_oid"].ToString(), "-1", (string)resultRow["database"]);

            SchemaNode =
                new SchemaNode(resultRow["schema_oid"].ToString(), resultRow["database_oid"].ToString(),
                    (string)resultRow["schema"]);

            ObjectNode =
                new ObjectNode(resultRow["object_oid"].ToString(), resultRow["schema_oid"].ToString(),
                    (string)resultRow["object_name"]);

            ColumnNode =
                new ColumnNode($"{resultRow["object_oid"]}:{resultRow["column"]}", resultRow["object_oid"].ToString(),
                    (string)resultRow["column"], (long)resultRow["position"], (bool)resultRow["nullable"],
                    (string)resultRow["type"]);
        }

        public DatabaseNode DatabaseNode { get; }

        public SchemaNode SchemaNode { get; }

        public ObjectNode ObjectNode { get; }

        public ColumnNode ColumnNode { get; }
    }
}