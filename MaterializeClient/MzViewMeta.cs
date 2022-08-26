namespace MaterializeClient;

public class MzViewMeta
{
    public string Schema { get; set; }
    public string View { get; set; }
    public string Column { get; set; }
    public int Position { get; set; }
    public bool Nullable { get; set; }
    public string Type { get; set; }

    public override string ToString()
    {
        return $"{nameof(Schema)}: {Schema}, {nameof(View)}: {View}, " +
               $"{nameof(Column)}: {Column}, {nameof(Position)}: {Position}, " +
               $"{nameof(Nullable)}: {Nullable}, {nameof(Type)}: {Type}";
    }
}