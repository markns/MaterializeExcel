namespace MaterializeExcel.Events
{
    public class AddToSheetEvent
    {
        public string ObjectName { get; }

        public AddToSheetEvent(string objectName)
        {
            ObjectName = objectName;
        }
    }
}