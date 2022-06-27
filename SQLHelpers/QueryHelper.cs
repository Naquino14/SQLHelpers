namespace QueryHelpers
{    
    public class QueryBuilder
    {
        
        
        public string Query { get; private set; }
        public string? Table { get; set; }

        public QueryBuilder() { this.Query = ""; }

        public QueryBuilder(string table) : this() => this.Table = table;
        
        public QueryBuilder Select(string data)
        {
            this.Query += $"SELECT {data} ";
            return this;
        }

        public QueryBuilder From() => From(Table ?? throw new QueryPropertyNullException("Table"));
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

        public QueryBuilder Update(string data) => Update(Table ?? throw new QueryPropertyNullException("Table"), data);
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

        public QueryBuilder InsertIntoCol() => InsertIntoCol(Table ?? throw new QueryPropertyNullException("Table"));
        public QueryBuilder InsertIntoCol(string table)
        {
            this.Table = table;
            this.Query += $"INSERT INTO {table} ( ) VALUES ( ";
            return this;
        }

        public QueryBuilder InsertInto()
        {
            this.Query += $"INSERT INTO {Table} VALUES ( ";
            return this;
        }

        public QueryBuilder AddInsertColumn(string name)
        {
            this.Query = $"{this.Query[..^11]}'{name}', ) VALUES ( ";
            return this;
        }

        public QueryBuilder AddInsertColumn(string[] columns)
        {
            this.Query = $"{this.Query[..^11]}'{string.Join("', '", columns)}', ) VALUES ( ";
            return this;
        }
        
        public QueryBuilder AddInsertValue(string value)
        {
            this.Query = $"{(this.Query[^2] == ')' ? this.Query[..^2] : this.Query)}{value}, ) ";
            return this;
        }

        public QueryBuilder AddInsertValue(string[] values)
        {
            this.Query = $"{(this.Query[^2] == ')' ? this.Query[..^2] : this.Query)}{string.Join(", ", values)}, ) ";
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
}
