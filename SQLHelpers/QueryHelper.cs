namespace QueryHelpers
{    
    public class QueryBuilder
    {
        private string query;
        public string Query 
        { 
            get { return Resolve().query; } 
            private set => query = value; 
        }
        
        private string? table;
        public string? Table 
        { 
            get => table; 
            set => table = value == "" ? null : value; 
        }

        public QueryBuilder() { query = ""; }

        public QueryBuilder(string table) : this() => Table = table;

        public QueryBuilder Select()
        {
            query += $"SELECT ";
            return this;
        }
        public QueryBuilder Select(string column)
        {
            Select();
            query += $"{column} ";
            return this;
        }
        public QueryBuilder SelectDistinct()
        {
            query += $"SELECT DISTINCT ";
            return this;
        }
        public QueryBuilder SelectDistinct(string column)
        {
            query += $"SELECT DISTINCT {column} ";
            return this;
        }

        public QueryBuilder AddSelectColumn(string column)
        {
            query = $"{query[..^1]}{(query[^7..^1] == "SELECT" || query[^9..^1] == "DISTINCT" ? "" : ',')} {column} ";
            return this;
        }
        public QueryBuilder AddSelectColumn(string[] columns)
        {
            foreach (var column in columns)
                AddSelectColumn(column);
            return this;
        }

        public QueryBuilder Top(int number)
        {
            query += $"TOP {(number < 0 ? 0 : number)} ";
            return this;
        }
        public QueryBuilder TopPercent(int percent)
        {
            query += $"TOP {(percent < 0 ? 0 : percent)} PERCENT ";
            return this;
        }
        
        public QueryBuilder From() => From(Table ?? throw new QueryPropertyNullException("Table"));
        public QueryBuilder From(string table)
        {
            Table ??= table;
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
            Table ??= table;
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

        public QueryBuilder CreateTable() => CreateTable(Table ?? throw new QueryPropertyNullException("Table"));
        public QueryBuilder CreateTable(string table)
        {
            Table ??= table;
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
            Table ??= table;
            query += $"INSERT INTO {table} ( ) VALUES ( ";
            return this;
        }

        public QueryBuilder InsertInto() => InsertInto(Table ?? throw new QueryPropertyNullException("Table"));
        public QueryBuilder InsertInto(string table)
        {
            Table ??= table;
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

        public QueryBuilder Drop() => Drop(Table ?? throw new QueryPropertyNullException("Table"));
        public QueryBuilder Drop(string table)
        {
            Table ??= table;
            query += $"DROP TABLE {table} ";
            return this;
        }

        public QueryBuilder Truncate() => Truncate(Table ?? throw new QueryPropertyNullException("Table"));
        public QueryBuilder Truncate(string table)
        {
            Table ??= table;
            query += $"TRUNCATE TABLE {table} ";
            return this;
        }

        public QueryBuilder Delete() => Delete(Table ?? throw new QueryPropertyNullException("Table"));
        public QueryBuilder Delete(string table)
        {
            Table ??= table;
            query += $"DELETE FROM {table} ";
            return this;
        }

        public override string ToString() => $"Query: {Query} {(Table is not null ? $" || Default table: {Table}" : "")}";

        private QueryBuilder Resolve()
        {
            var commaFix = query.Split(')');
            for (int i = 0; i < commaFix.Length; i++)
                if (commaFix[i].Length >= 2 && commaFix[i][^2] == ',')
                    commaFix[i] = commaFix[i][..^2];
            query = string.Join(')', commaFix);
            // smart line breaks
            // TODO

            // smart select
            // TODO redo this 
            // copilot says use regex
            // var selectFix = query.Split("SELECT");
            // foreach (var e in selectFix)
            //     Console.WriteLine($"{e}|");
            
            // var postSelectKeywords = new string[] { "DISTINCT", "PERCENT", "TOP" };
            
            //foreach (var keyword in postSelectKeywords)
            //    for (int i = 0; i < selectFix.Length; i++)
            //        if (selectFix[i].Length > 0 && !selectFix[i].Contains('*') && selectFix[i].Contains(keyword)
            //            && selectFix[i].IndexOf("FROM") - (selectFix[i].IndexOf(keyword) + keyword.Length) <= 1)
            //            selectFix[i] = selectFix[i].Insert(selectFix[i].IndexOf(keyword) + keyword.Length, " *");

            
            // query = string.Join("SELECT", selectFix);
            return this;
        }
    }
}
