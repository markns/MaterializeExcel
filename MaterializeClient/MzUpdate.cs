using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;

namespace MaterializeClient;

public interface IMzUpdate
{
    long Timestamp { get; }
}

public class MzProgress : IMzUpdate
{
    public long Timestamp { get; }

    public MzProgress(long timestamp)
    {
        Timestamp = timestamp;
    }

    public override string ToString()
    {
        return $"{nameof(Timestamp)}: {Timestamp}";
    }
}

public class MzDiff : IMzUpdate
{
    public long Timestamp { get; }
    public int Multiplicity { get; }
    public MzData Data { get; }

    public MzDiff(long timestamp, int multiplicity, IEnumerable<DbColumn> dbColumns, object[] columns)
    {
        Timestamp = timestamp;
        Multiplicity = multiplicity;
        Data = new MzData(dbColumns, columns);
    }

    public override string ToString()
    {
        return $"{nameof(Timestamp)}: {Timestamp}, {nameof(Multiplicity)}: {Multiplicity}, {nameof(Data)}: {Data}";
    }
}

public class MzData
{
    public MzData(IEnumerable<DbColumn> dbColumns, object[] values)
    {
        DbColumns = dbColumns;
        Values = values;
    }

    public IEnumerable<DbColumn> DbColumns { get; }
    public object[] Values { get; }

    protected bool Equals(MzData other)
    {
        return Values.SequenceEqual(other.Values);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((MzData)obj);
    }

    public override int GetHashCode() => ((IStructuralEquatable)Values).GetHashCode(EqualityComparer<object>.Default);

    public override string ToString()
    {
        return $"{nameof(Values)}: {string.Join("|", Values)}";
    }
}