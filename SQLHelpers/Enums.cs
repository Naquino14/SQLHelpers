namespace SQLHelpers
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

    public enum Op
    {
        EQAL,
        GTHAN,
        LTHAN,
        GTEQ,
        LTEQ,
        NEQAL,
        BETWEEN,
        LIKE,
        IN
    }

    internal class OpTranslator
    {
        public static string Translate(Op op) => op switch
        {
            Op.EQAL => "=",
            Op.GTHAN => ">",
            Op.LTHAN => "<",
            Op.GTEQ => ">=",
            Op.LTEQ => "<=",
            Op.NEQAL => "<>",
            Op.BETWEEN => "BETWEEN",
            Op.LIKE => "LIKE",
            Op.IN => "IN",
            _ => throw new ArgumentOutOfRangeException(nameof(op), op, null)
        };
    }
}