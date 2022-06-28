namespace SQLHelpers
{
    public class QueryPropertyNullException : Exception
    {
        public QueryPropertyNullException() : base() { }
        public QueryPropertyNullException(string propName) : base($"Property {propName} was null.") { }
        public override string ToString() => base.ToString();
    }
}
