using System.Collections.Generic;

namespace MaterializeClient;

public class MultiSet<TKey> : Dictionary<TKey, int>
{
    public new int this[TKey key]
    {
        get
        {
            if (!TryGetValue(key, out var val))
            {
                val = 0;
                Add(key, val);
            }

            return val;
        }
        set => base[key] = value;
    }

    // todo: what are the correct equality members?
    // protected bool Equals(MultiSet<TKey> other)
    // {
    //     if (Count != other.Count)
    //         return false;
    //     if (Keys.Except(other.Keys).Any())
    //         return false;
    //     if (other.Keys.Except(Keys).Any())
    //         return false;
    //     return this.All(pair => pair.Value.Equals(other[pair.Key]));
    // }
    //
    // public override bool Equals(object? obj)
    // {
    //     if (ReferenceEquals(null, obj)) return false;
    //     if (ReferenceEquals(this, obj)) return true;
    //     if (obj.GetType() != this.GetType()) return false;
    //     return Equals((MultiSet<TKey>)obj);
    // }
    
}