namespace QueryHelpers
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
}