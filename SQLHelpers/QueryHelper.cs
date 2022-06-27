namespace QueryHelper
{
    using QPN = QueryPropertyNullException;
    
    public class QueryBuilder
    {
        public enum Order
        {
            Ascending,
            Descending
        }

        /// <summary>
        /// Non parameterized data type
        /// </summary>
        public enum DType
        {
            TEXT,
            NCHAR,
            NVARCHAR,
            NTEXT,
            VARBINARY,
            IMAGE,
            BIT,
            TINYINT,
            SMALLINT,
            INT,
            BIGINT,
            SMALLMONEY,
            MONEY,
            REAL,
            DATETIME,
            DATETIME2,
            SMALLDATETIME,
            DATE,
            TIME,
            DATETIMEOFFSET,
            TIMESTAMP,
            SQL_VARIANT,
            UNIQUEIDENTIFIER,
            XML,
            CURSOR,
            TABLE

        }
        
        /// <summary>
        /// Parameterized data type
        /// </summary>
        public enum PType
        {
            CHAR,
            VARCHAR,
            BINARY,
            FLOAT
        }
        
        public string Query { get; private set; }
        public string? Table { get; set; }

        public QueryBuilder() { this.Query = ""; }

        public QueryBuilder(string table) : this() => this.Table = table;
        
        public QueryBuilder Select(string data)
        {
            this.Query += $"SELECT {data} ";
            return this;
        }

        public QueryBuilder From() => From(Table ?? throw new QPN());
        public QueryBuilder From(string table)
        {
            this.Query += $"FROM {table} ";
            return this;
        }

        public QueryBuilder OrderBy(Order order)
        {
            this.Query += $"ORDER BY {order switch { Order.Ascending => "ASC", Order.Descending => "DESC", _ => "ASC" }} ";
            return this;
        }

        public QueryBuilder Update(string table, string data)
        {
            this.Query += $"UPDATE {table} SET {data} ";
            return this;
        }

        public QueryBuilder Concat(QueryBuilder query)
        {
            this.Query += query.Query;
            return this;
        }

        public QueryBuilder Concat(string query)
        {
            query = query[0] == ' ' ? query[1..] : query[^1] != ' ' ? query + ' ' : query; 
            this.Query += query;
            return this;
        }

        public QueryBuilder CreateTable(string table)
        {
            this.Table = table;
            this.Query += $"CREATE TABLE {table} ( ) ";
            return this;
        }

        public QueryBuilder CreateAddNVP(string name, PType type, int size)
        {
            this.Query = $"{this.Query[..^2]}{name} {type}({(size < 0 ? 0 : size == int.MaxValue ? "MAX" : size)}), ) ";
            return this;
        }
        
        public QueryBuilder CreateAddNVP(string name, DType type)
        {
            this.Query = $"{this.Query[..^2]}{name} {type},  )";
            return this;
        }

        public QueryBuilder CreateAddIdentity(string name, DType type, int seed, int start)
        {
            this.Query = $"{this.Query[..^2]}{name} {type} IDENTITY({seed},{start}), ) ";
            return this;
        }

        public QueryBuilder Drop()
        {
            this.Query += $"DROP TABLE {Table} ";
            return this;
        }

        public QueryBuilder Truncate()
        {
            this.Query += $"TRUNCATE TABLE {Table} ";
            return this;
        }

        public override string ToString() => $"Query: {Query} {(Table is not null ? $" || Default table: {Table}" : "")}";

    }

    public class QueryPropertyNullException : Exception
    {
        public QueryPropertyNullException() : base() { }
        public QueryPropertyNullException(string propName) : base($"Property {propName} was null.") { }
        public override string ToString() => base.ToString();
    }
}
