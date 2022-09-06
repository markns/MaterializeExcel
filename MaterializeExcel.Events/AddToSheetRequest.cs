namespace MaterializeExcel.Events
{
    public class AddToSheetRequest
    {
        public string DatabaseName { get; }
        public string SchemaName { get; }
        public string ObjectName { get; }

        public AddToSheetRequest(string databaseName, string schemaName, string objectName)
        {
            SchemaName = schemaName;
            DatabaseName = databaseName;
            ObjectName = objectName;
        }
    }
}