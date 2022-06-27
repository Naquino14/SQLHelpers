using QueryHelper;
using QB = QueryHelper.QueryBuilder;
using c = System.Console;

// tests

var query = new QB()
    .CreateTable("Meme")
    .CreateAddIdentity("FunnyID", QB.DType.INT, 1, 1)
    .CreateAddNVP("FunnyName", QB.PType.VARCHAR, 50)
    .CreateAddNVP("FunnyContent", QB.PType.VARCHAR, 2000)
    .CreateAddNVP("FunnyImgPath", QB.PType.VARCHAR, 100)
    .CreateAddNVP("FunnyScore", QB.DType.INT)
    .CreateAddNVP("FunnyIsCringe", QB.DType.BIT);

c.WriteLine(query.Query);