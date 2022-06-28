using SQLHelpers;
using c = System.Console;

internal class Program
{
    internal static void Main()
    {
        // tests
        var columns = new string[] { "FunnyName", "FunnyContent" };
        var values = new string[] { "guh?", "reveal" };

        var query = new QueryBuilder()
            .CreateTable("Meme")
            .CreateAddIdentity("FunnyID", DType.INT, 1, 1)
            .CreateAddNVP("FunnyName", PType.VARCHAR, 50)
            .CreateAddNVP("FunnyContent", PType.VARCHAR, 2000)
            .CreateAddNVP("FunnyImgPath", PType.VARCHAR, 100)
            .CreateAddNVP("FunnyScore", DType.INT)
            .CreateAddNVP("FunnyIsCringe", DType.BIT)
            .InsertInto()
            .AddInsertValue("TestFunny")
            .AddInsertValue("This meme is absolutely ready for testing.")
            .AddInsertValue("")
            .AddInsertValue(new int[2] { 8, 0 })
            .Update().Set("FunnyImgPath", "null")
            .Where("FunnyID", Op.EQAL, 1)
            .InsertInto().AddInsertValue("Chris cringle")
            .AddInsertValue("THis meme is utter cringe")
            .AddInsertValue("cringle.jpeg")
            .AddInsertValue(2)
            .AddInsertValue(1)
            .InsertIntoCol().AddInsertColumn(new string[] { "FunnyName", "FunnyContent" })
            .AddInsertValue(new string[] { "guh?", "reveal" })
            .Select("*").From();
        ;

        query = new QueryBuilder().SelectDistinct("FunnyScore").AddSelectColumn("FunnyName").AddSelectColumn("FunnyID").From("Meme")
            .Select().Top(2).From().Where("FunnyID", Op.EQAL, 0)
            .Select().TopPercent(3).From().Where("FunnyID", Op.EQAL, 1);
        
        c.WriteLine(query.Query);
    }
}