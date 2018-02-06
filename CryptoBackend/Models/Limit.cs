namespace CryptoBackend.Models
{
    public class Limit
    {
        private decimal minLimit;
        private decimal maxLimit;

        public decimal Min { get => minLimit; set => minLimit = value; }
        public decimal Max { get => maxLimit; set => maxLimit = value; }
    }
}