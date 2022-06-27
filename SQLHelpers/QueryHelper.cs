namespace QueryHelpers
{    
    public class QueryBuilder
    {
        private string query;
        public string Query { get { return Resolve().query; } private set => query = value; }
        public string? Table { get; set; }

        public QueryBuilder() { query = ""; }

        public QueryBuilder(string table) : this() => Table = table;
        
        public QueryBuilder Select(string data)
        {
            query += $"SELECT {data} ";
            return this;
        }

        public QueryBuilder From() => From(Table ?? throw new QueryPropertyNullException("Table"));
        public QueryBuilder From(string table)
        {
            query += $"FROM {table} ";
            return this;
        }

        public QueryBuilder OrderBy(Order order)
        {
            query += $"ORDER BY {order switch { Order.Ascending => "ASC", Order.Descending => "DESC", _ => "ASC" }} ";
            return this;
        }

        public QueryBuilder Update() => Update(Table ?? throw new QueryPropertyNullException("Table"));
        public QueryBuilder Update(string table)
        {
            query += $"UPDATE {table} SET ";
            return this;
        }

        public QueryBuilder Set(string column, string value)
        {
            query = query.Substring(query.Length - 4, 3) == "SET" ? $"{query}{column} = {value} " : $", {query[..^2]}{column} = {value}, ";
            return this;
        }

        public QueryBuilder Where(string column, Op operatr, int value) => Where(column, operatr, $"{value}");
        public QueryBuilder Where(string column, Op operatr, string value)
        {
            query += $"WHERE {column} {OpTranslator.Translate(operatr)} {value} ";
            return this;
        }

        public QueryBuilder And(string column, Op operatr, int value) => And(column, operatr, $"{value}");
        public QueryBuilder And(string column, Op operatr, string value)
        {
            query += $"AND {column} {OpTranslator.Translate(operatr)} {value} ";
            return this;
        }

        public QueryBuilder Concat(QueryBuilder query)
        {
            this.query += query.Query;
            return this;
        }

        public QueryBuilder Concat(string query)
        {
            query = query[0] == ' ' ? query[1..] : query[^1] != ' ' ? query + ' ' : query; 
            this.query += query;
            return this;
        }

        public QueryBuilder CreateTable(string table)
        {
            Table = table;
            query += $"CREATE TABLE {table} ( ) ";
            return this;
        }

        public QueryBuilder CreateAddNVP(string name, PType type, int size)
        {
            query = $"{query[..^2]}{name} {type}({(size < 0 ? 0 : size == int.MaxValue ? "MAX" : size)}), ) ";
            return this;
        }
        
        public QueryBuilder CreateAddNVP(string name, DType type)
        {
            query = $"{query[..^2]}{name} {type}, ) ";
            return this;
        }

        public QueryBuilder CreateAddIdentity(string name, DType type, int seed, int start)
        {
            query = $"{query[..^2]}{name} {type} IDENTITY({seed}, {start}), ) ";
            return this;
        }

        public QueryBuilder InsertIntoCol() => InsertIntoCol(Table ?? throw new QueryPropertyNullException("Table"));
        public QueryBuilder InsertIntoCol(string table)
        {
            Table = table;
            query += $"INSERT INTO {table} ( ) VALUES ( ";
            return this;
        }

        public QueryBuilder InsertInto() => InsertInto(Table ?? throw new QueryPropertyNullException("Table"));
        public QueryBuilder InsertInto(string table)
        {
            query += $"INSERT INTO {table} VALUES ( ";
            return this;
        }

        public QueryBuilder AddInsertColumn(string name)
        {
            query = $"{query[..^11]}'{name}', ) VALUES ( ";
            return this;
        }

        public QueryBuilder AddInsertColumn(string[] columns)
        {
            query = $"{query[..^11]}{string.Join(", ", columns)}, ) VALUES ( ";
            return this;
        }

        public QueryBuilder AddInsertValue(int value)
        {
            query = $"{(query[^2] == ')' ? query[..^2] : query)}{value}, ) ";
            return this;
        }
        public QueryBuilder AddInsertValue(int[] values)
        {
            query = $"{(query[^2] == ')' ? query[..^2] : query)}{string.Join(", ", values)}, ) ";
            return this;
        }

        public QueryBuilder AddInsertValue(string value)
        {
            query = $"{(query[^2] == ')' ? query[..^2] : query)}'{value}', ) ";
            return this;
        }
        public QueryBuilder AddInsertValue(string[] values)
        {
            query = $"{(query[^2] == ')' ? query[..^2] : query)}'{string.Join("', '", values)}', ) ";
            return this;
        }

        public QueryBuilder Drop()
        {
            query += $"DROP TABLE {Table} ";
            return this;
        }

        public QueryBuilder Truncate()
        {
            query += $"TRUNCATE TABLE {Table} ";
            return this;
        }

        public QueryBuilder Resolve()
        {
            var split = query.Split(')');
            for (int i = 0; i < split.Length; i++)
                if (split[i].Length >= 2 && split[i][^2] == ',')
                    split[i] = split[i][..^2];
            query = string.Join(')', split);
            return this;
        }

        public override string ToString() => $"Query: {Query} {(Table is not null ? $" || Default table: {Table}" : "")}";
    }
}
