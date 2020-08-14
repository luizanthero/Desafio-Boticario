namespace boticario.Models
{
    public class Cashback
    {
        public int StatusCode { get; set; }

        public Body Body { get; set; }
    }

    public class Body
    {
        public int Credit { get; set; }
    }
}
