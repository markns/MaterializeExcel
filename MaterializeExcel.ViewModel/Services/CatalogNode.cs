using System;
using System.Security.Cryptography.X509Certificates;

namespace MaterializeExcel.ViewModel.Services
{
    public interface ICatalogNode
    {
        string Id { get; }
        string OwnerId { get; }
        string Name { get; }
    }

    public abstract class CatalogNode : ICatalogNode, IEquatable<CatalogNode>
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
        
        #region Equality Members

        public bool Equals(CatalogNode other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Id == other.Id;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((CatalogNode) obj);
        }

        public override int GetHashCode()
        {
            return (Id != null ? Id.GetHashCode() : 0);
        }

        public static bool operator ==(CatalogNode left, CatalogNode right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(CatalogNode left, CatalogNode right)
        {
            return !Equals(left, right);
        }
        #endregion
    }

    public class DatabaseNode : CatalogNode
    {
        public DatabaseNode(string id, string ownerId, string name) : base(id, ownerId, name)
        {
        }
    }

    public class SchemaNode : CatalogNode
    {
        public SchemaNode(string id, string ownerId, string name) : base(id, ownerId, name)
        {
        }
    }

    public class ObjectNode : CatalogNode
    {
        public ObjectNode(string id, string ownerId, string database, string schema, string name) : base(id, ownerId, name)
        {
            Database = database;
            Schema = schema;
        }

        public string Schema { get; }

        public string Database { get; }
    }

    public class ColumnNode : CatalogNode
    {
        public ColumnNode(string id, string ownerId, string name, long position, bool nullable, string type) : base(id,
            ownerId, name)
        {
            Position = position;
            Nullable = nullable;
            Type = type;
        }
        
        public long Position { get; }
        public bool Nullable { get; }
        public string Type { get; }

    }
}