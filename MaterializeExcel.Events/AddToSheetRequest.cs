namespace MaterializeExcel.Events
{
    public class AddToSheetRequest
    {
        public string DatabaseName { get; }
        public string SchemaName { get; }
        public string ObjectName { get; }

        public AddToSheetRequest(string databaseName, string schemaName, string objectName)
        {
            DatabaseName = databaseName;
            SchemaName = schemaName;
            ObjectName = objectName;
        }
    }
}