namespace CryptoBackend.Models
{
    public class Fee
    {
        private decimal percentageFee;
        private decimal fixedFee;

        public decimal Percentage { get => percentageFee; set => percentageFee = value; }
        public decimal Fixed { get => fixedFee; set => fixedFee = value; }
    }
}