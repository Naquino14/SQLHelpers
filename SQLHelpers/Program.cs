using QueryHelpers;
using c = System.Console;

// tests

var query = new QueryBuilder()
    .CreateTable("Meme")
    .CreateAddIdentity("FunnyID", DType.INT, 1, 1)
    .CreateAddNVP("FunnyName", PType.VARCHAR, 50)
    .CreateAddNVP("FunnyContent", PType.VARCHAR, 2000)
    .CreateAddNVP("FunnyImgPath", PType.VARCHAR, 100)
    .CreateAddNVP("FunnyScore", DType.INT)
    .CreateAddNVP("FunnyIsCringe", DType.BIT);

c.WriteLine(query.Query);

var columns = new string[] { "FunnyName", "FunnyContent" };
var values = new string[] { "guh?", "reveal?!?!?" };

query = new QueryBuilder()
    .InsertIntoCol("Meme")
    .AddInsertColumn(columns)
    .AddInsertValue(values)
    .AddInsertValue("penis");

//c.WriteLine(query.Query);